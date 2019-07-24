#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using SQLQueryStress.Properties;

#endregion

/*********************************************
TODO, version 1.0::::
 * figure out how to capture change of selection in parameter definer
 * 
 * Throw a message box if param database has not been selected, or if can't connect (if parameterization is on)
 * repaint exception window when datatable is updated
  * Bug w/ stats sometimes going blank ??
 *********************************************/

namespace SQLQueryStress
{
    public partial class Form1 : Form
    {
        private const string Dashes = "---";

        //Has this run been cancelled?
        private bool _cancelled;
        //Exceptions that occurred
        private Dictionary<string, int> _exceptions;

        //The exception viewer window
        private DataViewer _exceptionViewer;
        //Exit as soon as cancellation is finished?
        private bool _exitOnComplete;

        /* Configuration local */
        private QueryStressSettings _settings = new QueryStressSettings();

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

        private DateTime _testStartTime;

        private Guid _testGuid;

        private CommandLineOptions _runParameters; 

        public Form1(CommandLineOptions runParameters) : this()
        {
            _runParameters = runParameters;

            if (string.IsNullOrWhiteSpace(_runParameters.SettingsFile) == false)
            {
                var isConfigFileExists = File.Exists(_runParameters.SettingsFile); 
                if (isConfigFileExists)
                {
                    OpenConfigFile(_runParameters.SettingsFile);
                    if (_runParameters.Unattended)
                    {
                        Load += StartProcessing;
                    }
                }
                else
                {
                    throw new ArgumentException(string.Format("Settings file could not be found: {0}", _runParameters.SettingsFile));
                }
            }

            // are we overriding the config file?
            if (_runParameters.NumberOfThreads > 0)
            {
                threads_numericUpDown.Value = _settings.NumThreads = _runParameters.NumberOfThreads;
            }

            if (string.IsNullOrWhiteSpace(_runParameters.DbServer) == false)
            {
                _settings.MainDbConnectionInfo.Server = _runParameters.DbServer; 
            }
        }

        public Form1()
        {
            InitializeComponent();

            saveSettingsFileDialog.DefaultExt = "json";
            saveSettingsFileDialog.Filter = @"SQLQueryStress Configuration Files|*.json";
            saveSettingsFileDialog.FileOk += saveSettingsFileDialog_FileOk;

            loadSettingsFileDialog.DefaultExt = "json";
            loadSettingsFileDialog.Filter = @"SQLQueryStress Configuration Files|*.json";
            loadSettingsFileDialog.FileOk += loadSettingsFileDialog_FileOk;
        }

        private void StartProcessing(Object sender, EventArgs e)
        {
            go_button.PerformClick();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var a = new AboutBox();
            a.ShowDialog();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            int tmp;
            ((LoadEngine) e.Argument).StartLoad(backgroundWorker1, (Int32.TryParse(queryDelay_textBox.Text, out tmp) ? tmp : 0));
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var output = (LoadEngine.QueryOutput) e.UserState;

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
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            mainUITimer.Stop();

            UpdateUi();

            go_button.Enabled = true;
            cancel_button.Enabled = false;
            threads_numericUpDown.Enabled = true;
            iterations_numericUpDown.Enabled = true;
            queryDelay_textBox.Enabled = true;

            if (!_cancelled)
                progressBar1.Value = 100;

            ((BackgroundWorker) sender).Dispose();

            db_label.Text = "";

            if (string.IsNullOrEmpty(_runParameters.ResultsAutoSaveFileName) == false)
            {
                AutoSaveResults(_runParameters.ResultsAutoSaveFileName); 
            }

            // if we started automatically exit when done
            if (_exitOnComplete || _runParameters.Unattended)
            {
                Dispose();
            }
        }

        private void AutoSaveResults(string resultsAutoSaveFileName)
        {
            string extension = Path.GetExtension(resultsAutoSaveFileName).ToLower();
            if (extension == ".csv")
            {
                ExportBenchMarkToCsvFile(resultsAutoSaveFileName);
            }
            else
            {
                ExportBenchMarkToTextFile(resultsAutoSaveFileName);
            }
        }

