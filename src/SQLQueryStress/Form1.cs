using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Runtime.Serialization;

/*********************************************
TODO, version 1.0::::
 * figure out how to capture change of selection in parameter definer
 * Bug: Dotfuscated version crashes in some cases (need to verify this)
 * Bug: Dotfuscated version can't load non-dotfuscated .sqlstress files
 * 
 * Throw a message box if param database has not been selected, or if can't connect (if parameterization is on)
 * repaint exception window when datatable is updated
  * Bug w/ stats sometimes going blank ??
 *********************************************/

namespace SQLQueryStress
{
    public partial class Form1 : Form
    {
        /* Runtime locals */

        //total iterations that have run
        private int totalIterations;

        //This is the total time as reported by the client
        private double totalTime;

        //total elapsed time in milliseconds
        private double totalElapsedTime;
        //total CPU time in milliseconds
        private double totalCPUTime;
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
        private int totalTimeMessages;

        //Same comments as above for these two...
        private double totalLogicalReads;
        private int totalReadMessages;

        //total exceptions
        private int totalExceptions;
        //Exceptions that occurred
        private Dictionary<string, int> exceptions;

        //The exception viewer window
        DataViewer exceptionViewer;

        //threads * iterations
        private int totalExpectedIterations;

        //start of the load
        private TimeSpan start;

        //Has this run been cancelled?
        private bool cancelled;
        //Exit as soon as cancellation is finished?
        private bool exitOnComplete;

        /* Configuration local */
        QueryStressSettings settings = new QueryStressSettings();

        public Form1(string configFile) : this()
        {
            openConfigFile(configFile);
        }

        public Form1()
        {
            InitializeComponent();

            saveFileDialog1.DefaultExt = "sqlstress";
            saveFileDialog1.Filter = @"SQLQueryStress Configuration Files|*.sqlstress";
            saveFileDialog1.FileOk += new CancelEventHandler(saveFileDialog1_FileOk);

            openFileDialog1.DefaultExt = "sqlstress";
            openFileDialog1.Filter = @"SQLQueryStress Configuration Files|*.sqlstress";
            openFileDialog1.FileOk += new CancelEventHandler(openFileDialog1_FileOk);
        }

        private void go_button_Click(object sender, EventArgs e)
        {
            if (!this.settings.mainDBConnectionInfo.TestConnection())
            {
                MessageBox.Show("You must set valid database connection information. Click the Database button to configure the settings.");
                return;
            }

            this.cancelled = false;
            this.exitOnComplete = false;

            this.exceptions = new Dictionary<string,int>();

            this.totalIterations = 0;
            this.totalTime = 0;
            this.totalCPUTime = 0;
            this.totalElapsedTime = 0;
            this.totalTimeMessages = 0;
            this.totalLogicalReads = 0;
            this.totalReadMessages = 0;
            this.totalExceptions = 0;

            this.iterationsSecond_textBox.Text = "0";
            this.avgSeconds_textBox.Text = "0.0";
            this.actualSeconds_textBox.Text = "---";
            this.cpuTime_textBox.Text = "---";
            this.logicalReads_textBox.Text = "---";
            this.go_button.Enabled = false;
            this.cancel_button.Enabled = true;
            this.iterations_numericUpDown.Enabled = false;
            this.threads_numericUpDown.Enabled = false;
            
            this.progressBar1.Value = 0;

            SaveSettingsFromForm1();

            this.totalExpectedIterations = this.settings.numThreads * this.settings.numIterations;

            DatabaseSelect.ConnectionInfo paramConnectionInfo = settings.shareDBSettings ? settings.mainDBConnectionInfo : settings.paramDBConnectionInfo;
            db_label.Text = "" +
                "Server: " + paramConnectionInfo.Server +
                ((paramConnectionInfo.Database.Length > 0) ? ("  //  Database: " + paramConnectionInfo.Database) : (""));

            LoadEngine engine = new LoadEngine(
                this.settings.mainDBConnectionInfo.ConnectionString,
                this.settings.mainQuery, 
                this.settings.numThreads, 
                this.settings.numIterations,
                this.settings.paramQuery,
                this.settings.paramMappings, 
                paramConnectionInfo.ConnectionString,
                this.settings.commandTimeout,
                this.settings.collectIOStats,
                this.settings.collectTimeStats,
                this.settings.forceDataRetrieval);
            backgroundWorker1.RunWorkerAsync(engine);

            this.start = new TimeSpan(DateTime.Now.Ticks);

            mainUITimer.Start();
        }

        private void backgroundWorker1_DoWork(
            object sender,
            DoWorkEventArgs e)
        {
            ((LoadEngine)e.Argument).StartLoad(backgroundWorker1);
        }

