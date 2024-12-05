using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using SQLQueryStress.Controls;

namespace SQLQueryStress;

public partial class LoadEngine
{
    public sealed class QueryInput : IDisposable
    {
        private static QueryOutput _outInfo=new();
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
            int iterations, bool forceDataRetrieval, int queryDelay, BackgroundWorker backgroundWorker, bool killQueriesOnCancel, int numWorkerThreads, int threadNumber,GanttChartControl ganttChart)
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
}