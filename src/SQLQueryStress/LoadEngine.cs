using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Diagnostics;

namespace SQLQueryStress
{
    class LoadEngine
    {
        private static Queue<queryOutput> queryOutInfo = new Queue<queryOutput>();

        private readonly string connectionString;
        private readonly string query;
        private readonly int threads;
        private readonly int iterations;
        private readonly List<Thread> threadPool = new List<Thread>();
        private readonly List<SqlCommand> commandPool = new List<SqlCommand>();
        //private readonly List<Queue<queryOutput>> queryOutInfoPool = new List<Queue<queryOutput>>();        
        private readonly string paramQuery;
        private readonly Dictionary<string, string> paramMappings;
        private readonly string paramConnectionString;
        private readonly int commandTimeout;
        private readonly bool collectIOStats;
        private readonly bool collectTimeStats;
        private readonly bool forceDataRetrieval;

        public LoadEngine(  string connectionString, 
                            string query, 
                            int threads, 
                            int iterations,
                            string paramQuery,
                            Dictionary<string, string> paramMappings,
                            string paramConnectionString,
                            int commandTimeout,
                            bool collectIOStats,
                            bool collectTimeStats,
                            bool forceDataRetrieval)
        {
            //Set the min pool size so that the pool does not have
            //to get allocated in real-time
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
            builder.MinPoolSize = threads;

            this.connectionString = builder.ConnectionString;
            this.query = query;
            this.threads = threads;
            this.iterations = iterations;
            this.paramQuery = paramQuery;
            this.paramMappings = paramMappings;
            this.paramConnectionString = paramConnectionString;
            this.commandTimeout = commandTimeout;
            this.collectIOStats = collectIOStats;
            this.collectTimeStats = collectTimeStats;
            this.forceDataRetrieval = forceDataRetrieval;
        }

