using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.XEvent.XELite;
using SQLQueryStress.Controls;

namespace SQLQueryStress;
#pragma warning disable CA1031 // Do not catch general exception types
public partial class LoadEngine
{
    private static BlockingCollection<QueryOutput> QueryOutInfo;
    private static int _finishedThreads;
    private readonly int _commandTimeout;
    private readonly string _connectionString;
    private readonly ConcurrentDictionary<Guid, List<IXEvent>> _events = new();
    private readonly bool _forceDataRetrieval;
    private readonly GanttChartControl _ganttChart;
    private readonly int _iterations;
    private readonly bool _killQueriesOnCancel;
    private readonly string _paramConnectionString;
    private readonly Dictionary<string, string> _paramMappings;
    private readonly string _paramQuery;
    private readonly string _query;
    private readonly List<Thread> _threadPool = new();

    private readonly int _threads;
    private Task _extendedEventReaderTask;
    private ExtendedEventsReader _extendedEventsReader;
    private int _queryDelay;

    public LoadEngine(string connectionString, string query, int threads, int iterations, string paramQuery,
        Dictionary<string, string> paramMappings, string paramConnectionString, int commandTimeout,
        bool collectIoStats, bool collectTimeStats, bool forceDataRetrieval, bool killQueriesOnCancel,
        CancellationTokenSource cts, GanttChartControl ganttChart,
        ConcurrentDictionary<Guid, List<IXEvent>> events)
    {
        _events = events;
        //Set the min pool size so that the pool does not have
        //to get allocated in real-time
        var builder = new SqlConnectionStringBuilder(connectionString)
        {
            MinPoolSize = threads,
            MaxPoolSize = threads,
            CurrentLanguage = "English"
        };
        QueryOutInfo = new BlockingCollection<QueryOutput>();
        _connectionString = builder.ConnectionString;
        _query = query;
        _threads = threads;
        _iterations = iterations;
        _paramQuery = paramQuery;
        _paramMappings = paramMappings;
        _paramConnectionString = paramConnectionString;
        _commandTimeout = commandTimeout;
        _forceDataRetrieval = forceDataRetrieval;
        _killQueriesOnCancel = killQueriesOnCancel;
        _ganttChart = ganttChart;
    }

    public static bool ExecuteCommand(string connectionString, string sql)
    {
        using var conn = new SqlConnection(connectionString);
        conn.Open();
#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
        using var cmd = new SqlCommand(sql, conn);
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities
        cmd.ExecuteNonQuery();
        return true;
    }

    public void StartLoad(BackgroundWorker worker, int queryDelay)
    {
        _queryDelay = queryDelay;

        StartLoad(worker);
    }

