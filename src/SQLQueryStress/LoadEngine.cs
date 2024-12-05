using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
}
#pragma warning restore CA1031 // Do not catch general exception types