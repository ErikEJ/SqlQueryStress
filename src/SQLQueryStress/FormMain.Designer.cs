using SQLQueryStress.Properties;
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
                System.Windows.Forms.MessageBox.Show("Please wait while background threads are canceled.", Resources.AppTitle);
            else if (backgroundWorker1.IsBusy)
            {
                if (System.Windows.Forms.MessageBox.Show(
                    "Really Close? A test is currently running.  Cancel and shut down?",
                    Resources.AppTitle,
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            label1 = new Label();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            optionsToolStripMenuItem = new ToolStripMenuItem();
            saveSettingsToolStripMenuItem = new ToolStripMenuItem();
            loadSettingsToolStripMenuItem = new ToolStripMenuItem();
            saveBenchMarkToolStripMenuItem = new ToolStripMenuItem();
            toCsvToolStripMenuItem = new ToolStripMenuItem();
            toTextToolStripMenuItem = new ToolStripMenuItem();
            toClipboardToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            go_button = new Button();
            label2 = new Label();
            label3 = new Label();
            iterations_numericUpDown = new NumericUpDown();
            threads_numericUpDown = new NumericUpDown();
            cancel_button = new Button();
            label4 = new Label();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            label5 = new Label();
            avgSeconds_textBox = new Label();
            progressBar1 = new ProgressBar();
            label6 = new Label();
            label7 = new Label();
            totalExceptions_textBox = new Label();
            mainUITimer = new Timer(components);
            label8 = new Label();
            elapsedTime_textBox = new Label();
            perfCounterTimer = new Timer(components);
            database_button = new Button();
            iterationsSecond_textBox = new Label();
            activeThreads_textBox = new Label();
            activeThreads_label = new Label();
            exceptions_button = new Button();
            saveSettingsFileDialog = new SaveFileDialog();
            loadSettingsFileDialog = new OpenFileDialog();
            label9 = new Label();
            cpuTime_textBox = new Label();
            label12 = new Label();
            logicalReads_textBox = new Label();
            db_label = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            label10 = new Label();
            flowLayoutPanel1 = new FlowLayoutPanel();
            actualSeconds_textBox = new Label();
            tableLayoutPanel2 = new TableLayoutPanel();
            param_button = new Button();
            tableLayoutPanel4 = new TableLayoutPanel();
            btnFreeCache = new Button();
            btnCleanBuffer = new Button();
            label11 = new Label();
            queryDelay_numericUpDown = new NumericUpDown();
            tableLayoutPanel3 = new TableLayoutPanel();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)iterations_numericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)threads_numericUpDown).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)queryDelay_numericUpDown).BeginInit();
            tableLayoutPanel3.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label1.Location = new System.Drawing.Point(13, 49);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(52, 17);
            label1.TabIndex = 3;
            label1.Text = "Query";
            // 
            // menuStrip1
            // 
            menuStrip1.BackColor = System.Drawing.SystemColors.MenuBar;
            menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, helpToolStripMenuItem });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(8, 3, 0, 3);
            menuStrip1.Size = new System.Drawing.Size(989, 30);
            menuStrip1.TabIndex = 2;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { toolStripSeparator1, optionsToolStripMenuItem, saveSettingsToolStripMenuItem, loadSettingsToolStripMenuItem, saveBenchMarkToolStripMenuItem, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            fileToolStripMenuItem.Text = "File";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(197, 6);
            // 
            // optionsToolStripMenuItem
            // 
            optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            optionsToolStripMenuItem.Size = new System.Drawing.Size(200, 26);
            optionsToolStripMenuItem.Text = "Options";
            optionsToolStripMenuItem.Click += optionsToolStripMenuItem_Click;
            // 
            // saveSettingsToolStripMenuItem
            // 
            saveSettingsToolStripMenuItem.Name = "saveSettingsToolStripMenuItem";
            saveSettingsToolStripMenuItem.Size = new System.Drawing.Size(200, 26);
            saveSettingsToolStripMenuItem.Text = "Save Settings";
            saveSettingsToolStripMenuItem.Click += saveSettingsToolStripMenuItem_Click;
            // 
            // loadSettingsToolStripMenuItem
            // 
            loadSettingsToolStripMenuItem.Name = "loadSettingsToolStripMenuItem";
            loadSettingsToolStripMenuItem.Size = new System.Drawing.Size(200, 26);
            loadSettingsToolStripMenuItem.Text = "Load Settings";
            loadSettingsToolStripMenuItem.Click += loadSettingsToolStripMenuItem_Click;
            // 
            // saveBenchMarkToolStripMenuItem
            // 
            saveBenchMarkToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { toCsvToolStripMenuItem, toTextToolStripMenuItem, toClipboardToolStripMenuItem });
            saveBenchMarkToolStripMenuItem.Name = "saveBenchMarkToolStripMenuItem";
            saveBenchMarkToolStripMenuItem.Size = new System.Drawing.Size(200, 26);
            saveBenchMarkToolStripMenuItem.Text = "Save BenchMark";
            // 
            // toCsvToolStripMenuItem
            // 
            toCsvToolStripMenuItem.Name = "toCsvToolStripMenuItem";
            toCsvToolStripMenuItem.Size = new System.Drawing.Size(178, 26);
            toCsvToolStripMenuItem.Text = "To Csv";
            toCsvToolStripMenuItem.Click += toCsvToolStripMenuItem_Click;
            // 
            // toTextToolStripMenuItem
            // 
            toTextToolStripMenuItem.Name = "toTextToolStripMenuItem";
            toTextToolStripMenuItem.Size = new System.Drawing.Size(178, 26);
            toTextToolStripMenuItem.Text = "To Text";
            toTextToolStripMenuItem.Click += toTextToolStripMenuItem_Click;
            // 
            // toClipboardToolStripMenuItem
            // 
            toClipboardToolStripMenuItem.Name = "toClipboardToolStripMenuItem";
            toClipboardToolStripMenuItem.Size = new System.Drawing.Size(178, 26);
            toClipboardToolStripMenuItem.Text = "To Clipboard";
            toClipboardToolStripMenuItem.Click += toClipboardToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new System.Drawing.Size(200, 26);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aboutToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new System.Drawing.Size(55, 24);
            helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new System.Drawing.Size(133, 26);
            aboutToolStripMenuItem.Text = "About";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // go_button
            // 
            go_button.Dock = DockStyle.Fill;
            go_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            go_button.Location = new System.Drawing.Point(3, 5);
            go_button.Margin = new Padding(3, 5, 3, 5);
            go_button.Name = "go_button";
            go_button.Size = new System.Drawing.Size(126, 60);
            go_button.TabIndex = 0;
            go_button.Text = "GO";
            go_button.UseVisualStyleBackColor = true;
            go_button.Click += go_button_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Fill;
            label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label2.Location = new System.Drawing.Point(3, 232);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(264, 21);
            label2.TabIndex = 5;
            label2.Text = "Number of Iterations";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Fill;
            label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label3.Location = new System.Drawing.Point(3, 308);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(264, 21);
            label3.TabIndex = 7;
            label3.Text = "Number of Threads";
            // 
            // iterations_numericUpDown
            // 
            iterations_numericUpDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            iterations_numericUpDown.Location = new System.Drawing.Point(3, 258);
            iterations_numericUpDown.Margin = new Padding(3, 5, 3, 5);
            iterations_numericUpDown.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            iterations_numericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            iterations_numericUpDown.Name = "iterations_numericUpDown";
            iterations_numericUpDown.Size = new System.Drawing.Size(262, 23);
            iterations_numericUpDown.TabIndex = 3;
            iterations_numericUpDown.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // threads_numericUpDown
            // 
            threads_numericUpDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            threads_numericUpDown.Location = new System.Drawing.Point(3, 334);
            threads_numericUpDown.Margin = new Padding(3, 5, 3, 5);
            threads_numericUpDown.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            threads_numericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            threads_numericUpDown.Name = "threads_numericUpDown";
            threads_numericUpDown.Size = new System.Drawing.Size(262, 23);
            threads_numericUpDown.TabIndex = 4;
            threads_numericUpDown.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // cancel_button
            // 
            cancel_button.Dock = DockStyle.Fill;
            cancel_button.Enabled = false;
            cancel_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            cancel_button.Location = new System.Drawing.Point(135, 5);
            cancel_button.Margin = new Padding(3, 5, 3, 5);
            cancel_button.Name = "cancel_button";
            cancel_button.Size = new System.Drawing.Size(126, 60);
            cancel_button.TabIndex = 1;
            cancel_button.Text = "Cancel";
            cancel_button.UseVisualStyleBackColor = true;
            cancel_button.Click += cancel_button_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Dock = DockStyle.Fill;
            label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label4.Location = new System.Drawing.Point(273, 232);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(264, 21);
            label4.TabIndex = 12;
            label4.Text = "Iterations Completed";
            // 
            // backgroundWorker1
            // 
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            backgroundWorker1.ProgressChanged += backgroundWorker1_ProgressChanged;
            backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Dock = DockStyle.Fill;
            label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label5.Location = new System.Drawing.Point(273, 308);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(264, 21);
            label5.TabIndex = 14;
            label5.Text = "Client Seconds/Iteration (Avg)";
            // 
            // avgSeconds_textBox
            // 
            avgSeconds_textBox.BackColor = System.Drawing.Color.Black;
            avgSeconds_textBox.Dock = DockStyle.Fill;
            avgSeconds_textBox.Font = new System.Drawing.Font("Courier New", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            avgSeconds_textBox.ForeColor = System.Drawing.Color.Lime;
            avgSeconds_textBox.Location = new System.Drawing.Point(273, 334);
            avgSeconds_textBox.Margin = new Padding(3, 5, 3, 5);
            avgSeconds_textBox.Name = "avgSeconds_textBox";
            avgSeconds_textBox.Size = new System.Drawing.Size(264, 45);
            avgSeconds_textBox.TabIndex = 12;
            avgSeconds_textBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // progressBar1
            // 
            progressBar1.BackColor = System.Drawing.SystemColors.Control;
            progressBar1.Dock = DockStyle.Fill;
            progressBar1.Location = new System.Drawing.Point(273, 106);
            progressBar1.Margin = new Padding(3, 5, 3, 5);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new System.Drawing.Size(264, 45);
            progressBar1.Style = ProgressBarStyle.Continuous;
            progressBar1.TabIndex = 9;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Dock = DockStyle.Fill;
            label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label6.Location = new System.Drawing.Point(273, 80);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(264, 21);
            label6.TabIndex = 16;
            label6.Text = "Progress";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Dock = DockStyle.Fill;
            label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label7.Location = new System.Drawing.Point(273, 384);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(264, 21);
            label7.TabIndex = 18;
            label7.Text = "Total Exceptions";
            // 
            // totalExceptions_textBox
            // 
            totalExceptions_textBox.BackColor = System.Drawing.Color.Black;
            totalExceptions_textBox.Font = new System.Drawing.Font("Courier New", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            totalExceptions_textBox.ForeColor = System.Drawing.Color.Red;
            totalExceptions_textBox.Location = new System.Drawing.Point(3, 5);
            totalExceptions_textBox.Margin = new Padding(3, 5, 3, 5);
            totalExceptions_textBox.Name = "totalExceptions_textBox";
            totalExceptions_textBox.Size = new System.Drawing.Size(206, 45);
            totalExceptions_textBox.TabIndex = 1;
            totalExceptions_textBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            totalExceptions_textBox.Click += totalExceptions_textBox_Click;
            // 
            // mainUITimer
            // 
            mainUITimer.Tick += mainUITimer_Tick;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Dock = DockStyle.Fill;
            label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label8.Location = new System.Drawing.Point(273, 156);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(264, 21);
            label8.TabIndex = 20;
            label8.Text = "Elapsed Time";
            // 
            // elapsedTime_textBox
            // 
            elapsedTime_textBox.BackColor = System.Drawing.Color.Black;
            elapsedTime_textBox.Dock = DockStyle.Fill;
            elapsedTime_textBox.Font = new System.Drawing.Font("Courier New", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            elapsedTime_textBox.ForeColor = System.Drawing.Color.Lime;
            elapsedTime_textBox.Location = new System.Drawing.Point(273, 182);
            elapsedTime_textBox.Margin = new Padding(3, 5, 3, 5);
            elapsedTime_textBox.Name = "elapsedTime_textBox";
            elapsedTime_textBox.Size = new System.Drawing.Size(264, 45);
            elapsedTime_textBox.TabIndex = 10;
            elapsedTime_textBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // perfCounterTimer
            // 
            perfCounterTimer.Interval = 2500;
            // 
            // database_button
            // 
            database_button.Dock = DockStyle.Fill;
            database_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            database_button.Location = new System.Drawing.Point(3, 106);
            database_button.Margin = new Padding(3, 5, 3, 5);
            database_button.Name = "database_button";
            database_button.Size = new System.Drawing.Size(264, 45);
            database_button.TabIndex = 1;
            database_button.Text = "Database";
            database_button.UseVisualStyleBackColor = true;
            database_button.Click += database_button_Click;
            // 
            // iterationsSecond_textBox
            // 
            iterationsSecond_textBox.BackColor = System.Drawing.Color.Black;
            iterationsSecond_textBox.BorderStyle = BorderStyle.Fixed3D;
            iterationsSecond_textBox.Dock = DockStyle.Fill;
            iterationsSecond_textBox.Font = new System.Drawing.Font("Courier New", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            iterationsSecond_textBox.ForeColor = System.Drawing.Color.Lime;
            iterationsSecond_textBox.Location = new System.Drawing.Point(273, 258);
            iterationsSecond_textBox.Margin = new Padding(3, 5, 3, 5);
            iterationsSecond_textBox.Name = "iterationsSecond_textBox";
            iterationsSecond_textBox.Size = new System.Drawing.Size(264, 45);
            iterationsSecond_textBox.TabIndex = 11;
            iterationsSecond_textBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // activeThreads_textBox
            // 
            activeThreads_textBox.BackColor = System.Drawing.Color.Black;
            activeThreads_textBox.BorderStyle = BorderStyle.Fixed3D;
            activeThreads_textBox.Dock = DockStyle.Fill;
            activeThreads_textBox.Font = new System.Drawing.Font("Courier New", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            activeThreads_textBox.ForeColor = System.Drawing.Color.Lime;
            activeThreads_textBox.Location = new System.Drawing.Point(273, 562);
            activeThreads_textBox.Margin = new Padding(3, 5, 3, 5);
            activeThreads_textBox.Name = "activeThreads_textBox";
            activeThreads_textBox.Size = new System.Drawing.Size(264, 46);
            activeThreads_textBox.TabIndex = 30;
            activeThreads_textBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // activeThreads_label
            // 
            activeThreads_label.AutoSize = true;
            activeThreads_label.Dock = DockStyle.Fill;
            activeThreads_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            activeThreads_label.Location = new System.Drawing.Point(273, 536);
            activeThreads_label.Name = "activeThreads_label";
            activeThreads_label.Size = new System.Drawing.Size(264, 21);
            activeThreads_label.TabIndex = 29;
            activeThreads_label.Text = "Active Threads";
            // 
            // exceptions_button
            // 
            exceptions_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            exceptions_button.Location = new System.Drawing.Point(215, 5);
            exceptions_button.Margin = new Padding(3, 5, 3, 5);
            exceptions_button.Name = "exceptions_button";
            exceptions_button.Size = new System.Drawing.Size(35, 35);
            exceptions_button.TabIndex = 1;
            exceptions_button.Text = "...";
            exceptions_button.UseVisualStyleBackColor = true;
            exceptions_button.Click += exceptions_button_Click;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Dock = DockStyle.Fill;
            label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label9.Location = new System.Drawing.Point(3, 460);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(264, 21);
            label9.TabIndex = 26;
            label9.Text = "CPU Seconds/Iteration (Avg)";
            // 
            // cpuTime_textBox
            // 
            cpuTime_textBox.BackColor = System.Drawing.Color.Black;
            cpuTime_textBox.Dock = DockStyle.Fill;
            cpuTime_textBox.Font = new System.Drawing.Font("Courier New", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            cpuTime_textBox.ForeColor = System.Drawing.Color.Lime;
            cpuTime_textBox.Location = new System.Drawing.Point(3, 486);
            cpuTime_textBox.Margin = new Padding(3, 5, 3, 5);
            cpuTime_textBox.Name = "cpuTime_textBox";
            cpuTime_textBox.Size = new System.Drawing.Size(264, 45);
            cpuTime_textBox.TabIndex = 6;
            cpuTime_textBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Dock = DockStyle.Fill;
            label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label12.Location = new System.Drawing.Point(273, 460);
            label12.Name = "label12";
            label12.Size = new System.Drawing.Size(264, 21);
            label12.TabIndex = 30;
            label12.Text = "Logical Reads/Iteration (Avg)";
            // 
            // logicalReads_textBox
            // 
            logicalReads_textBox.BackColor = System.Drawing.Color.Black;
            logicalReads_textBox.Dock = DockStyle.Fill;
            logicalReads_textBox.Font = new System.Drawing.Font("Courier New", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            logicalReads_textBox.ForeColor = System.Drawing.Color.Lime;
            logicalReads_textBox.Location = new System.Drawing.Point(273, 486);
            logicalReads_textBox.Margin = new Padding(3, 5, 3, 5);
            logicalReads_textBox.Name = "logicalReads_textBox";
            logicalReads_textBox.Size = new System.Drawing.Size(264, 45);
            logicalReads_textBox.TabIndex = 14;
            logicalReads_textBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // db_label
            // 
            db_label.AutoSize = true;
            db_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            db_label.Location = new System.Drawing.Point(73, 49);
            db_label.Name = "db_label";
            db_label.Size = new System.Drawing.Size(0, 17);
            db_label.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 270F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 270F));
            tableLayoutPanel1.Controls.Add(logicalReads_textBox, 1, 12);
            tableLayoutPanel1.Controls.Add(activeThreads_label, 1, 13);
            tableLayoutPanel1.Controls.Add(activeThreads_textBox, 1, 14);
            tableLayoutPanel1.Controls.Add(label12, 1, 11);
            tableLayoutPanel1.Controls.Add(label2, 0, 5);
            tableLayoutPanel1.Controls.Add(iterations_numericUpDown, 0, 6);
            tableLayoutPanel1.Controls.Add(threads_numericUpDown, 0, 8);
            tableLayoutPanel1.Controls.Add(label3, 0, 7);
            tableLayoutPanel1.Controls.Add(database_button, 0, 2);
            tableLayoutPanel1.Controls.Add(label9, 0, 11);
            tableLayoutPanel1.Controls.Add(label10, 0, 13);
            tableLayoutPanel1.Controls.Add(cpuTime_textBox, 0, 12);
            tableLayoutPanel1.Controls.Add(flowLayoutPanel1, 1, 10);
            tableLayoutPanel1.Controls.Add(actualSeconds_textBox, 0, 14);
            tableLayoutPanel1.Controls.Add(label7, 1, 9);
            tableLayoutPanel1.Controls.Add(avgSeconds_textBox, 1, 8);
            tableLayoutPanel1.Controls.Add(label5, 1, 7);
            tableLayoutPanel1.Controls.Add(iterationsSecond_textBox, 1, 6);
            tableLayoutPanel1.Controls.Add(label4, 1, 5);
            tableLayoutPanel1.Controls.Add(label6, 1, 1);
            tableLayoutPanel1.Controls.Add(label8, 1, 3);
            tableLayoutPanel1.Controls.Add(progressBar1, 1, 2);
            tableLayoutPanel1.Controls.Add(elapsedTime_textBox, 1, 4);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 1, 0);
            tableLayoutPanel1.Controls.Add(param_button, 0, 4);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel4, 0, 0);
            tableLayoutPanel1.Controls.Add(label11, 0, 9);
            tableLayoutPanel1.Controls.Add(queryDelay_numericUpDown, 0, 10);
            tableLayoutPanel1.Location = new System.Drawing.Point(448, 5);
            tableLayoutPanel1.Margin = new Padding(3, 5, 3, 5);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 15;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 21F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 55F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 21F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 55F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 21F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 55F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 21F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 55F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 21F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 55F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 21F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 55F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 21F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new System.Drawing.Size(538, 613);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Dock = DockStyle.Fill;
            label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label10.Location = new System.Drawing.Point(3, 536);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(264, 21);
            label10.TabIndex = 28;
            label10.Text = "Actual Seconds/Iteration (Avg)";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(totalExceptions_textBox);
            flowLayoutPanel1.Controls.Add(exceptions_button);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new System.Drawing.Point(273, 410);
            flowLayoutPanel1.Margin = new Padding(3, 5, 3, 5);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new System.Drawing.Size(264, 45);
            flowLayoutPanel1.TabIndex = 13;
            // 
            // actualSeconds_textBox
            // 
            actualSeconds_textBox.BackColor = System.Drawing.Color.Black;
            actualSeconds_textBox.Dock = DockStyle.Fill;
            actualSeconds_textBox.Font = new System.Drawing.Font("Courier New", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            actualSeconds_textBox.ForeColor = System.Drawing.Color.Lime;
            actualSeconds_textBox.Location = new System.Drawing.Point(3, 562);
            actualSeconds_textBox.Margin = new Padding(3, 5, 3, 5);
            actualSeconds_textBox.Name = "actualSeconds_textBox";
            actualSeconds_textBox.Size = new System.Drawing.Size(264, 46);
            actualSeconds_textBox.TabIndex = 7;
            actualSeconds_textBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(cancel_button, 1, 0);
            tableLayoutPanel2.Controls.Add(go_button, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new System.Drawing.Point(273, 5);
            tableLayoutPanel2.Margin = new Padding(3, 5, 3, 5);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new System.Drawing.Size(264, 70);
            tableLayoutPanel2.TabIndex = 8;
            // 
            // param_button
            // 
            param_button.Dock = DockStyle.Fill;
            param_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            param_button.Location = new System.Drawing.Point(3, 182);
            param_button.Margin = new Padding(3, 5, 3, 5);
            param_button.Name = "param_button";
            param_button.Size = new System.Drawing.Size(264, 45);
            param_button.TabIndex = 2;
            param_button.Text = "Parameter Substitution";
            param_button.UseVisualStyleBackColor = true;
            param_button.Click += param_button_Click;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 2;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.Controls.Add(btnFreeCache, 1, 0);
            tableLayoutPanel4.Controls.Add(btnCleanBuffer, 0, 0);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new System.Drawing.Point(3, 5);
            tableLayoutPanel4.Margin = new Padding(3, 5, 3, 5);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 2;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.Size = new System.Drawing.Size(264, 70);
            tableLayoutPanel4.TabIndex = 0;
            // 
            // btnFreeCache
            // 
            btnFreeCache.Dock = DockStyle.Fill;
            btnFreeCache.Location = new System.Drawing.Point(134, 3);
            btnFreeCache.Margin = new Padding(2, 3, 2, 3);
            btnFreeCache.Name = "btnFreeCache";
            btnFreeCache.Size = new System.Drawing.Size(128, 29);
            btnFreeCache.TabIndex = 1;
            btnFreeCache.Text = "Free Cache";
            btnFreeCache.UseVisualStyleBackColor = true;
            btnFreeCache.Click += btnFreeCache_Click;
            // 
            // btnCleanBuffer
            // 
            btnCleanBuffer.Dock = DockStyle.Fill;
            btnCleanBuffer.Location = new System.Drawing.Point(2, 3);
            btnCleanBuffer.Margin = new Padding(2, 3, 2, 3);
            btnCleanBuffer.Name = "btnCleanBuffer";
            btnCleanBuffer.Size = new System.Drawing.Size(128, 29);
            btnCleanBuffer.TabIndex = 0;
            btnCleanBuffer.Text = "Clean Buffers";
            btnCleanBuffer.UseVisualStyleBackColor = true;
            btnCleanBuffer.Click += btnCleanBuffer_Click;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Dock = DockStyle.Fill;
            label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label11.Location = new System.Drawing.Point(3, 384);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(264, 21);
            label11.TabIndex = 34;
            label11.Text = "Delay between queries (ms)";
            // 
            // queryDelay_numericUpDown
            // 
            queryDelay_numericUpDown.Dock = DockStyle.Fill;
            queryDelay_numericUpDown.Location = new System.Drawing.Point(3, 410);
            queryDelay_numericUpDown.Margin = new Padding(3, 5, 3, 5);
            queryDelay_numericUpDown.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            queryDelay_numericUpDown.Name = "queryDelay_numericUpDown";
            queryDelay_numericUpDown.Size = new System.Drawing.Size(264, 27);
            queryDelay_numericUpDown.TabIndex = 5;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 2;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel3.Controls.Add(tableLayoutPanel1, 1, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new System.Drawing.Point(0, 30);
            tableLayoutPanel3.Margin = new Padding(3, 5, 3, 5);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Size = new System.Drawing.Size(989, 638);
            tableLayoutPanel3.TabIndex = 2;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new System.Drawing.Size(1300, 668);
            Controls.Add(tableLayoutPanel3);
            Controls.Add(db_label);
            Controls.Add(label1);
            Controls.Add(menuStrip1);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Margin = new Padding(3, 5, 3, 5);
            MinimumSize = new System.Drawing.Size(988, 695);
            Name = "FormMain";
            Text = "SQLQueryStress";
            Load += FormMain_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)iterations_numericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)threads_numericUpDown).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)queryDelay_numericUpDown).EndInit();
            tableLayoutPanel3.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
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