        public void StartLoad(BackgroundWorker worker)
        {
            bool useParams = false;

            List<string> badParams = new List<string>();
            foreach (string theKey in paramMappings.Keys)
            {
                if ((paramMappings[theKey] == null) ||
                    (paramMappings[theKey].Length == 0))
                {
                    badParams.Add(theKey);
                }
            }

            foreach (string theKey in badParams)
            {
                paramMappings.Remove(theKey);
            }

            //Need some parameters?
            if (paramMappings.Count > 0)
            {
                ParamServer.Initialize(paramQuery, paramConnectionString, paramMappings);
                useParams = true;
            }

            //Initialize the connection pool            
            SqlConnection conn = new SqlConnection(this.connectionString);
            //TODO: use this or not??
            SqlConnection.ClearPool(conn);
            conn.Open();
            conn.Dispose();

            //make sure the run cancelled flag is not set
            queryInput.RunCancelled = false;

            //Spin up the load threads
            for (int i = 0; i < threads; i++)
            {
                conn = new SqlConnection(this.connectionString);

                //TODO: Figure out how to make this option work (maybe)
                //conn.FireInfoMessageEventOnUserErrors = true;

                SqlCommand stats_comm = null;

                SqlCommand query_comm = new SqlCommand();
                query_comm.CommandTimeout = this.commandTimeout;
                query_comm.Connection = conn;
                query_comm.CommandText = this.query;

                if (useParams)
                {
                    query_comm.Parameters.AddRange(ParamServer.GetParams());
                }

                string setStatistics =
                    ((collectIOStats) ? (@"SET STATISTICS IO ON;") : ("")) +
                    ((collectTimeStats) ? (@"SET STATISTICS TIME ON;") : (""));

                if (setStatistics.Length > 0)
                {
                    stats_comm = new SqlCommand();
                    stats_comm.CommandTimeout = this.commandTimeout;
                    stats_comm.Connection = conn;
                    stats_comm.CommandText = setStatistics;
                }

                //Queue<queryOutput> queryOutInfo = new Queue<queryOutput>();

                queryInput input = new queryInput(
                    stats_comm,
                    query_comm,
//                    this.queryOutInfo,
                    this.iterations, 
                    this.forceDataRetrieval);

                Thread theThread = new Thread(new ThreadStart(input.startLoadThread));
                theThread.Priority = ThreadPriority.BelowNormal;

                threadPool.Add(theThread);
                commandPool.Add(query_comm);
                //queryOutInfoPool.Add(queryOutInfo);
            }

            //Start the load threads
            for (int i = 0; i < threads; i++)
            {
                threadPool[i].Start();
            }

            //Start reading the queue...
            int finishedThreads = 0;
            bool cancelled = false;

            while (finishedThreads < threads)
            {
//                for (int i = 0; i < threads; i++)
//                {
                   // try
                   // {
                        queryOutput theOut = null;
                        //lock (queryOutInfoPool[i])
                        lock(queryOutInfo)
                        {
                            //if (queryOutInfoPool[i].Count > 0)
                                //theOut = (queryOutput)queryOutInfoPool[i].Dequeue();
                            if (queryOutInfo.Count > 0)
                                theOut = queryOutInfo.Dequeue();
                            else
                                Monitor.Wait(queryOutInfo);
                        }

                        if (theOut != null)
                        {
                            //Report output to the UI
                            worker.ReportProgress((int)(((decimal)finishedThreads / (decimal)threads) * 100), theOut);

                            //TODO: Make this actually remove the queue from the pool so that it's not checked again -- maintain this with a bitmap, perhaps?
                            if (theOut.finished)
                                finishedThreads++;
                        }
                   /* }
                    catch (InvalidOperationException e)
                    {
                    }
                    */

                    /*
                        if (theOut != null)
                            Thread.Sleep(200);
                        else
                            Thread.Sleep(10);
                     */
 //               }

                //TODO: Remove this ?
                GC.Collect();

                if (worker.CancellationPending && (!cancelled))
                {
                    queryInput.RunCancelled = true;

                    //First, kill connections as fast as possible
                    SqlConnection.ClearAllPools();

                    //for each 20 threads, create a new thread dedicated
                    //to killing them
                    int threadNum = threadPool.Count;

                    List<Thread> killerThreads = new List<Thread>();
                    while (threadNum > 0)
                    {
                        int i = (threadNum <= 20) ? 0 : (threadNum - 20);

                        Thread[] killThreads = new Thread[((threadNum-i)<1) ? threadNum : (threadNum-i)];
                        SqlCommand[] killCommands = new SqlCommand[((threadNum - i) < 1) ? threadNum : (threadNum - i)];

                        threadPool.CopyTo(
                            i, killThreads, 0, killThreads.Length);
                        commandPool.CopyTo(
                            i, killCommands, 0, killCommands.Length);

                        for (int j = (threadNum-1); j >= i; j--)
                        {
                            threadPool.RemoveAt(j);
                            commandPool.RemoveAt(j);
                        }

                        ThreadKiller kill = new ThreadKiller(
                            killThreads,
                            killCommands);
                        Thread killer = new Thread(new ThreadStart(kill.KillEm));
                        killer.Start();
                        Thread.Sleep(0);

                        killerThreads.Add(killer);

                        threadNum = i;
                    }

                    //wait for the kill threads to return
                    //before exiting...
                    foreach (Thread theThread in killerThreads)
                    {
                        theThread.Join();
                    }

                    cancelled = true;
                }
            }

            //clear any remaining messages -- these are almost certainly
            //execeptions due to thread cancellation
            //queryOutInfo.Clear();
        }

        private class queryInput
        {

            //This regex is used to find the number of logical reads
            //in the messages collection returned in the queryOutput class
            private static Regex findReads = new Regex(@"(?:Table \'\w{1,}\'. Scan count \d{1,}, logical reads )(\d{1,})", RegexOptions.Compiled);

            //This regex is used to find the CPU and elapsed time
            //in the messages collection returned in the queryOutput class
            private static Regex findTimes = new Regex(@"(?:SQL Server Execution Times:|SQL Server parse and compile time:)(?:\s{1,}CPU time = )(\d{1,})(?: ms,\s{1,}elapsed time = )(\d{1,})", RegexOptions.Compiled);

            [ThreadStatic]
            private static queryOutput outInfo;