        private void backgroundWorker1_ProgressChanged(
            object sender,
            ProgressChangedEventArgs e)
        {
            LoadEngine.queryOutput output = (LoadEngine.queryOutput)e.UserState;

            this.totalIterations++;

            if (output.LogicalReads > 0)
            {
                totalReadMessages++;
                totalLogicalReads += output.LogicalReads;
            }

            if (output.ElapsedTime > 0)
            {
                totalTimeMessages++;
                totalCPUTime += output.CPUTime;
                totalElapsedTime += output.ElapsedTime;
            }

            totalTime += output.time.TotalMilliseconds;

            if (output.e != null)
            {
                this.totalExceptions++;
                string theMessage = null;

                //strip the time stats, if they showed up as part
                //of the exception
                if (this.settings.collectTimeStats)
                {
#if DEBUG
                    this.exceptions.Add(output.e.Message + " !Source: " + output.e.Source + " !TargetSite: " + output.e.TargetSite.ToString() + " !Exception Type: " + output.e.GetType().ToString());
#else
                    int matchPos = output.e.Message.IndexOf("SQL Server parse and compile time:");

                    if (matchPos > -1)
                        theMessage = output.e.Message.Substring(0, matchPos - 2);
                    else
                        theMessage = output.e.Message;
#endif
                }
                else
                {
                    theMessage = output.e.Message;
                }

                if (!exceptions.ContainsKey(theMessage))
                {
                    exceptions.Add(theMessage, 1);
                }
                else
                {
                    exceptions[theMessage] += 1;
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

        private void backgroundWorker1_RunWorkerCompleted(
            object sender, 
            RunWorkerCompletedEventArgs e)
        {
            mainUITimer.Stop();

            updateUI();

            this.go_button.Enabled = true;
            this.cancel_button.Enabled = false;            
            this.threads_numericUpDown.Enabled = true;
            this.iterations_numericUpDown.Enabled = true;

            if (!this.cancelled)
                this.progressBar1.Value = 100;

            ((BackgroundWorker)sender).Dispose();

            this.db_label.Text = "";

            if (this.exitOnComplete)
            {
                this.Dispose();
            }
        }

        private void updateUI()
        {
            this.iterationsSecond_textBox.Text = totalIterations.ToString();
            double avgIterations = (totalIterations == 0) ? 0.0 : ((totalTime / (double)totalIterations) / 1000);
            double avgCPU = (totalTimeMessages == 0) ? 0.0 : ((totalCPUTime / (double)totalTimeMessages) / 1000);
            double avgActual = (totalTimeMessages == 0) ? 0.0 : ((totalElapsedTime / (double)totalTimeMessages) / 1000);
            double avgReads = (totalReadMessages == 0) ? 0.0 : ((totalLogicalReads / totalReadMessages));

            this.avgSeconds_textBox.Text = avgIterations.ToString("0.0000");
            this.cpuTime_textBox.Text = (totalTimeMessages == 0) ? "---" : avgCPU.ToString("0.0000");
            this.actualSeconds_textBox.Text = (totalTimeMessages == 0) ? "---" : avgActual.ToString("0.0000");
            this.logicalReads_textBox.Text = (totalReadMessages == 0) ? "---" : avgReads.ToString("0.0000");

            this.totalExceptions_textBox.Text = totalExceptions.ToString();
            this.progressBar1.Value = (int)(((decimal)totalIterations / (decimal)totalExpectedIterations) * 100);

            TimeSpan end = new TimeSpan(DateTime.Now.Ticks);
            end = end.Subtract(start);

            string theTime = end.ToString();

            //Some systems return "hh:mm:ss" instead of "hh:mm:ss.0000" if
            //there is no fractional part of the second.  I'm not sure
            //why, but this fixes it for now.
            if (theTime.Length > 8)
                this.elapsedTime_textBox.Text = theTime.Substring(0, 13);            
            else
                this.elapsedTime_textBox.Text = (theTime + ".0000");
        }

        private void mainUITimer_Tick(object sender, EventArgs e)
        {
            updateUI();
        }

        private void cancel_button_Click(object sender, EventArgs e)
        {
            this.cancel_button.Enabled = false;

            backgroundWorker1.CancelAsync();
            
            this.cancelled = true;

            if (sender is String)
            {
                this.exitOnComplete = true;
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox a = new AboutBox();
            a.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void param_button_Click(object sender, EventArgs e)
        {
            ParamWindow p = new ParamWindow(this.settings, this.query_textBox.Text);
            p.StartPosition = FormStartPosition.CenterParent;
            p.ShowDialog();
        }

        private void totalExceptions_textBox_Click(object sender, EventArgs e)
        {
            this.exceptionViewer = new DataViewer();

            this.exceptionViewer.StartPosition = FormStartPosition.CenterParent;

            this.exceptionViewer.Text = "Exceptions";

            DataTable dt = new DataTable();
            dt.Columns.Add("Count");
            dt.Columns.Add("Exception");

            if (exceptions != null)
            {
                Dictionary<string, int>.ValueCollection.Enumerator values = this.exceptions.Values.GetEnumerator();

                foreach (string ex in this.exceptions.Keys)
                {
                    values.MoveNext();
                    int count = values.Current;
                    dt.Rows.Add(count, ex);
                }
            }

            this.exceptionViewer.DataView = dt;

            this.exceptionViewer.ShowDialog();
        }

        private void database_button_Click(object sender, EventArgs e)
        {
            DatabaseSelect dbselect = new DatabaseSelect(this.settings);
            dbselect.StartPosition = FormStartPosition.CenterParent;
            dbselect.ShowDialog();
        }

        private void exceptions_button_Click(object sender, EventArgs e)
        {
            totalExceptions_textBox_Click(null, null);
        }

        [Serializable]
        public class QueryStressSettings
        {
            public QueryStressSettings()
            {
                mainDBConnectionInfo = new DatabaseSelect.ConnectionInfo(this);
                shareDBSettings = true;
                paramDBConnectionInfo = new DatabaseSelect.ConnectionInfo();
                mainQuery = "";
                paramQuery = "";
                numThreads = 1;
                numIterations = 1;
                paramMappings = new Dictionary<string, string>();
                connectionTimeout = 15;
                commandTimeout = 0;
                enableConnectionPooling = true;
                collectIOStats = true;
                collectTimeStats = true;
                forceDataRetrieval = false;
            }

            [OnDeserialized]
            internal void fixSettings(StreamingContext context)
            {
                connectionTimeout = (connectionTimeout == 0) ? 15 : connectionTimeout;
            }

            /// <summary>
            /// Connection info for the DB in which to run the test
            /// </summary>
            public DatabaseSelect.ConnectionInfo mainDBConnectionInfo;

            /// <summary>
            /// Should the main db and param db share the same settings?
            /// If so, use main db settings for the params
            /// </summary>
            public bool shareDBSettings;

            /// <summary>
            /// Connection info for the DB from which to get the paramaters
            /// </summary>
            public DatabaseSelect.ConnectionInfo paramDBConnectionInfo;

            /// <summary>
            /// main query to test
            /// </summary>
            public string mainQuery;
            /// <summary>
            /// query from which to take parameters
            /// </summary>
            public string paramQuery;

            /// <summary>
            /// Number of threads to test with
            /// </summary>
            public int numThreads;
            /// <summary>
            /// Number of iterations to run per thread
            /// </summary>
            public int numIterations;

            /// <summary>
            /// mapped parameters
            /// </summary>
            public Dictionary<string, string> paramMappings;

            /// <summary>
            /// Connection Timeout
            /// </summary>
            public int connectionTimeout;

            /// <summary>
            /// command timeout
            /// </summary>
            public int commandTimeout;

            /// <summary>
            /// Enable pooling?
            /// </summary>
            public bool enableConnectionPooling;

            /// <summary>
            /// Collect I/O stats?
            /// </summary>
            public bool collectIOStats;

            /// <summary>
            /// Collect time stats?
            /// </summary>
            public bool collectTimeStats;

            /// <summary>
            /// Force the client to retrieve all data?
            /// </summary>
            public bool forceDataRetrieval;
        }

        private void SaveSettingsFromForm1()
        {
            settings.mainQuery = query_textBox.Text;
            settings.numThreads = (int)threads_numericUpDown.Value;
            settings.numIterations = (int)iterations_numericUpDown.Value;
        }

        private void saveSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveSettingsFromForm1();
            saveFileDialog1.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, EventArgs e)
        {
            System.IO.FileStream fs = null;

            try
            {
                fs = new System.IO.FileStream(saveFileDialog1.FileName, System.IO.FileMode.Create);
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                bf.Serialize(fs, this.settings);
            }
            catch
            {
                MessageBox.Show("Error saving settings.");
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }
        }

        private void loadSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, EventArgs e)
        {
            openConfigFile(openFileDialog1.FileName);
        }

        private void openConfigFile(string FileName)
        {
            System.IO.FileStream fs = null;

            try
            {
                fs = new System.IO.FileStream(FileName, System.IO.FileMode.Open);
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                this.settings = (QueryStressSettings)bf.Deserialize(fs);
            }
            catch
            {
                MessageBox.Show("Error loading settings.");
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }

            query_textBox.Text = settings.mainQuery;
            threads_numericUpDown.Value = settings.numThreads;
            iterations_numericUpDown.Value = settings.numIterations;
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Options options = new Options(this.settings);
            options.ShowDialog();
        }
    }
}