        private void cancel_button_Click(object sender, EventArgs e)
        {
            cancel_button.Enabled = false;

            backgroundWorker1.CancelAsync();

            _cancelled = true;

            if (sender is string)
            {
                _exitOnComplete = true;
            }
        }

        private void database_button_Click(object sender, EventArgs e)
        {
            var dbselect = new DatabaseSelect(_settings) {StartPosition = FormStartPosition.CenterParent};
            dbselect.ShowDialog();
        }

        private void exceptions_button_Click(object sender, EventArgs e)
        {
            totalExceptions_textBox_Click(null, null);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void go_button_Click(object sender, EventArgs e)
        {
            if (!_settings.MainDbConnectionInfo.TestConnection())
            {
                MessageBox.Show(Resources.MustSetValidDbConnInfo);
                return;
            }

            _testStartTime = DateTime.Now;
            _testGuid = Guid.NewGuid();
            _cancelled = false;
            _exitOnComplete = false;

            _exceptions = new Dictionary<string, int>();

            _totalIterations = 0;
            _totalTime = 0;
            _totalCpuTime = 0;
            _totalElapsedTime = 0;
            _totalTimeMessages = 0;
            _totalLogicalReads = 0;
            _totalReadMessages = 0;
            _totalExceptions = 0;

            iterationsSecond_textBox.Text = @"0";
            avgSeconds_textBox.Text = @"0.0";
            actualSeconds_textBox.Text = Dashes;
            cpuTime_textBox.Text = Dashes;
            logicalReads_textBox.Text = Dashes;
            go_button.Enabled = false;
            cancel_button.Enabled = true;
            iterations_numericUpDown.Enabled = false;
            threads_numericUpDown.Enabled = false;
            queryDelay_textBox.Enabled = false;

            progressBar1.Value = 0;

            SaveSettingsFromForm1();

            _totalExpectedIterations = _settings.NumThreads * _settings.NumIterations;

            var paramConnectionInfo = _settings.ShareDbSettings ? _settings.MainDbConnectionInfo : _settings.ParamDbConnectionInfo;
            db_label.Text = "" + @"Server: " + paramConnectionInfo.Server +
                            (paramConnectionInfo.Database.Length > 0 ? "  //  Database: " + paramConnectionInfo.Database : "");

            var engine = new LoadEngine(_settings.MainDbConnectionInfo.ConnectionString, _settings.MainQuery, _settings.NumThreads, _settings.NumIterations,
                _settings.ParamQuery, _settings.ParamMappings, paramConnectionInfo.ConnectionString, _settings.CommandTimeout, _settings.CollectIoStats,
                _settings.CollectTimeStats, _settings.ForceDataRetrieval, _settings.KillQueriesOnCancel);

            backgroundWorker1.RunWorkerAsync(engine);

            _start = new TimeSpan(DateTime.Now.Ticks);

            mainUITimer.Start();
        }

        private void loadSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadSettingsFileDialog.ShowDialog();
        }

        private void mainUITimer_Tick(object sender, EventArgs e)
        {
            UpdateUi();
        }

        private void OpenConfigFile(string fileName)
        {
            try
            {
                var contents = File.ReadAllText(fileName);
                _settings = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryStressSettings>(contents);
            }
            catch (Exception exc)
            {
                MessageBox.Show(string.Format("{0}: {1}", Resources.ErrLoadingSettings, exc.Message));
            }

            var sqlControl = elementHost1.Child as SqlControl;
            if (sqlControl != null)
            {
                sqlControl.Text = _settings.MainQuery;
            }
            threads_numericUpDown.Value = _settings.NumThreads;
            iterations_numericUpDown.Value = _settings.NumIterations;
            queryDelay_textBox.Text = _settings.DelayBetweenQueries.ToString();
        }

        private void loadSettingsFileDialog_FileOk(object sender, EventArgs e)
        {
            OpenConfigFile(loadSettingsFileDialog.FileName);
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var options = new Options(_settings);
            options.ShowDialog();
        }

        private void param_button_Click(object sender, EventArgs e)
        {
            var sqlControl = elementHost1.Child as SqlControl;
            if (sqlControl != null)
            {
                var p = new ParamWindow(_settings, sqlControl.Text) {StartPosition = FormStartPosition.CenterParent};
                p.ShowDialog();
            }
        }