            private readonly SqlCommand stats_comm;
            private readonly SqlCommand query_comm;
  //          private readonly Queue<queryOutput> queryOutInfo;
            private readonly int iterations;
            private readonly bool forceDataRetrieval;
            private static bool runCancelled;

            public static bool RunCancelled
            {
                set
                {
                    runCancelled = value;
                }
            }

            //private static Dictionary<int, List<string>> theInfoMessages = new Dictionary<int, List<string>>();

            private Stopwatch sw = new Stopwatch();

            public queryInput(
                SqlCommand stats_comm, 
                SqlCommand query_comm,
//                Queue<queryOutput> queryOutInfo,
                int iterations, 
                bool forceDataRetrieval)
            {
                this.stats_comm = stats_comm;
                this.query_comm = query_comm;
//                this.queryOutInfo = queryOutInfo;
                this.iterations = iterations;
                this.forceDataRetrieval = forceDataRetrieval;

                //Prepare the infoMessages collection, if we are collecting statistics
                //if (stats_comm != null)
                //    theInfoMessages.Add(stats_comm.Connection.GetHashCode(), new List<string>());
            }

            public void startLoadThread()
            {
                try
                {
                    //do the work
                    using (SqlConnection conn = query_comm.Connection)
                    {
                        int connectionHashCode = conn.GetHashCode();
                        SqlInfoMessageEventHandler handler = new SqlInfoMessageEventHandler(queryInput.GetInfoMessages);

                        for (int i = 0; i < iterations; i++)
                        {
                            if (runCancelled)
                                throw new Exception();

                            Exception outException = null;

                            try
                            {
                                //initialize the outInfo structure
                                queryInput.outInfo = new queryOutput();

                                conn.Open();

                                //set up the statistics gathering
                                if (stats_comm != null)
                                {
                                    stats_comm.ExecuteNonQuery();
                                    Thread.Sleep(0);
                                    conn.InfoMessage += handler;
                                }
                                
                                //Params are assigned only once -- after that, their values are dynamically retrieved
                                if (query_comm.Parameters.Count > 0)
                                {
                                    ParamServer.GetNextRow_Values(query_comm.Parameters);
                                }

                                sw.Start();

                                if (forceDataRetrieval)
                                {
                                    SqlDataReader reader = query_comm.ExecuteReader();
                                    Thread.Sleep(0);

                                    do
                                    {
                                        Thread.Sleep(0);

                                        while (reader.Read())
                                        {
                                            //grab the first column to force the row down the pipe
                                            object x = reader[0];
                                            Thread.Sleep(0);
                                        }

                                    } while (reader.NextResult());

                                }
                                else
                                {
                                    query_comm.ExecuteNonQuery();
                                    Thread.Sleep(0);
                                }

                                sw.Stop();
                            }
                            catch (Exception e)
                            {
                                if (runCancelled)
                                    throw;
                                else
                                    outException = e;

                                if (sw.IsRunning)
                                {
                                    sw.Stop();
                                }
                            } 
                            finally
                            {
                                //Clean up the connection
                                if (stats_comm != null)
                                    conn.InfoMessage -= handler;

                                conn.Close();
                            }

                            bool finished = (i == (iterations - 1)) ? true : false;

                            //List<string> infoMessages = null;

                            //infoMessages = (stats_comm != null) ? theInfoMessages[connectionHashCode] : null;

                            /*
                            queryOutput theout = new queryOutput(
                                outException,
                                sw.Elapsed,
                                finished,
                                (infoMessages == null || infoMessages.Count == 0) ? null : infoMessages.ToArray());
                             */

                            outInfo.e = outException;
                            outInfo.time = sw.Elapsed;
                            outInfo.finished = finished;

                            lock (LoadEngine.queryOutInfo)
                            {
                                LoadEngine.queryOutInfo.Enqueue(outInfo);
                                Monitor.Pulse(LoadEngine.queryOutInfo);
                            }

                            //Prep the collection for the next round
                            //if (infoMessages != null && infoMessages.Count > 0)
                            //    infoMessages.Clear();

                            sw.Reset();
                        }
                    }
                }
                catch                    
                {
                    if (runCancelled)
                    {
                        //queryOutput theout = new queryOutput(null, new TimeSpan(0), true, null);
                        outInfo.time = new TimeSpan(0);
                        outInfo.finished = true;

                        lock (LoadEngine.queryOutInfo)
                        {
                            LoadEngine.queryOutInfo.Enqueue(outInfo);
                        }
                    }
                    else
                        throw;
                }
            }

