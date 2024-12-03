using System;
using System.Windows.Forms;

namespace SQLQueryStress.Forms
{
    public partial class QueryDetailForm : Form
    {
        private readonly LoadEngine.QueryOutput _queryOutput;

        public QueryDetailForm(LoadEngine.QueryOutput queryOutput)
        {
            InitializeComponent();
            _queryOutput = queryOutput;
            this.Text = $"Query Details - {_queryOutput.startTime:HH:mm:ss.fff}";
            LoadData();
        }

        private void LoadData()
        {
            // Set up the DataGridView
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = new();

            // Add query details
            detailsTextBox.Text = $"Start Time: {_queryOutput.startTime:HH:mm:ss.fff}\r\n" +
                              //   $"Duration: {_queryOutput.iterationTime:F3}ms\r\n" +
                                 $"Context: {_queryOutput.context}\r\n";
            /*
            if (_queryOutput.exception != null)
            {
                detailsTextBox.Text += $"\r\nException:\r\n{_queryOutput.exception}";
            }
            */
        }

        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.detailsTextBox = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            
            // splitContainer1
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            
            // detailsTextBox
            this.detailsTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.detailsTextBox.Multiline = true;
            this.detailsTextBox.ReadOnly = true;
            this.detailsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            
            // dataGridView1
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ReadOnly = true;
            
            // Add controls to split container
            this.splitContainer1.Panel1.Controls.Add(this.detailsTextBox);
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView1);
            
            // Form settings
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.splitContainer1);
            this.Name = "QueryDetailForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox detailsTextBox;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
} 