using System.Windows.Forms;

namespace SQLQueryStress
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (backgroundWorker1.CancellationPending)
                System.Windows.Forms.MessageBox.Show("Please wait while background threads are cancelled.");
            else if (backgroundWorker1.IsBusy)
            {
                if (System.Windows.Forms.MessageBox.Show(
                "A test is currently running.  Cancel and shut down?",
                "Really Close?",
                System.Windows.Forms.MessageBoxButtons.YesNo,
                System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    cancel_button_Click(new System.String(' ', 0), null);
                }
            }
            else
            {
                if (disposing && (components != null))
                {
                    components.Dispose();
                }
                base.Dispose(disposing);
            }
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveBenchMarkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toCsvToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.go_button = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.iterations_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.threads_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.cancel_button = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.label5 = new System.Windows.Forms.Label();
            this.avgSeconds_textBox = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.totalExceptions_textBox = new System.Windows.Forms.Label();
            this.mainUITimer = new System.Windows.Forms.Timer(this.components);
            this.label8 = new System.Windows.Forms.Label();
            this.elapsedTime_textBox = new System.Windows.Forms.Label();
            this.perfCounterTimer = new System.Windows.Forms.Timer(this.components);
            this.database_button = new System.Windows.Forms.Button();
            this.iterationsSecond_textBox = new System.Windows.Forms.Label();
            this.activeThreads_textBox = new System.Windows.Forms.Label();
            this.activeThreads_label = new System.Windows.Forms.Label();
            this.exceptions_button = new System.Windows.Forms.Button();
            this.saveSettingsFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.loadSettingsFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.label9 = new System.Windows.Forms.Label();
            this.cpuTime_textBox = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.logicalReads_textBox = new System.Windows.Forms.Label();
            this.db_label = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label10 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.actualSeconds_textBox = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.param_button = new System.Windows.Forms.Button();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.btnFreeCache = new System.Windows.Forms.Button();
            this.btnCleanBuffer = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.queryDelay_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iterations_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.threads_numericUpDown)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.queryDelay_numericUpDown)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(11, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Query";
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.MenuBar;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(865, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.optionsToolStripMenuItem,
            this.saveSettingsToolStripMenuItem,
            this.loadSettingsToolStripMenuItem,
            this.saveBenchMarkToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(158, 6);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.optionsToolStripMenuItem.Text = "Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // saveSettingsToolStripMenuItem
            // 
            this.saveSettingsToolStripMenuItem.Name = "saveSettingsToolStripMenuItem";
            this.saveSettingsToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.saveSettingsToolStripMenuItem.Text = "Save Settings";
            this.saveSettingsToolStripMenuItem.Click += new System.EventHandler(this.saveSettingsToolStripMenuItem_Click);
            // 
            // loadSettingsToolStripMenuItem
            // 
            this.loadSettingsToolStripMenuItem.Name = "loadSettingsToolStripMenuItem";
            this.loadSettingsToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.loadSettingsToolStripMenuItem.Text = "Load Settings";
            this.loadSettingsToolStripMenuItem.Click += new System.EventHandler(this.loadSettingsToolStripMenuItem_Click);
            // 
            // saveBenchMarkToolStripMenuItem
            // 
            this.saveBenchMarkToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toCsvToolStripMenuItem,
            this.toTextToolStripMenuItem,
            this.toClipboardToolStripMenuItem});
            this.saveBenchMarkToolStripMenuItem.Name = "saveBenchMarkToolStripMenuItem";
            this.saveBenchMarkToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.saveBenchMarkToolStripMenuItem.Text = "Save BenchMark";
            // 
            // toCsvToolStripMenuItem
            // 
            this.toCsvToolStripMenuItem.Name = "toCsvToolStripMenuItem";
            this.toCsvToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.toCsvToolStripMenuItem.Text = "To Csv";
            this.toCsvToolStripMenuItem.Click += new System.EventHandler(this.toCsvToolStripMenuItem_Click);
            // 
            // toTextToolStripMenuItem
            // 
            this.toTextToolStripMenuItem.Name = "toTextToolStripMenuItem";
            this.toTextToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.toTextToolStripMenuItem.Text = "To Text";
            this.toTextToolStripMenuItem.Click += new System.EventHandler(this.toTextToolStripMenuItem_Click);
            // 
            // toClipboardToolStripMenuItem
            // 
            this.toClipboardToolStripMenuItem.Name = "toClipboardToolStripMenuItem";
            this.toClipboardToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.toClipboardToolStripMenuItem.Text = "To Clipboard";
            this.toClipboardToolStripMenuItem.Click += new System.EventHandler(this.toClipboardToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // go_button
            // 
            this.go_button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.go_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.go_button.Location = new System.Drawing.Point(3, 4);
            this.go_button.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.go_button.Name = "go_button";
            this.go_button.Size = new System.Drawing.Size(109, 44);
            this.go_button.TabIndex = 0;
            this.go_button.Text = "GO";
            this.go_button.UseVisualStyleBackColor = true;
            this.go_button.Click += new System.EventHandler(this.go_button_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(3, 174);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(230, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "Number of Iterations";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(3, 231);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(230, 16);
            this.label3.TabIndex = 7;
            this.label3.Text = "Number of Threads";
            // 
            // iterations_numericUpDown
            // 
            this.iterations_numericUpDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.iterations_numericUpDown.Location = new System.Drawing.Point(3, 194);
            this.iterations_numericUpDown.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.iterations_numericUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.iterations_numericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.iterations_numericUpDown.Name = "iterations_numericUpDown";
            this.iterations_numericUpDown.Size = new System.Drawing.Size(229, 20);
            this.iterations_numericUpDown.TabIndex = 3;
            this.iterations_numericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // threads_numericUpDown
            // 
            this.threads_numericUpDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.threads_numericUpDown.Location = new System.Drawing.Point(3, 251);
            this.threads_numericUpDown.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.threads_numericUpDown.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.threads_numericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.threads_numericUpDown.Name = "threads_numericUpDown";
            this.threads_numericUpDown.Size = new System.Drawing.Size(229, 20);
            this.threads_numericUpDown.TabIndex = 4;
            this.threads_numericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cancel_button
            // 
            this.cancel_button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cancel_button.Enabled = false;
            this.cancel_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.cancel_button.Location = new System.Drawing.Point(118, 4);
            this.cancel_button.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cancel_button.Name = "cancel_button";
            this.cancel_button.Size = new System.Drawing.Size(109, 44);
            this.cancel_button.TabIndex = 1;
            this.cancel_button.Text = "Cancel";
            this.cancel_button.UseVisualStyleBackColor = true;
            this.cancel_button.Click += new System.EventHandler(this.cancel_button_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(239, 174);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(230, 16);
            this.label4.TabIndex = 12;
            this.label4.Text = "Iterations Completed";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label5.Location = new System.Drawing.Point(239, 231);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(230, 16);
            this.label5.TabIndex = 14;
            this.label5.Text = "Client Seconds/Iteration (Avg)";
            // 
            // avgSeconds_textBox
            // 
            this.avgSeconds_textBox.BackColor = System.Drawing.Color.Black;
            this.avgSeconds_textBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.avgSeconds_textBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.avgSeconds_textBox.Font = new System.Drawing.Font("Courier New", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.avgSeconds_textBox.ForeColor = System.Drawing.Color.Lime;
            this.avgSeconds_textBox.Location = new System.Drawing.Point(239, 251);
            this.avgSeconds_textBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.avgSeconds_textBox.Name = "avgSeconds_textBox";
            this.avgSeconds_textBox.Size = new System.Drawing.Size(230, 33);
            this.avgSeconds_textBox.TabIndex = 12;
            this.avgSeconds_textBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // progressBar1
            // 
            this.progressBar1.BackColor = System.Drawing.SystemColors.Control;
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar1.Location = new System.Drawing.Point(239, 80);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(230, 33);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label6.Location = new System.Drawing.Point(239, 60);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(230, 16);
            this.label6.TabIndex = 16;
            this.label6.Text = "Progress";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label7.Location = new System.Drawing.Point(239, 288);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(230, 16);
            this.label7.TabIndex = 18;
            this.label7.Text = "Total Exceptions";
            // 
            // totalExceptions_textBox
            // 
            this.totalExceptions_textBox.BackColor = System.Drawing.Color.Black;
            this.totalExceptions_textBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.totalExceptions_textBox.Font = new System.Drawing.Font("Courier New", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.totalExceptions_textBox.ForeColor = System.Drawing.Color.Red;
            this.totalExceptions_textBox.Location = new System.Drawing.Point(3, 4);
            this.totalExceptions_textBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.totalExceptions_textBox.Name = "totalExceptions_textBox";
            this.totalExceptions_textBox.Size = new System.Drawing.Size(180, 34);
            this.totalExceptions_textBox.TabIndex = 1;
            this.totalExceptions_textBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.totalExceptions_textBox.Click += new System.EventHandler(this.totalExceptions_textBox_Click);
            // 
            // mainUITimer
            // 
            this.mainUITimer.Tick += new System.EventHandler(this.mainUITimer_Tick);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label8.Location = new System.Drawing.Point(239, 117);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(230, 16);
            this.label8.TabIndex = 20;
            this.label8.Text = "Elapsed Time";
            // 
            // elapsedTime_textBox
            // 
            this.elapsedTime_textBox.BackColor = System.Drawing.Color.Black;
            this.elapsedTime_textBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.elapsedTime_textBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elapsedTime_textBox.Font = new System.Drawing.Font("Courier New", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.elapsedTime_textBox.ForeColor = System.Drawing.Color.Lime;
            this.elapsedTime_textBox.Location = new System.Drawing.Point(239, 137);
            this.elapsedTime_textBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.elapsedTime_textBox.Name = "elapsedTime_textBox";
            this.elapsedTime_textBox.Size = new System.Drawing.Size(230, 33);
            this.elapsedTime_textBox.TabIndex = 10;
            this.elapsedTime_textBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // perfCounterTimer
            // 
            this.perfCounterTimer.Interval = 2500;
            // 
            // database_button
            // 
            this.database_button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.database_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.database_button.Location = new System.Drawing.Point(3, 80);
            this.database_button.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.database_button.Name = "database_button";
            this.database_button.Size = new System.Drawing.Size(230, 33);
            this.database_button.TabIndex = 1;
            this.database_button.Text = "Database";
            this.database_button.UseVisualStyleBackColor = true;
            this.database_button.Click += new System.EventHandler(this.database_button_Click);
            // 
            // iterationsSecond_textBox
            // 
            this.iterationsSecond_textBox.BackColor = System.Drawing.Color.Black;
            this.iterationsSecond_textBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.iterationsSecond_textBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.iterationsSecond_textBox.Font = new System.Drawing.Font("Courier New", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.iterationsSecond_textBox.ForeColor = System.Drawing.Color.Lime;
            this.iterationsSecond_textBox.Location = new System.Drawing.Point(239, 194);
            this.iterationsSecond_textBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.iterationsSecond_textBox.Name = "iterationsSecond_textBox";
            this.iterationsSecond_textBox.Size = new System.Drawing.Size(230, 33);
            this.iterationsSecond_textBox.TabIndex = 11;
            this.iterationsSecond_textBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // activeThreads_textBox
            // 
            this.activeThreads_textBox.BackColor = System.Drawing.Color.Black;
            this.activeThreads_textBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.activeThreads_textBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.activeThreads_textBox.Font = new System.Drawing.Font("Courier New", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.activeThreads_textBox.ForeColor = System.Drawing.Color.Lime;
            this.activeThreads_textBox.Location = new System.Drawing.Point(239, 422);
            this.activeThreads_textBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.activeThreads_textBox.Name = "activeThreads_textBox";
            this.activeThreads_textBox.Size = new System.Drawing.Size(230, 34);
            this.activeThreads_textBox.TabIndex = 30;
            this.activeThreads_textBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // activeThreads_label
            // 
            this.activeThreads_label.AutoSize = true;
            this.activeThreads_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.activeThreads_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.activeThreads_label.Location = new System.Drawing.Point(239, 402);
            this.activeThreads_label.Name = "activeThreads_label";
            this.activeThreads_label.Size = new System.Drawing.Size(230, 16);
            this.activeThreads_label.TabIndex = 29;
            this.activeThreads_label.Text = "Active Threads";
            // 
            // exceptions_button
            // 
            this.exceptions_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.exceptions_button.Location = new System.Drawing.Point(189, 4);
            this.exceptions_button.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.exceptions_button.Name = "exceptions_button";
            this.exceptions_button.Size = new System.Drawing.Size(31, 26);
            this.exceptions_button.TabIndex = 1;
            this.exceptions_button.Text = "...";
            this.exceptions_button.UseVisualStyleBackColor = true;
            this.exceptions_button.Click += new System.EventHandler(this.exceptions_button_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label9.Location = new System.Drawing.Point(3, 345);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(230, 16);
            this.label9.TabIndex = 26;
            this.label9.Text = "CPU Seconds/Iteration (Avg)";
            // 
            // cpuTime_textBox
            // 
            this.cpuTime_textBox.BackColor = System.Drawing.Color.Black;
            this.cpuTime_textBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.cpuTime_textBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cpuTime_textBox.Font = new System.Drawing.Font("Courier New", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.cpuTime_textBox.ForeColor = System.Drawing.Color.Lime;
            this.cpuTime_textBox.Location = new System.Drawing.Point(3, 365);
            this.cpuTime_textBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cpuTime_textBox.Name = "cpuTime_textBox";
            this.cpuTime_textBox.Size = new System.Drawing.Size(230, 33);
            this.cpuTime_textBox.TabIndex = 6;
            this.cpuTime_textBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label12.Location = new System.Drawing.Point(239, 345);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(230, 16);
            this.label12.TabIndex = 30;
            this.label12.Text = "Logical Reads/Iteration (Avg)";
            // 
            // logicalReads_textBox
            // 
            this.logicalReads_textBox.BackColor = System.Drawing.Color.Black;
            this.logicalReads_textBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.logicalReads_textBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logicalReads_textBox.Font = new System.Drawing.Font("Courier New", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.logicalReads_textBox.ForeColor = System.Drawing.Color.Lime;
            this.logicalReads_textBox.Location = new System.Drawing.Point(239, 365);
            this.logicalReads_textBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.logicalReads_textBox.Name = "logicalReads_textBox";
            this.logicalReads_textBox.Size = new System.Drawing.Size(230, 33);
            this.logicalReads_textBox.TabIndex = 14;
            this.logicalReads_textBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // db_label
            // 
            this.db_label.AutoSize = true;
            this.db_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.db_label.Location = new System.Drawing.Point(64, 37);
            this.db_label.Name = "db_label";
            this.db_label.Size = new System.Drawing.Size(0, 13);
            this.db_label.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 236F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 236F));
            this.tableLayoutPanel1.Controls.Add(this.logicalReads_textBox, 1, 12);
            this.tableLayoutPanel1.Controls.Add(this.activeThreads_label, 1, 13);
            this.tableLayoutPanel1.Controls.Add(this.activeThreads_textBox, 1, 14);
            this.tableLayoutPanel1.Controls.Add(this.label12, 1, 11);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.iterations_numericUpDown, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.threads_numericUpDown, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.database_button, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.label10, 0, 13);
            this.tableLayoutPanel1.Controls.Add(this.cpuTime_textBox, 0, 12);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 10);
            this.tableLayoutPanel1.Controls.Add(this.actualSeconds_textBox, 0, 14);
            this.tableLayoutPanel1.Controls.Add(this.label7, 1, 9);
            this.tableLayoutPanel1.Controls.Add(this.avgSeconds_textBox, 1, 8);
            this.tableLayoutPanel1.Controls.Add(this.label5, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.iterationsSecond_textBox, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.label4, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.label6, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label8, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.progressBar1, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.elapsedTime_textBox, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.param_button, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label11, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.queryDelay_numericUpDown, 0, 10);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(391, 4);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 15;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 41F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 41F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 41F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 41F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 41F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 41F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(471, 460);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label10.Location = new System.Drawing.Point(3, 402);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(230, 16);
            this.label10.TabIndex = 28;
            this.label10.Text = "Actual Seconds/Iteration (Avg)";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.totalExceptions_textBox);
            this.flowLayoutPanel1.Controls.Add(this.exceptions_button);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(239, 308);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(230, 33);
            this.flowLayoutPanel1.TabIndex = 13;
            // 
            // actualSeconds_textBox
            // 
            this.actualSeconds_textBox.BackColor = System.Drawing.Color.Black;
            this.actualSeconds_textBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.actualSeconds_textBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.actualSeconds_textBox.Font = new System.Drawing.Font("Courier New", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.actualSeconds_textBox.ForeColor = System.Drawing.Color.Lime;
            this.actualSeconds_textBox.Location = new System.Drawing.Point(3, 422);
            this.actualSeconds_textBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.actualSeconds_textBox.Name = "actualSeconds_textBox";
            this.actualSeconds_textBox.Size = new System.Drawing.Size(230, 34);
            this.actualSeconds_textBox.TabIndex = 7;
            this.actualSeconds_textBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.cancel_button, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.go_button, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(239, 4);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(230, 52);
            this.tableLayoutPanel2.TabIndex = 8;
            // 
            // param_button
            // 
            this.param_button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.param_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.param_button.Location = new System.Drawing.Point(3, 137);
            this.param_button.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.param_button.Name = "param_button";
            this.param_button.Size = new System.Drawing.Size(230, 33);
            this.param_button.TabIndex = 2;
            this.param_button.Text = "Parameter Substitution";
            this.param_button.UseVisualStyleBackColor = true;
            this.param_button.Click += new System.EventHandler(this.param_button_Click);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.btnFreeCache, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnCleanBuffer, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 4);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(230, 52);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // btnFreeCache
            // 
            this.btnFreeCache.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFreeCache.Location = new System.Drawing.Point(117, 2);
            this.btnFreeCache.Margin = new System.Windows.Forms.Padding(2);
            this.btnFreeCache.Name = "btnFreeCache";
            this.btnFreeCache.Size = new System.Drawing.Size(111, 22);
            this.btnFreeCache.TabIndex = 1;
            this.btnFreeCache.Text = "Free Cache";
            this.btnFreeCache.UseVisualStyleBackColor = true;
            this.btnFreeCache.Click += new System.EventHandler(this.btnFreeCache_Click);
            // 
            // btnCleanBuffer
            // 
            this.btnCleanBuffer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCleanBuffer.Location = new System.Drawing.Point(2, 2);
            this.btnCleanBuffer.Margin = new System.Windows.Forms.Padding(2);
            this.btnCleanBuffer.Name = "btnCleanBuffer";
            this.btnCleanBuffer.Size = new System.Drawing.Size(111, 22);
            this.btnCleanBuffer.TabIndex = 0;
            this.btnCleanBuffer.Text = "Clean Buffers";
            this.btnCleanBuffer.UseVisualStyleBackColor = true;
            this.btnCleanBuffer.Click += new System.EventHandler(this.btnCleanBuffer_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label11.Location = new System.Drawing.Point(3, 288);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(230, 16);
            this.label11.TabIndex = 34;
            this.label11.Text = "Delay between queries (ms)";
            // 
            // queryDelay_numericUpDown
            // 
            this.queryDelay_numericUpDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.queryDelay_numericUpDown.Location = new System.Drawing.Point(3, 308);
            this.queryDelay_numericUpDown.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.queryDelay_numericUpDown.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.queryDelay_numericUpDown.Name = "queryDelay_numericUpDown";
            this.queryDelay_numericUpDown.Size = new System.Drawing.Size(230, 23);
            this.queryDelay_numericUpDown.TabIndex = 5;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel1, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(865, 477);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(865, 501);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Controls.Add(this.db_label);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MinimumSize = new System.Drawing.Size(867, 533);
            this.Name = "Form1";
            this.Text = "SQLQueryStress";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iterations_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.threads_numericUpDown)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.queryDelay_numericUpDown)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Button go_button;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown iterations_numericUpDown;
        private System.Windows.Forms.NumericUpDown threads_numericUpDown;
        private System.Windows.Forms.Button cancel_button;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label avgSeconds_textBox;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label totalExceptions_textBox;
        private System.Windows.Forms.Timer mainUITimer;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label elapsedTime_textBox;
        private System.Windows.Forms.Timer perfCounterTimer;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Button database_button;
        private System.Windows.Forms.Label iterationsSecond_textBox;
        private System.Windows.Forms.ToolStripMenuItem saveSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadSettingsToolStripMenuItem;
        private System.Windows.Forms.Button exceptions_button;
        private System.Windows.Forms.SaveFileDialog saveSettingsFileDialog;
        private System.Windows.Forms.OpenFileDialog loadSettingsFileDialog;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label cpuTime_textBox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label logicalReads_textBox;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.Label db_label;
        private TableLayoutPanel tableLayoutPanel1;
        private FlowLayoutPanel flowLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private Button param_button;
        private TableLayoutPanel tableLayoutPanel3;
        private TableLayoutPanel tableLayoutPanel4;
        private Button btnFreeCache;
        private Button btnCleanBuffer;
        private Label actualSeconds_textBox;
        private Label label10;
        private Label label11;
        private NumericUpDown queryDelay_numericUpDown;
        private ToolStripMenuItem saveBenchMarkToolStripMenuItem;
        private ToolStripMenuItem toCsvToolStripMenuItem;
        private ToolStripMenuItem toTextToolStripMenuItem;
        private ToolStripMenuItem toClipboardToolStripMenuItem;
        private System.Windows.Forms.Label activeThreads_label;
        private System.Windows.Forms.Label activeThreads_textBox;
    }
}

