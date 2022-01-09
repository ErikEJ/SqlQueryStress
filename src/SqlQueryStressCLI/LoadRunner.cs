using Spectre.Console;
using SQLQueryStress;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;

namespace SqlQueryStressCLI
{
    public class LoadRunner
    {
        private System.ComponentModel.BackgroundWorker backgroundWorker1 = new System.ComponentModel.BackgroundWorker();

        //Exceptions that occurred
        private Dictionary<string, int> _exceptions;

        private readonly QueryStressSettings _settings;

        //start of the load
        private TimeSpan _start;
        //total CPU time in milliseconds
        private double _totalCpuTime;

        //total elapsed time in milliseconds
        private double _totalElapsedTime;

        //total exceptions
        private int _totalExceptions;

        //threads * iterations
        private int _totalExpectedIterations;
        /* Runtime locals */

        //total iterations that have run
        private int _totalIterations;

        //number of active threads running
        private int _activeThreads;

        //Same comments as above for these two...
        private double _totalLogicalReads;
        private int _totalReadMessages;

        //This is the total time as reported by the client
        private double _totalTime;

        //Number of query requests that returned time messages
        //Note:: Average times will be computed by:
        // A) Add up all results from time messages returned by 
        //    each query output
        // B) If the query returned one or more time messages,
        //    increment the totalTimeMessages counter
        // C) Add the TOTAL of all messages to the total counters
        // D) Divide to find the actual time
        //TODO: Find out why elapsed time is not accurate in 
        //some cases.  For instance, look at time reported for
        //WAITFOR DELAY '00:00:05'  (1300 ms?? WTF??)
        private int _totalTimeMessages;
        private int _numThreads;

        private DateTime _testStartTime;

        private string theTime;
        private string avgSeconds;
        private string cpuTime;
        private string actualSeconds;
        private string logicalReads;

        private Guid _testGuid;

        private System.Threading.CancellationTokenSource _backgroundWorkerCTS;

        private System.Timers.Timer timer = new System.Timers.Timer(100);

        private readonly CommandLineOptions _runParameters;

        public LoadRunner(QueryStressSettings settings, CommandLineOptions options)
        {
            _settings = settings;
            _runParameters = options;
        }

        public void Run()
        {
            if (!_settings.MainDbConnectionInfo.TestConnection())
            {
                Console.Error.WriteLine("Invalid connection info");
                return;
            }

            _testStartTime = DateTime.Now;
            _testGuid = Guid.NewGuid();
            _backgroundWorkerCTS = new System.Threading.CancellationTokenSource();

            _exceptions = new Dictionary<string, int>();

            _totalIterations = 0;
            _activeThreads = 0;
            _totalTime = 0;
            _totalCpuTime = 0;
            _totalElapsedTime = 0;
            _totalTimeMessages = 0;
            _totalLogicalReads = 0;
            _totalReadMessages = 0;
            _totalExceptions = 0;

            _totalExpectedIterations = _settings.NumThreads * _settings.NumIterations;

            var paramConnectionInfo = _settings.ShareDbSettings ? _settings.MainDbConnectionInfo : _settings.ParamDbConnectionInfo;

            _numThreads = _runParameters.NumberOfThreads ?? _settings.NumThreads;

            var query = _settings.MainQuery;

            if (_runParameters.Input != null)
            {
                query = File.ReadAllText(_runParameters.Input.FullName, Encoding.UTF8);
            }

            var engine = new LoadEngine(_settings.MainDbConnectionInfo.ConnectionString, query, _numThreads, _settings.NumIterations,
                _settings.ParamQuery, _settings.ParamMappings, paramConnectionInfo.ConnectionString, _settings.CommandTimeout, _settings.CollectIoStats,
                _settings.CollectTimeStats, _settings.ForceDataRetrieval, _settings.KillQueriesOnCancel, _backgroundWorkerCTS);

            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.DoWork += new DoWorkEventHandler(this.backgroundWorker1_DoWork);
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);

            backgroundWorker1.RunWorkerAsync(engine);

            //timer.Elapsed += Timer_Elapsed;
            //timer.AutoReset = true;
            //timer.Enabled = true;

            _start = new TimeSpan(DateTime.Now.Ticks);

            while (backgroundWorker1.IsBusy)
            {
                Thread.Sleep(1000);
            }

            Thread.Sleep(2000);
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            UpdateUi();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            ((LoadEngine)e.Argument).StartLoad(backgroundWorker1, _settings.DelayBetweenQueries);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var output = (LoadEngine.QueryOutput)e.UserState;

            _totalIterations++;

            if (output.LogicalReads > 0)
            {
                _totalReadMessages++;
                _totalLogicalReads += output.LogicalReads;
            }

            if (output.ElapsedTime > 0)
            {
                _totalTimeMessages++;
                _totalCpuTime += output.CpuTime;
                _totalElapsedTime += output.ElapsedTime;
            }

            _totalTime += output.Time.TotalMilliseconds;