        private void saveSettingsFileDialog_FileOk(object sender, EventArgs e)
        {
            try
            {
                var jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(_settings);
                File.WriteAllText(saveSettingsFileDialog.FileName, jsonContent);
            }
            catch (Exception exc)
            {
                MessageBox.Show(string.Format("{0}: {1}", Resources.ErrorSavingSettings, exc.Message)); 
            }
        }

        private void SaveSettingsFromForm1()
        {
            var sqlControl = elementHost1.Child as SqlControl;
            if (sqlControl != null) _settings.MainQuery =  sqlControl.Text;
            _settings.NumThreads = (int) threads_numericUpDown.Value;
            _settings.NumIterations = (int) iterations_numericUpDown.Value;
            _settings.DelayBetweenQueries = int.Parse(queryDelay_textBox.Text);
        }

        private void saveSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveSettingsFromForm1();
            saveSettingsFileDialog.ShowDialog();
        }

        private void totalExceptions_textBox_Click(object sender, EventArgs e)
        {
            _exceptionViewer = new DataViewer {StartPosition = FormStartPosition.CenterParent, Text = Resources.Exceptions};

            var dt = new DataTable();
            dt.Columns.Add("Count");
            dt.Columns.Add("Exception");

            if (_exceptions != null)
            {
                var values = _exceptions.Values.GetEnumerator();

                foreach (var ex in _exceptions.Keys)
                {
                    values.MoveNext();
                    var count = values.Current;
                    dt.Rows.Add(count, ex);
                }
            }

            _exceptionViewer.DataView = dt;

            _exceptionViewer.ShowDialog();
        }

        private void UpdateUi()
        {
            iterationsSecond_textBox.Text = _totalIterations.ToString();
            var avgIterations = _totalIterations == 0 ? 0.0 : _totalTime / _totalIterations / 1000;
            var avgCpu = _totalTimeMessages == 0 ? 0.0 : _totalCpuTime / _totalTimeMessages / 1000;
            var avgActual = _totalTimeMessages == 0 ? 0.0 : _totalElapsedTime / _totalTimeMessages / 1000;
            var avgReads = _totalReadMessages == 0 ? 0.0 : _totalLogicalReads / _totalReadMessages;

            avgSeconds_textBox.Text = avgIterations.ToString("0.0000");
            cpuTime_textBox.Text = _totalTimeMessages == 0 ? "---" : avgCpu.ToString("0.0000");
            actualSeconds_textBox.Text = _totalTimeMessages == 0 ? "---" : avgActual.ToString("0.0000");
            logicalReads_textBox.Text = _totalReadMessages == 0 ? "---" : avgReads.ToString("0.0000");

            totalExceptions_textBox.Text = _totalExceptions.ToString();
            progressBar1.Value = Math.Min((int) (_totalIterations / (decimal) _totalExpectedIterations * 100), 100);

            var end = new TimeSpan(DateTime.Now.Ticks);
            end = end.Subtract(_start);

            var theTime = end.ToString();

            //Some systems return "hh:mm:ss" instead of "hh:mm:ss.0000" if
            //there is no fractional part of the second.  I'm not sure
            //why, but this fixes it for now.
            if (theTime.Length > 8)
                elapsedTime_textBox.Text = theTime.Substring(0, 13);
            else
                elapsedTime_textBox.Text = theTime + @".0000";
        }

        private void btnCleanBuffer_Click(object sender, EventArgs e)
        {
            MessageBox.Show(LoadEngine.ExecuteCommand(_settings.MainDbConnectionInfo.ConnectionString, "DBCC DROPCLEANBUFFERS")
                ? "Buffers cleared"
                : "Errors encountered");
        }

        private void btnFreeCache_Click(object sender, EventArgs e)
        {
            MessageBox.Show(LoadEngine.ExecuteCommand(_settings.MainDbConnectionInfo.ConnectionString, "DBCC FREEPROCCACHE")
                ? "Cache freed"
                : "Errors encountered");
        }


        private void toTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.AddExtension = true;
            saveFileDialog.OverwritePrompt = false;
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt";
            saveFileDialog.ShowDialog();