    private void StartLoad(BackgroundWorker worker)
    {
        var useParams = false;

        var badParams = new List<string>();
        foreach (var theKey in _paramMappings.Keys)
            if (_paramMappings[theKey] == null || _paramMappings[theKey].Length == 0)
                badParams.Add(theKey);

        foreach (var theKey in badParams) _paramMappings.Remove(theKey);

        //Need some parameters?
        if (_paramMappings.Count > 0)
        {
            ParamServer.Initialize(_paramQuery, _paramConnectionString, _paramMappings);
            useParams = true;
        }

        var exEventCTS = new CancellationTokenSource();
        var exToken = exEventCTS.Token;

        _extendedEventsReader = new ExtendedEventsReader(_connectionString, exToken, _events);
        _extendedEventsReader.StartSession().GetAwaiter().GetResult();
        _extendedEventReaderTask = _extendedEventsReader.ReadEventsLoop();

        //Initialize the connection pool            
        var conn = new SqlConnection(_connectionString);
        //TODO: use this or not??
        SqlConnection.ClearPool(conn);
        conn.Open();
        conn.Dispose();
        using var workerCTS = new CancellationTokenSource();
        var token = workerCTS.Token;
        //Spin up the load threads
        for (var i = 0; i < _threads; i++)
        {
            conn = new SqlConnection(_connectionString);

            //TODO: Figure out how to make this option work (maybe)
            //conn.FireInfoMessageEventOnUserErrors = true;

            SqlCommand statsComm = null;

#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
            var queryComm = new SqlCommand
                { CommandTimeout = _commandTimeout, Connection = conn, CommandText = _query };
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities

            if (useParams) queryComm.Parameters.AddRange(ParamServer.GetParams());

            var input = new QueryInput(statsComm, queryComm,
                _iterations, _forceDataRetrieval, _queryDelay, worker, _killQueriesOnCancel,
                _threads, i, _ganttChart);

            var theThread = new Thread(input.StartLoadThread)
                { Priority = ThreadPriority.BelowNormal, IsBackground = true };
            theThread.Name = "thread: " + i;
            Debug.WriteLine($"Thread {i} Created");
            _threadPool.Add(theThread);
        }

        // create a token source for the workers to be able to listen to a cancel event
        _finishedThreads = 0;

        //Start reading the queue...
        var cancelled = false;


        _finishedThreads = 0;
        for (var i = 0; i < _threads; i++)
        {
            _threadPool[i].Start(workerCTS.Token);
            Debug.WriteLine($"Thread {_threadPool[i].Name} Started");
        }
        Debug.WriteLine("Threads Started");

        while (!cancelled)
        {
            Task.Delay(250).GetAwaiter().GetResult();
           // QueryOutput theOut = null;
            try
            {
                processOuts(worker);

                bool allComplete = true;
                for (var i = 0; i < _threads; i++)
                {
                    if (_threadPool[i].IsAlive)
                    {
                        allComplete = false;
                    }
                }

                if (allComplete) break;
            }
            catch (Exception ex)
            {
                // The exception is InvalidOperationException if the threads are done
                // and OperationCanceledException if the user clicked cancel.
                // If it's OperationCanceledException, we need to cancel
                // the worker threads and wait for them to exit
                if (ex is OperationCanceledException)
                {
                    workerCTS.Cancel();
                    foreach (var theThread in _threadPool)
                        // give the thread max 5 seconds to cancel nicely
                        if (!theThread.Join(5000))
                            theThread.Interrupt();
                }

                SqlConnection.ClearAllPools();
                cancelled = true;
                Debug.WriteLine($"Exception caught and loop cancelled \r\n {ex}");
            }

        }
        Debug.WriteLine("Threads Completed");
        QueryOutInfo.CompleteAdding();
      
        worker.ReportProgress(100, null);


        _ = Task.Run(() =>
        {
            Task.Delay(10000).GetAwaiter().GetResult();
            Debug.WriteLine("Cancelling Reader");
            exEventCTS.Cancel();
            Debug.WriteLine("Reader Cancelled");
            Task.Delay(10000).GetAwaiter().GetResult();
            Debug.WriteLine("Calling Dispose of reader");
            _extendedEventsReader.Dispose();
            _extendedEventReaderTask.Dispose();

            exEventCTS.Dispose();
            Debug.WriteLine("Disposing Done");
        });
    }

    private void processOuts(BackgroundWorker worker)
    {
        Debug.WriteLine("In processOuts");
        while (true)
        {
            if (!QueryOutInfo.TryTake(out var theOut))
                break;

            _ganttChart.AddGanttItem(theOut.ThreadNumber, theOut.startTime, (int)theOut.Time.TotalMilliseconds, theOut);

        }

        GanttMessages.SendFitToData(_ganttChart);
    }

    private static class ParamServer
    {
        private static int _currentRow;
        private static int _numRows;

        //The actual params that will be filled
        private static SqlParameter[] _outputParams;
        //Map the param columns to ordinals in the data table
        private static int[] _paramDtMappings;
        private static DataTable _theParams;

        public static void GetNextRow_Values(SqlParameterCollection newParam)
        {
            var rowNum = Interlocked.Increment(ref _currentRow);
            var dr = _theParams.Rows[rowNum % _numRows];

            for (var i = 0; i < _outputParams.Length; i++)
            {
                newParam[i].Value = dr[_paramDtMappings[i]];
            }
        }

