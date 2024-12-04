using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
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
    private readonly bool _collectIoStats;
    private readonly bool _collectTimeStats;
    private readonly List<SqlCommand> _commandPool = new();
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
    private readonly List<Task> _threadPool = new();
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
        _collectIoStats = collectIoStats;
        _collectTimeStats = collectTimeStats;
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

            using var input = new QueryInput(statsComm, queryComm,
                _iterations, _forceDataRetrieval, _queryDelay, worker, _killQueriesOnCancel, _threads, i, _ganttChart);

            var theThread = input.StartLoadThread(token);

            _threadPool.Add(theThread);
            _commandPool.Add(queryComm);
        }

        // create a token source for the workers to be able to listen to a cancel event
        _finishedThreads = 0;

        //Start reading the queue...
        var cancelled = false;


        while (_threadPool.Any(x => !x.IsCompleted))
            try
            {
                Task.Delay(250).GetAwaiter().GetResult();

                processOuts(worker);
            }
            catch (Exception ex)
            {
                // The exception is InvalidOperationException if the threads are done
                // and OperationCanceledException if the user clicked cancel.
                // If it's OperationCanceledException, we need to cancel
                // the worker threads and wait for them to exit
                if (ex is OperationCanceledException) workerCTS.Cancel();
                SqlConnection.ClearAllPools();
                cancelled = true;
            }

        cancelled = true;
        QueryOutInfo.CompleteAdding();
        processOuts(worker);
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
        var finishedThreads = _threadPool.Count(x => x.IsCompleted);
        var totalThreads = _threadPool.Count();

        while (true)
        {
            if (!QueryOutInfo.TryTake(out var theOut)) break;

            worker.ReportProgress((int)(finishedThreads / (decimal)totalThreads * 100), theOut);
        }
    }
}
#pragma warning restore CA1031 // Do not catch general exception types