            if (!string.IsNullOrEmpty(saveFileDialog.FileName))
                ExportBenchMarkToTextFile(saveFileDialog.FileName);
        }

        private void ExportBenchMarkToTextFile(string fileName)
        {
            try
            {
                var textWriter = new StreamWriter(fileName, true);
                WriteBenchmarkTextContent(textWriter);
                textWriter.Close();
            }
            catch
            {
                MessageBox
                    .Show("Error While Saving BenchMark",
                    string.Format("There was an error saving the benchmark to '{0}', make sure you have write privileges to that path",fileName));
            }
        }

        private void WriteBenchmarkTextContent(TextWriter tw)
        {
            tw.WriteLine(string.Format("Test ID: {0}",
                                _testGuid));
            tw.WriteLine(string.Format("Test TimeStamp: {0}",
                                _testStartTime));
            tw.WriteLine(string.Format("Elapsed Time: {0}",
                elapsedTime_textBox.Text));
            tw.WriteLine(string.Format("Number of Iterations: {0}",
                (int)iterations_numericUpDown.Value));
            tw.WriteLine(string.Format("Number of Threads: {0}",
                (int)threads_numericUpDown.Value));
            tw.WriteLine(string.Format("Delay Between Queries (ms): {0}",
                int.Parse(queryDelay_textBox.Text)));
            tw.WriteLine(string.Format("CPU Seconds/Iteration (Avg): {0}",
                cpuTime_textBox.Text));
            tw.WriteLine(string.Format("Actual Seconds/Iteration (Avg): {0}",
                actualSeconds_textBox.Text));
            tw.WriteLine(string.Format("Iterations Completed: {0}",
                iterationsSecond_textBox.Text));
            tw.WriteLine(string.Format("Client Seconds/Iteration (Avg): {0}",
                avgSeconds_textBox.Text));
            tw.WriteLine(string.Format("Logical Reads/Iteration (Avg): {0}",
                logicalReads_textBox.Text));
            tw.WriteLine("");
        }


        private void toClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var textWriter = new StringWriter();
                WriteBenchmarkTextContent(textWriter);
                Clipboard.SetText(textWriter.ToString());
            }
            catch
            {
                MessageBox
                    .Show("Error While Copying BenchMark to Clipboard",
                    string.Format("There was an error copying the benchmark to clipboard"));
            }
        }

        private void toCsvToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.AddExtension = true;
            saveFileDialog.OverwritePrompt = false;
            saveFileDialog.Filter = "Csv Files (*.csv)|*.csv";
            saveFileDialog.ShowDialog();

            if (!string.IsNullOrEmpty(saveFileDialog.FileName))
                ExportBenchMarkToCsvFile(saveFileDialog.FileName);
        }

        private void ExportBenchMarkToCsvFile(string fileName)
        {
            try
            {
                var fileExists = File.Exists(fileName);
                var textWriter = new StreamWriter(fileName, true);
                // we do not write the header line if we are appending to an existing file 
                if (fileExists == false)
                {
                    WriteBenchmarkCsvHeader(textWriter);
                }
                WriteBenchmarkCsvText(textWriter);
                textWriter.Close();
            }
            catch
            {
                MessageBox
                    .Show("Error While Saving BenchMark",
                    string.Format("There was an error saving the benchmark to '{0}', make sure you have write privileges to that path", fileName));
            }
        }

        private void WriteBenchmarkCsvHeader(StreamWriter tw)
        {
            tw.WriteLine("TestId,TestStartTime,ElapsedTime,Iterations,Threads,Delay,CompletedIterations,AvgCPUSeconds,AvgActualSeconds,AvgClientSeconds,AvgLogicalReads");
        }

        private void WriteBenchmarkCsvText(TextWriter tw)
        {
            tw.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}",
                _testGuid,
                _testStartTime,
                elapsedTime_textBox.Text,
                (int)iterations_numericUpDown.Value,
                (int)threads_numericUpDown.Value,
                int.Parse(queryDelay_textBox.Text),
                iterationsSecond_textBox.Text,
                cpuTime_textBox.Text,
                actualSeconds_textBox.Text,
                avgSeconds_textBox.Text,
                logicalReads_textBox.Text
                );
        }


    }
}