        public static SqlParameter[] GetParams()
        {
            var newParam = new SqlParameter[_outputParams.Length];

            for (var i = 0; i < _outputParams.Length; i++)
            {
                newParam[i] = (SqlParameter)((ICloneable)_outputParams[i]).Clone();
            }

            return newParam;
        }

        public static void Initialize(string paramQuery, string connString, Dictionary<string, string> paramMappings)
        {
#pragma warning disable CA2100
            using var sqlDataAdapter = new SqlDataAdapter(paramQuery, connString);
#pragma warning restore CA2100
            _theParams = new DataTable();
            sqlDataAdapter.Fill(_theParams);

            _numRows = _theParams.Rows.Count;

            _outputParams = new SqlParameter[paramMappings.Keys.Count];
            _paramDtMappings = new int[paramMappings.Keys.Count];

            //Populate the array of parameters that will be cloned and filled
            //on each request
            var i = 0;
            foreach (var parameterName in paramMappings.Keys)
            {
                _outputParams[i] = new SqlParameter { ParameterName = parameterName };
                var paramColumn = paramMappings[parameterName];

                //if there is a param mapped to this column
                if (paramColumn != null)
                    _paramDtMappings[i] = _theParams.Columns[paramColumn].Ordinal;

                i++;
            }
        }
    }

    public sealed class QueryInput : IDisposable
    {
        private static QueryOutput _outInfo = new();
        private readonly SqlCommand _queryComm;
        private readonly Stopwatch _sw = new Stopwatch();
        private readonly System.Timers.Timer _killTimer = new System.Timers.Timer();
        private readonly bool _forceDataRetrieval;
        private readonly int _iterations;
        private readonly int _queryDelay;
        private readonly int _threadNumber;
        private readonly BackgroundWorker _backgroundWorker;
        private readonly GanttChartControl _ganntChart;

        public QueryInput(SqlCommand statsComm, SqlCommand queryComm,
            //                Queue<queryOutput> queryOutInfo,
            int iterations, bool forceDataRetrieval, int queryDelay, BackgroundWorker backgroundWorker, bool killQueriesOnCancel, int numWorkerThreads, int threadNumber, GanttChartControl ganttChart)
        {
            _queryComm = queryComm;
            _iterations = iterations;
            _forceDataRetrieval = forceDataRetrieval;
            _queryDelay = queryDelay;
            _ganntChart = ganttChart;

            this._backgroundWorker = backgroundWorker;

            if (killQueriesOnCancel)
            {
                _killTimer.Interval = 2000;
                _killTimer.Elapsed += KillTimer_Elapsed;
                _killTimer.Enabled = true;
            }

            _threadNumber = threadNumber;
        }

