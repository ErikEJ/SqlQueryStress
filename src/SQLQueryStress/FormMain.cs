#region

using SQLQueryStress.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

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
#pragma warning disable CA1031 // Do not catch general exception types
    public partial class FormMain : Form
    {
        private const string Dashes = "---";

        //Has this run been cancelled?
        private bool _cancelled;
        //Exceptions that occurred
        private Dictionary<string, int> _exceptions;

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

        private DateTime _testStartTime;

        private Guid _testGuid;

        private readonly CommandLineOptions _runParameters;

        private System.Threading.CancellationTokenSource _backgroundWorkerCTS;

        private SqlControl sqlControl1;

        public FormMain(CommandLineOptions runParameters) : this()
        {
            _runParameters = runParameters;

            if (string.IsNullOrWhiteSpace(_runParameters.SettingsFile) == false)
            {
                if (File.Exists(_runParameters.SettingsFile))
                {
                    OpenConfigFile(_runParameters.SettingsFile);
                    if (_runParameters.Unattended)
                    {
                        Load += StartProcessing;
                    }
                }
                else
                {
                    throw new ArgumentException($"Settings file could not be found: {_runParameters.SettingsFile}");
                }
            }

            // are we overriding the config file?
            if (_runParameters.NumberOfThreads > 0)
            {
                threads_numericUpDown.Value = _settings.NumThreads = _runParameters.NumberOfThreads;
            }

            if (!string.IsNullOrWhiteSpace(_runParameters.DbServer))
            {
                _settings.MainDbConnectionInfo.Server = _runParameters.DbServer;
            }
        }

        public FormMain()
        {
            InitializeComponent();

            saveSettingsFileDialog.DefaultExt = "json";
            saveSettingsFileDialog.Filter = Resources.ConfigFiles;
            saveSettingsFileDialog.FileOk += saveSettingsFileDialog_FileOk;

            loadSettingsFileDialog.DefaultExt = "json";
            loadSettingsFileDialog.Filter = Resources.ConfigFiles;
            loadSettingsFileDialog.FileOk += loadSettingsFileDialog_FileOk;
        }

        private void StartProcessing(object sender, EventArgs e)
        {
            go_button.PerformClick();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            ((LoadEngine)e.Argument).StartLoad(backgroundWorker1, (int.TryParse(queryDelay_numericUpDown.Text, out int tmp) ? tmp : 0));
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

                if (!_exceptions.TryAdd(theMessage, 1))
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
            mainUITimer.Stop();

            UpdateUi();

            go_button.Enabled = true;
            cancel_button.Enabled = false;
            threads_numericUpDown.Enabled = true;
            iterations_numericUpDown.Enabled = true;
            queryDelay_numericUpDown.Enabled = true;

            if (!_cancelled)
            {
                progressBar1.Value = 100;
            }
            else
            {
                progressBar1.Value = progressBar1.Minimum;
            }

            ((BackgroundWorker)sender).Dispose();
            _backgroundWorkerCTS?.Dispose();

            db_label.Text = string.Empty;
#pragma warning disable CA1303 // Do not pass literals as localized parameters
            activeThreads_textBox.Text = "0";
#pragma warning restore CA1303 // Do not pass literals as localized parameters

            if (!string.IsNullOrEmpty(_runParameters.ResultsAutoSaveFileName))
            {
                AutoSaveResults(_runParameters.ResultsAutoSaveFileName);
            }

            // if we started automatically exit when done
            if (_exitOnComplete || _runParameters.Unattended)
            {
                Dispose();
            }

            progressBar1.Value = progressBar1.Minimum;
        }

        private void AutoSaveResults(string resultsAutoSaveFileName)
        {
            string extension = Path.GetExtension(resultsAutoSaveFileName).ToUpperInvariant();
            if (extension.Equals(".csv", StringComparison.OrdinalIgnoreCase))
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

            _backgroundWorkerCTS?.Cancel();
            _backgroundWorkerCTS?.Dispose();

            backgroundWorker1.CancelAsync();

            _cancelled = true;

            if (sender is string)
            {
                _exitOnComplete = true;
            }
        }

        private void database_button_Click(object sender, EventArgs e)
        {
            using var dbselect = new DatabaseSelect(_settings) { StartPosition = FormStartPosition.CenterParent };
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "<Pending>")]
        private void go_button_Click(object sender, EventArgs e)
        {
            if (!_settings.MainDbConnectionInfo.TestConnection())
            {
                MessageBox.Show(Resources.MustSetValidDbConnInfo, Resources.AppTitle);
                return;
            }

            _testStartTime = DateTime.Now;
            _testGuid = Guid.NewGuid();
            _cancelled = false;
            _exitOnComplete = false;
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

            iterationsSecond_textBox.Text = @"0";
            avgSeconds_textBox.Text = @"0.0";
            actualSeconds_textBox.Text = Dashes;
            cpuTime_textBox.Text = Dashes;
            logicalReads_textBox.Text = Dashes;
            go_button.Enabled = false;
            cancel_button.Enabled = true;
            iterations_numericUpDown.Enabled = false;
            threads_numericUpDown.Enabled = false;
            queryDelay_numericUpDown.Enabled = false;

            progressBar1.Value = 0;

            SaveSettingsFromFormMain();

            _totalExpectedIterations = _settings.NumThreads * _settings.NumIterations;

            // override main query string with selected text when selected text exists
            if (sqlControl1.SelectedText.Length != 0)
            {
                _settings.MainQuery = sqlControl1.SelectedText;
            }

            var paramConnectionInfo = _settings.ShareDbSettings ? _settings.MainDbConnectionInfo : _settings.ParamDbConnectionInfo;
            db_label.Text = $@"Server: {paramConnectionInfo.Server}{(paramConnectionInfo.Database.Length > 0 ? "  //  Database: " + paramConnectionInfo.Database : string.Empty)}";

            var engine = new LoadEngine(_settings.MainDbConnectionInfo.ConnectionString, _settings.MainQuery, _settings.NumThreads, _settings.NumIterations,
                _settings.ParamQuery, _settings.ParamMappings, paramConnectionInfo.ConnectionString, _settings.CommandTimeout, _settings.CollectIoStats,
                _settings.CollectTimeStats, _settings.ForceDataRetrieval, _settings.KillQueriesOnCancel, _backgroundWorkerCTS);

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
                _settings = JsonSerializer.ReadToObject<QueryStressSettings>(contents);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{Resources.ErrLoadingSettings}: {ex.Message}", Resources.AppTitle);
            }

            sqlControl1.Text = _settings.MainQuery;
            
            threads_numericUpDown.Value = _settings.NumThreads;
            iterations_numericUpDown.Value = _settings.NumIterations;
            queryDelay_numericUpDown.Text = _settings.DelayBetweenQueries.ToString(CultureInfo.InvariantCulture);
        }

        private void loadSettingsFileDialog_FileOk(object sender, EventArgs e)
        {
            OpenConfigFile(loadSettingsFileDialog.FileName);
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var options = new Options(_settings);
            options.ShowDialog();
        }

        private void param_button_Click(object sender, EventArgs e)
        {
            using var paramWindow = new ParamWindow(_settings, sqlControl1.Text) { StartPosition = FormStartPosition.CenterParent };
            paramWindow.ShowDialog();
        }

        private void saveSettingsFileDialog_FileOk(object sender, EventArgs e)
        {
            try
            {
                var jsonContent = JsonSerializer.WriteFromObject(_settings);
                File.WriteAllText(saveSettingsFileDialog.FileName, jsonContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{Resources.ErrorSavingSettings}: {ex.Message}", Resources.AppTitle);
            }
        }

        private void SaveSettingsFromFormMain()
        {
            _settings.MainQuery = sqlControl1.Text;
            _settings.NumThreads = (int)threads_numericUpDown.Value;
            _settings.NumIterations = (int)iterations_numericUpDown.Value;
            _settings.DelayBetweenQueries = (int)queryDelay_numericUpDown.Value;
        }

        private void saveSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveSettingsFromFormMain();
            saveSettingsFileDialog.ShowDialog();
        }

        private void totalExceptions_textBox_Click(object sender, EventArgs e)
        {
            using var exceptionViewer = new DataViewer { StartPosition = FormStartPosition.CenterParent, Text = Resources.Exceptions };

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

            exceptionViewer.DataView = dt;
            exceptionViewer.ShowDialog();
        }

        private void UpdateUi()
        {
            iterationsSecond_textBox.Text = _totalIterations.ToString(CultureInfo.CurrentCulture);
            activeThreads_textBox.Text = _activeThreads.ToString(CultureInfo.CurrentCulture);
            var avgIterations = _totalIterations == 0 ? 0.0 : _totalTime / _totalIterations / 1000;
            var avgCpu = _totalTimeMessages == 0 ? 0.0 : _totalCpuTime / _totalTimeMessages / 1000;
            var avgActual = _totalTimeMessages == 0 ? 0.0 : _totalElapsedTime / _totalTimeMessages / 1000;
            var avgReads = _totalReadMessages == 0 ? 0.0 : _totalLogicalReads / _totalReadMessages;

            avgSeconds_textBox.Text = avgIterations.ToString("0.0000", CultureInfo.CurrentCulture);
            cpuTime_textBox.Text = _totalTimeMessages == 0 ? "---" : avgCpu.ToString("0.0000", CultureInfo.CurrentCulture);
            actualSeconds_textBox.Text = _totalTimeMessages == 0 ? "---" : avgActual.ToString("0.0000", CultureInfo.CurrentCulture);
            logicalReads_textBox.Text = _totalReadMessages == 0 ? "---" : avgReads.ToString("0.0000", CultureInfo.CurrentCulture);

            totalExceptions_textBox.Text = _totalExceptions.ToString(CultureInfo.CurrentCulture);
            progressBar1.Value = Math.Min((int)(_totalIterations / (decimal)_totalExpectedIterations * 100), 100);

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
            if (!_settings.MainDbConnectionInfo.TestConnection())
            {
                MessageBox.Show(Resources.MustSetValidDbConnInfo, Resources.AppTitle);
                return;
            }

            LoadEngine.ExecuteCommand(_settings.MainDbConnectionInfo.ConnectionString, "CHECKPOINT");

            MessageBox.Show(LoadEngine.ExecuteCommand(_settings.MainDbConnectionInfo.ConnectionString, "DBCC DROPCLEANBUFFERS")
                ? "Buffers cleared"
                : "Errors encountered",
                Resources.AppTitle);
        }

        private void btnFreeCache_Click(object sender, EventArgs e)
        {
            if (!_settings.MainDbConnectionInfo.TestConnection())
            {
                MessageBox.Show(Resources.MustSetValidDbConnInfo, Resources.AppTitle);
                return;
            }

            MessageBox.Show(LoadEngine.ExecuteCommand(_settings.MainDbConnectionInfo.ConnectionString, "DBCC FREEPROCCACHE")
                ? "Cache freed"
                : "Errors encountered",
                Resources.AppTitle);
        }


        private void toTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var saveFileDialog = new SaveFileDialog
            {
                AddExtension = true,
                OverwritePrompt = false,
                Filter = Resources.TextFiles
            };
            saveFileDialog.ShowDialog();

            if (!string.IsNullOrEmpty(saveFileDialog.FileName))
                ExportBenchMarkToTextFile(saveFileDialog.FileName);
        }

        private void ExportBenchMarkToTextFile(string fileName)
        {
            try
            {
                using var textWriter = new StreamWriter(fileName, true);
                WriteBenchmarkTextContent(textWriter);
            }
            catch (Exception)
            {
                MessageBox
                    .Show($"Error While Saving BenchMark: There was an error saving the benchmark to '{fileName}', make sure you have write privileges to that path", Resources.AppTitle);
            }
        }

        private void WriteBenchmarkTextContent(TextWriter tw)
        {
            tw.WriteLine($"Test ID: {_testGuid}");
            tw.WriteLine($"Test TimeStamp: {_testStartTime}");
            tw.WriteLine($"Elapsed Time: {elapsedTime_textBox.Text}");
            tw.WriteLine($"Number of Iterations: {(int)iterations_numericUpDown.Value}");
            tw.WriteLine($"Number of Threads: {(int)threads_numericUpDown.Value}");
            tw.WriteLine($"Delay Between Queries (ms): {int.Parse(queryDelay_numericUpDown.Text, CultureInfo.InvariantCulture)}");
            tw.WriteLine($"CPU Seconds/Iteration (Avg): {cpuTime_textBox.Text}");
            tw.WriteLine($"Actual Seconds/Iteration (Avg): {actualSeconds_textBox.Text}");
            tw.WriteLine($"Iterations Completed: {iterationsSecond_textBox.Text}");
            tw.WriteLine($"Client Seconds/Iteration (Avg): {avgSeconds_textBox.Text}");
            tw.WriteLine($"Logical Reads/Iteration (Avg): {logicalReads_textBox.Text}");
            tw.WriteLine(string.Empty);
        }

        private void toClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using var textWriter = new StringWriter();
                WriteBenchmarkTextContent(textWriter);
                Clipboard.SetText(textWriter.ToString());
            }
            catch (Exception)
            {
                MessageBox
                    .Show("Error While Copying BenchMark to Clipboard", "There was an error copying the benchmark to clipboard");
            }
        }

        private void toCsvToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var saveFileDialog = new SaveFileDialog
            {
                AddExtension = true,
                OverwritePrompt = false,
                Filter = Resources.CsvFiles
            };
            saveFileDialog.ShowDialog();

            if (!string.IsNullOrEmpty(saveFileDialog.FileName))
                ExportBenchMarkToCsvFile(saveFileDialog.FileName);
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
                MessageBox
                    .Show("Error While Saving BenchMark",
                    $"There was an error saving the benchmark to '{fileName}', make sure you have write privileges to that path");
            }
        }

        private static void WriteBenchmarkCsvHeader(StreamWriter tw)
        {
            tw.WriteLine("TestId,TestStartTime,ElapsedTime,Iterations,Threads,Delay,CompletedIterations,AvgCPUSeconds,AvgActualSeconds,AvgClientSeconds,AvgLogicalReads");
        }

        private void WriteBenchmarkCsvText(StreamWriter tw)
        {
            tw.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}",
                _testGuid,
                _testStartTime,
                elapsedTime_textBox.Text,
                (int)iterations_numericUpDown.Value,
                (int)threads_numericUpDown.Value,
                int.Parse(queryDelay_numericUpDown.Text, CultureInfo.InvariantCulture),
                iterationsSecond_textBox.Text,
                cpuTime_textBox.Text,
                actualSeconds_textBox.Text,
                avgSeconds_textBox.Text,
                logicalReads_textBox.Text
                );
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            var elemHost = new System.Windows.Forms.Integration.ElementHost();
            sqlControl1 = new SqlControl();
            elemHost.Dock = DockStyle.Fill;
            elemHost.Location = new System.Drawing.Point(4, 5);
            elemHost.Margin = new Padding(4, 5, 4, 5);
            elemHost.Size = new System.Drawing.Size(490, 623);
            elemHost.Child = sqlControl1;
            tableLayoutPanel3.Controls.Add(elemHost, 0, 0);
        }

    }
}
#pragma warning restore CA1031 // Do not catch general exception types