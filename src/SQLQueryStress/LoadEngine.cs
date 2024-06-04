using Microsoft.Data.SqlClient;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SQLQueryStress
{
#pragma warning disable CA1031 // Do not catch general exception types
    sealed class LoadEngine
    {
        private static BlockingCollection<QueryOutput> QueryOutInfo;
        private static CancellationTokenSource _backgroundWorkerCTS;
        private readonly bool _collectIoStats;
        private readonly bool _collectTimeStats;
        private readonly List<SqlCommand> _commandPool = new List<SqlCommand>();
        private readonly int _commandTimeout;

        private readonly string _connectionString;
        private readonly bool _forceDataRetrieval;
        private readonly bool _killQueriesOnCancel;
        private readonly int _iterations;
        private readonly string _paramConnectionString;
        private readonly Dictionary<string, string> _paramMappings;
        //private readonly List<Queue<queryOutput>> queryOutInfoPool = new List<Queue<queryOutput>>();        
        private readonly string _paramQuery;
        private readonly string _query;
        private readonly List<Thread> _threadPool = new List<Thread>();
        private readonly int _threads;
        private static int _finishedThreads;
        private int _queryDelay;

        public LoadEngine(string connectionString, string query, int threads, int iterations, string paramQuery,
            Dictionary<string, string> paramMappings, string paramConnectionString, int commandTimeout,
            bool collectIoStats, bool collectTimeStats, bool forceDataRetrieval, bool killQueriesOnCancel, CancellationTokenSource cts)
        {
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
            _collectIoStats = collectIoStats;
            _collectTimeStats = collectTimeStats;
            _forceDataRetrieval = forceDataRetrieval;
            _killQueriesOnCancel = killQueriesOnCancel;
            _backgroundWorkerCTS = cts;
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
            {
                if ((_paramMappings[theKey] == null) || (_paramMappings[theKey].Length == 0))
                {
                    badParams.Add(theKey);
                }
            }

            foreach (var theKey in badParams)
            {
                _paramMappings.Remove(theKey);
            }

            //Need some parameters?
            if (_paramMappings.Count > 0)
            {
                ParamServer.Initialize(_paramQuery, _paramConnectionString, _paramMappings);
                useParams = true;
            }

            //Initialize the connection pool            
            var conn = new SqlConnection(_connectionString);
            //TODO: use this or not??
            SqlConnection.ClearPool(conn);
            conn.Open();
            conn.Dispose();

            //Spin up the load threads
            for (var i = 0; i < _threads; i++)
            {
                conn = new SqlConnection(_connectionString);

                //TODO: Figure out how to make this option work (maybe)
                //conn.FireInfoMessageEventOnUserErrors = true;

                SqlCommand statsComm = null;

#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
                var queryComm = new SqlCommand { CommandTimeout = _commandTimeout, Connection = conn, CommandText = _query };
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities

                if (useParams)
                {
                    queryComm.Parameters.AddRange(ParamServer.GetParams());
                }

                var setStatistics = (_collectIoStats ? @"SET STATISTICS IO ON;" : string.Empty) + (_collectTimeStats ? @"SET STATISTICS TIME ON;" : string.Empty);

                if (setStatistics.Length > 0)
                {
                    statsComm = new SqlCommand { CommandTimeout = _commandTimeout, Connection = conn, CommandText = setStatistics };
                }

                //Queue<queryOutput> queryOutInfo = new Queue<queryOutput>();

                using var input = new QueryInput(statsComm, queryComm,
                    //                    this.queryOutInfo,
                    _iterations, _forceDataRetrieval, _queryDelay, worker, _killQueriesOnCancel, _threads);

                var theThread = new Thread(input.StartLoadThread) { Priority = ThreadPriority.BelowNormal, IsBackground = true };
                theThread.Name = "thread: " + i;

                _threadPool.Add(theThread);
                _commandPool.Add(queryComm);
                //queryOutInfoPool.Add(queryOutInfo);
            }
            // create a token source for the workers to be able to listen to a cancel event
            using var workerCTS = new CancellationTokenSource();
            _finishedThreads = 0;
            for (var i = 0; i < _threads; i++)
            {
                _threadPool[i].Start(workerCTS.Token);
            }

            //Start reading the queue...
            var cancelled = false;

            while (!cancelled)
            {
                QueryOutput theOut = null;
                try
                {
                    // wait for OutInfo items in the queue or a user cancel event
                    theOut = QueryOutInfo.Take(_backgroundWorkerCTS.Token);
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
                        {
                            // give the thread max 5 seconds to cancel nicely
                            if (!theThread.Join(5000))
                                theThread.Interrupt();
                        }
                    }
                    SqlConnection.ClearAllPools();
                    cancelled = true;
                }

                if (theOut != null)
                {
                    //Report output to the UI
                    int finishedThreads = Interlocked.CompareExchange(ref _finishedThreads, 0, 0);
                    theOut.ActiveThreads = _threads - finishedThreads;
                    worker.ReportProgress((int)(_finishedThreads / (decimal)_threads * 100), theOut);
                }
            }
        }


        //TODO: Monostate pattern to be investigated (class is never instantiated)
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

        private sealed class QueryInput : IDisposable
        {
            [ThreadStatic] private static QueryOutput _outInfo;

            //This regex is used to find the number of logical reads
            //in the messages collection returned in the queryOutput class
            private static readonly Regex FindReads = new Regex(@"(?:Table (\'\w{1,}\'|'#\w{1,}\'|'##\w{1,}\'). Scan count \d{1,}, logical reads )(\d{1,})", RegexOptions.Compiled);

            //This regex is used to find the CPU and elapsed time
            //in the messages collection returned in the queryOutput class
            private static readonly Regex FindTimes =
                new Regex(
                    @"(?:SQL Server Execution Times:|SQL Server parse and compile time:)(?:\s{1,}CPU time = )(\d{1,})(?: ms,\s{1,}elapsed time = )(\d{1,})",
                    RegexOptions.Compiled);

            private readonly SqlCommand _queryComm;

            private readonly SqlCommand _statsComm;

            //private static Dictionary<int, List<string>> theInfoMessages = new Dictionary<int, List<string>>();

            private readonly Stopwatch _sw = new Stopwatch();
            private readonly System.Timers.Timer _killTimer = new System.Timers.Timer();
            private readonly bool _forceDataRetrieval;
            //          private readonly Queue<queryOutput> queryOutInfo;
            private readonly int _iterations;
            private readonly int _queryDelay;
            private readonly int _numWorkerThreads;
            private readonly BackgroundWorker _backgroundWorker;

            public QueryInput(SqlCommand statsComm, SqlCommand queryComm,
                //                Queue<queryOutput> queryOutInfo,
                int iterations, bool forceDataRetrieval, int queryDelay, BackgroundWorker _backgroundWorker, bool killQueriesOnCancel, int numWorkerThreads)
            {
                _statsComm = statsComm;
                _queryComm = queryComm;
                //                this.queryOutInfo = queryOutInfo;
                _iterations = iterations;
                _forceDataRetrieval = forceDataRetrieval;
                _queryDelay = queryDelay;
                _numWorkerThreads = numWorkerThreads;

                //Prepare the infoMessages collection, if we are collecting statistics
                //if (stats_comm != null)
                //    theInfoMessages.Add(stats_comm.Connection.GetHashCode(), new List<string>());

                this._backgroundWorker = _backgroundWorker;

                if (killQueriesOnCancel)
                {
                    _killTimer.Interval = 2000;
                    _killTimer.Elapsed += KillTimer_Elapsed;
                    _killTimer.Enabled = true;
                }
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

            private static void GetInfoMessages(object sender, SqlInfoMessageEventArgs args)
            {
                foreach (SqlError err in args.Errors)
                {
                    var matches = FindReads.Split(err.Message);

                    //we have a read
                    if (matches.Length > 1)
                    {
                        _outInfo.LogicalReads += Convert.ToInt32(matches[2], CultureInfo.InvariantCulture);
                        continue;
                    }

                    matches = FindTimes.Split(err.Message);

                    //we have times
                    if (matches.Length > 1)
                    {
                        _outInfo.CpuTime += Convert.ToInt32(matches[1], CultureInfo.InvariantCulture);
                        _outInfo.ElapsedTime += Convert.ToInt32(matches[2], CultureInfo.InvariantCulture);
                    }
                }
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
                        _statsComm.Cancel();
                        _queryComm.Cancel();
                    });
                    //do the work
                    using (var conn = _queryComm.Connection)
                    {
                        SqlInfoMessageEventHandler handler = GetInfoMessages;

                        for (var i = 0; i < _iterations && !runCancelled; i++)
                        {
                            Exception outException = null;

                            try
                            {
                                //initialize the outInfo structure
                                _outInfo = new QueryOutput();

                                if (conn != null)
                                {
                                    conn.Open();

                                    //set up the statistics gathering
                                    if (_statsComm != null)
                                    {
                                        _statsComm.ExecuteNonQuery();
                                        conn.InfoMessage += handler;
                                    }
                                }

                                //Params are assigned only once -- after that, their values are dynamically retrieved
                                if (_queryComm.Parameters.Count > 0)
                                {
                                    ParamServer.GetNextRow_Values(_queryComm.Parameters);
                                }

                                _sw.Start();

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

                                _sw.Stop();
                            }
                            catch (Exception ex)
                            {
                                if (!runCancelled)
                                    outException = ex;

                                if (_sw.IsRunning)
                                {
                                    _sw.Stop();
                                }
                            }
                            finally
                            {
                                //Clean up the connection
                                if (conn != null)
                                {
                                    if (_statsComm != null)
                                    {
                                        conn.InfoMessage -= handler;
                                    }
                                    conn.Close();
                                }
                            }

                            var finished = i == _iterations - 1;

                            //List<string> infoMessages = null;

                            //infoMessages = (stats_comm != null) ? theInfoMessages[connectionHashCode] : null;

                            /*
                            queryOutput theout = new queryOutput(
                                outException,
                                sw.Elapsed,
                                finished,
                                (infoMessages == null || infoMessages.Count == 0) ? null : infoMessages.ToArray());
                             */

                            _outInfo.E = outException;
                            _outInfo.Time = _sw.Elapsed;
                            _outInfo.Finished = finished;

                            QueryOutInfo.Add(_outInfo);

                            //Prep the collection for the next round
                            //if (infoMessages != null && infoMessages.Count > 0)
                            //    infoMessages.Clear();

                            _sw.Reset();

                            if (!runCancelled)
                            {
                                try
                                {
                                    if (_queryDelay > 0)
                                        Task.Delay(_queryDelay, ctsToken).Wait();
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
                }
                catch (Exception)
                {
                    if (runCancelled)
                    {
                        //queryOutput theout = new queryOutput(null, new TimeSpan(0), true, null);
                        _outInfo.Time = new TimeSpan(0);
                        _outInfo.Finished = true;
                        QueryOutInfo.Add(_outInfo);
                    }
                }
                Interlocked.Increment(ref _finishedThreads);
                if (_finishedThreads == _numWorkerThreads)
                {
                    // once all of the threads have exited, tell the other side that we're done adding items to the collection
                    QueryOutInfo.CompleteAdding();
                }
            }

            public void Dispose()
            {
                _killTimer.Dispose();
            }
        }

        internal sealed class QueryOutput
        {
            public int CpuTime;
            public Exception E;
            public int ElapsedTime;
            public bool Finished;
            public int LogicalReads;
            public TimeSpan Time;

            // Remaining active threads for the load
            public int ActiveThreads;

            /*
            public queryOutput(
                Exception e, 
                TimeSpan time, 
                bool finished,
                string[] infoMessages)
            {
                this.e = e;
                this.time = time;
                this.finished = finished;
                this.infoMessages = infoMessages;
            }
             */
        }
    }
}
#pragma warning restore CA1031 // Do not catch general exception types