        private void KillTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_backgroundWorker.CancellationPending)
            {
                _queryComm.Cancel();
                _killTimer.Enabled = false;
            }
            else if (_queryComm.Connection == null || _queryComm.Connection.State == ConnectionState.Closed)
            {
                _killTimer.Enabled = false;
            }
        }

        static string ConvertGuidToHexString(Guid guid)
        {
            var bytes = guid.ToByteArray();
            var binaryString = new StringBuilder();
            foreach (var b in bytes)
            {
                binaryString.Append(Convert.ToString(b, 16).PadLeft(2, '0'));
            }
            return binaryString.ToString();
        }

        public void StartLoadThread(Object token)
        {
            bool runCancelled = false;

            CancellationToken ctsToken = (CancellationToken)token;
            try
            {
                ctsToken.Register(() =>
                {
                    // Cancellation on the token will interrupt and cancel the thread
                    runCancelled = true;
                    _queryComm.Cancel();
                });
                //do the work
                using var conn = _queryComm.Connection;
                var startTime = DateTime.Now;
                var endTime = DateTime.Now;
                var context = Guid.Empty;
                for (var i = 0; i < _iterations && !runCancelled; i++)
                {
                    Exception outException = null;

                    try
                    {
                        context = Guid.NewGuid();
                        if (conn != null)
                        {
                            conn.Open();
                        }
                        var contextcmd = new SqlCommand($"SET CONTEXT_INFO 0x{ConvertGuidToHexString(context)};", conn);
                        contextcmd.ExecuteNonQuery();
                        //Params are assigned only once -- after that, their values are dynamically retrieved
                        if (_queryComm.Parameters.Count > 0)
                        {
                            ParamServer.GetNextRow_Values(_queryComm.Parameters);
                        }

                        _sw.Start();
                        startTime = DateTime.Now;
                        //TODO: This could be made better
                        if (_forceDataRetrieval)
                        {
                            var reader = _queryComm.ExecuteReader();

                            do
                            {
                                while (!runCancelled && reader.Read())
                                {
                                    //grab the first column to force the row down the pipe
                                    // ReSharper disable once UnusedVariable
                                    var x = reader[0];
                                }
                            } while (!runCancelled && reader.NextResult());
                        }
                        else
                        {
                            _queryComm.ExecuteNonQuery();
                        }
                        endTime = DateTime.Now;
                        _sw.Stop();
                        Debug.WriteLine($"Thread {Thread.CurrentThread.Name} ReadDone Iteration {i}");
                    }
                    catch (Exception ex)
                    {
                        if (!runCancelled)
                            outException = ex;

                        if (_sw.IsRunning)
                        {
                            endTime = DateTime.Now;
                            _sw.Stop();
                        }
                    }
                    finally
                    {
                        //Clean up the connection
                        if (conn != null)
                        {
                            conn.Close();
                        }
                    }

                    var finished = i == _iterations - 1;

                    _outInfo.E = outException;
                    _outInfo.Time = _sw.Elapsed;
                    _outInfo.Finished = finished;
                    _outInfo.startTime = startTime;
                    _outInfo.endTime = endTime;
                    _outInfo.context = context;
                    _outInfo.ThreadNumber = _threadNumber;
                    var copyOutInfo = new QueryOutput()
                    {
                        E = outException,
                        Time = _sw.Elapsed,
                        Finished = finished,
                        startTime = startTime,
                        endTime = endTime,
                        context = context,
                        ThreadNumber = _threadNumber,
                        ActiveThreads = _outInfo.ActiveThreads,
                        CpuTime = _outInfo.CpuTime,
                        ElapsedTime = _outInfo.ElapsedTime,
                        LogicalReads = _outInfo.LogicalReads
                    };

                    QueryOutInfo.Add(copyOutInfo);
                    _sw.Reset();

                    if (!runCancelled)
                    {
                        try
                        {
                            if (_queryDelay > 0)
                                Task.Delay(_queryDelay, ctsToken).GetAwaiter().GetResult();
                        }
                        catch (AggregateException ae)
                        {
                            ae.Handle((x) =>
                            {
                                if (x is TaskCanceledException)
                                {
                                    runCancelled = true;
                                    return true;
                                }
                                // if we get here, the exception wasn't a cancel
                                // so don't swallow it
                                return false;
                            });
                        }
                    }
                }
            }
            catch (Exception)
            {
                if (runCancelled)
                {
                    _outInfo.Time = new TimeSpan(0);
                    _outInfo.Finished = true;
                    QueryOutInfo.Add(_outInfo);
                }
            }
            var t = Interlocked.Increment(ref _finishedThreads);
            Debug.WriteLine($"Thread {Thread.CurrentThread.Name} Completed , FT ={t}");

        }

        public void Dispose()
        {
            _killTimer.Dispose();
        }
    }

    public class QueryOutput
    {
        public DateTime startTime;
        public DateTime endTime;
        public Guid context;
        public int CpuTime;
        public Exception E;
        public int ElapsedTime;
        public bool Finished;
        public int LogicalReads;
        public TimeSpan Time;
        public int ThreadNumber;
        // Remaining active threads for the load
        public int ActiveThreads;

    }
}
#pragma warning restore CA1031 // Do not catch general exception types