            if (output.E != null)
            {
                _totalExceptions++;
                string theMessage;

                //strip the time stats, if they showed up as part
                //of the exception
                if (_settings.CollectTimeStats)
                {
                    var matchPos = output.E.Message.IndexOf("SQL Server parse and compile time:", StringComparison.Ordinal);

                    theMessage = matchPos > -1 ? output.E.Message.Substring(0, matchPos - 2) : output.E.Message;
                }
                else
                {
                    theMessage = output.E.Message;
                }

                if (!_exceptions.ContainsKey(theMessage))
                {
                    _exceptions.Add(theMessage, 1);
                }
                else
                {
                    _exceptions[theMessage] += 1;
                }

                //TODO: Get this working? -- Repaint exceptions as they occur?
                /*
                if ((exceptionViewer != null) && (exceptionViewer.WindowState == FormWindowState.Normal))
                {
                    exceptionViewer.Invoke(new System.Threading.ThreadStart(exceptionViewer.Repaint));
                }
                 */
            }

            _activeThreads = output.ActiveThreads;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            UpdateUi();

            ((BackgroundWorker)sender).Dispose();
            _backgroundWorkerCTS?.Dispose();

            if (!string.IsNullOrEmpty(_runParameters.ResultsAutoSaveFileName))
            {
                AutoSaveResults(_runParameters.ResultsAutoSaveFileName);
            }
        }

        private void UpdateUi()
        {
            var avgIterations = _totalIterations == 0 ? 0.0 : _totalTime / _totalIterations / 1000;
            var avgCpu = _totalTimeMessages == 0 ? 0.0 : _totalCpuTime / _totalTimeMessages / 1000;
            var avgActual = _totalTimeMessages == 0 ? 0.0 : _totalElapsedTime / _totalTimeMessages / 1000;
            var avgReads = _totalReadMessages == 0 ? 0.0 : _totalLogicalReads / _totalReadMessages;

            avgSeconds = avgIterations.ToString("0.0000", CultureInfo.InvariantCulture);
            cpuTime = _totalTimeMessages == 0 ? "---" : avgCpu.ToString("0.0000", CultureInfo.InvariantCulture);
            actualSeconds = _totalTimeMessages == 0 ? "---" : avgActual.ToString("0.0000", CultureInfo.InvariantCulture);
            logicalReads = _totalReadMessages == 0 ? "---" : avgReads.ToString("0.0000", CultureInfo.InvariantCulture);

            var end = new TimeSpan(DateTime.Now.Ticks);
            end = end.Subtract(_start);
            theTime = end.ToString();

            var color = "[lime]";

            var table = new Table();
            table.Border(TableBorder.Square);
            table.Collapse();

            table.AddColumn("sqlstresscmd result");
            table.AddColumn(new TableColumn("Value").Centered());

            table.Columns[1].RightAligned();

            _ = table.AddRow("Start time", $"{color}{_testStartTime:u}[/]");
            table.AddRow("Elapsed time", $"{color}{theTime}[/]");
            table.AddRow("Number of Threads", $"{color}{_numThreads}[/]");
            table.AddRow("Iterations Completed", $"{color}{_totalIterations}[/]");
            table.AddRow("CPU Seconds/Iteration (Avg)", $"{color}{cpuTime}[/]");
            table.AddRow("Actual Seconds/Iteration (Avg)", $"{color}{actualSeconds}[/]");
            table.AddRow("Client Seconds/Iteration (Avg)", $"{color}{avgSeconds}[/]");
            table.AddRow("Logical Reads/Iteration (Avg)", $"{color}{logicalReads}[/]");

            AnsiConsole.Write(table);
        }

        private void AutoSaveResults(string resultsAutoSaveFileName)
        {
            string extension = Path.GetExtension(resultsAutoSaveFileName).ToUpperInvariant();
            if (extension.Equals(".csv", StringComparison.InvariantCultureIgnoreCase))
            {
                ExportBenchMarkToCsvFile(resultsAutoSaveFileName);
            }
        }

        private void ExportBenchMarkToCsvFile(string fileName)
        {
            try
            {
                var fileExists = File.Exists(fileName);
                using var textWriter = new StreamWriter(fileName, true);
                // we do not write the header line if we are appending to an existing file 
                if (fileExists == false)
                {
                    WriteBenchmarkCsvHeader(textWriter);
                }
                WriteBenchmarkCsvText(textWriter);
            }
            catch (Exception)
            {
                Console.Error.WriteLine("Error While Saving BenchMark",
                    $"There was an error saving the benchmark to '{fileName}', make sure you have write privileges to that path");
            }
        }

        private static void WriteBenchmarkCsvHeader(StreamWriter tw)
        {
            tw.WriteLine("TestId,TestStartTime,ElapsedTime,Iterations,Threads,Delay,CompletedIterations,AvgCPUSeconds,AvgActualSeconds,AvgClientSeconds,AvgLogicalReads");
        }

        private void WriteBenchmarkCsvText(TextWriter tw)
        {
            tw.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}",
                _testGuid,
                _testStartTime,
                theTime,
                _totalIterations,
                _runParameters.NumberOfThreads,
                _settings.DelayBetweenQueries,
                _totalIterations,
                cpuTime,
                actualSeconds,
                avgSeconds,
                logicalReads
                );
        }
    }
}