            private static void GetInfoMessages(
                object sender,
                SqlInfoMessageEventArgs args)
            {
                foreach (SqlError err in args.Errors)
                {
                    string[] matches = findReads.Split(err.Message);

                    //we have a read
                    if (matches.Length > 1)
                    {
                        queryInput.outInfo.LogicalReads += Convert.ToInt32(matches[1]);
                        continue;
                    }

                    matches = findTimes.Split(err.Message);

                    //we have times
                    if (matches.Length > 1)
                    {
                        queryInput.outInfo.CPUTime += Convert.ToInt32(matches[1]);
                        queryInput.outInfo.ElapsedTime += Convert.ToInt32(matches[2]);
                    }                    
                }
            }
        }

        private class ParamServer
        {
            private static DataTable theParams;

            //The actual params that will be filled
            private static SqlParameter[] outputParams;
            //Map the param columns to ordinals in the data table
            private static int[] paramDTMappings;
            
            private static int currentRow;
            private static int numRows;

            public static SqlParameter[] GetParams()
            {
                SqlParameter[] newParam = new SqlParameter[outputParams.Length];

                for (int i = 0; i < outputParams.Length; i++)
                {
                    newParam[i] = (SqlParameter)((ICloneable)outputParams[i]).Clone();
                }

                return(newParam);
            }

            public static void GetNextRow_Values(SqlParameterCollection newParam)
            {
                int rowNum = (int)Interlocked.Increment(ref currentRow);
                DataRow dr = theParams.Rows[rowNum % numRows];

                for (int i = 0; i < outputParams.Length; i++)
                {
                    newParam[i].Value = dr[paramDTMappings[i]];
                }
            }

            public static void Initialize(string paramQuery, string connString, Dictionary<string, string> paramMappings)
            {
                SqlDataAdapter a = new SqlDataAdapter(paramQuery, connString);
                theParams = new DataTable();
                a.Fill(theParams);

                numRows = theParams.Rows.Count;

                outputParams = new SqlParameter[paramMappings.Keys.Count];
                paramDTMappings = new int[paramMappings.Keys.Count];

                //Populate the array of parameters that will be cloned and filled
                //on each request
                int i = 0;
                foreach (string parameterName in paramMappings.Keys)
                {
                    outputParams[i] = new SqlParameter();
                    outputParams[i].ParameterName = parameterName;
                    string paramColumn = paramMappings[parameterName];
    
                    //if there is a param mapped to this column
                    if (paramColumn != null)
                        paramDTMappings[i] = theParams.Columns[paramColumn].Ordinal;

                    i++;
                }
            }
        }

        public class queryOutput
        {
            public Exception e;
            public TimeSpan time;
            public bool finished;
            public int LogicalReads;
            public int CPUTime;
            public int ElapsedTime;

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

        private class ThreadKiller
        {
            private Thread[] theThreads;
            private SqlCommand[] theCommands;

            public ThreadKiller(
                Thread[] TheThreads,
                SqlCommand[] TheCommands)
            {
                this.theThreads = TheThreads;
                this.theCommands = TheCommands;
            }

            public void KillEm()
            {
                foreach (SqlCommand comm in theCommands)
                {
                    comm.Cancel();
                    comm.Connection.Dispose();
                    comm.Connection = null;
                    comm.Dispose();
                    Thread.Sleep(0);
                }
                
                bool keepKilling = true;

                while (keepKilling)
                {
                    keepKilling = false;

                    foreach (Thread theThread in theThreads)
                    {
                        if (theThread.IsAlive)
                        {
                            keepKilling = true;
                            theThread.Abort();
                            Thread.Sleep(0);
                        }
                    }
                }
            }
        }
    }
}
