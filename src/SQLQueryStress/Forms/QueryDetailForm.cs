using Microsoft.SqlServer.XEvent.XELite;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SQLQueryStress.Forms
{
    public partial class QueryDetailForm : Form
    {
        private readonly LoadEngine.QueryOutput _queryOutput;
        private readonly ConcurrentDictionary<Guid, List<IXEvent>> _events;

        public QueryDetailForm(LoadEngine.QueryOutput queryOutput, ConcurrentDictionary<Guid, List<IXEvent>> events)
        {
            _events = events;
            _queryOutput = queryOutput;

            InitializeComponent();
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

            if (!_events.TryGetValue(_queryOutput.context, out var contextEvents))
            {
                detailsTextBox.Text += "No ExEvents Found!";
                return;
            }



            /*
            if (_queryOutput.exception != null)
            {
                detailsTextBox.Text += $"\r\nException:\r\n{_queryOutput.exception}";
            }
            */
        }

        private void InitializeComponent()
        {
            dataGridView1 = new DataGridView();
            detailsTextBox = new TextBox();
            splitContainer1 = new SplitContainer();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new System.Drawing.Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.Size = new System.Drawing.Size(800, 296);
            dataGridView1.TabIndex = 0;
            // 
            // detailsTextBox
            // 
            detailsTextBox.Dock = DockStyle.Fill;
            detailsTextBox.Location = new System.Drawing.Point(0, 0);
            detailsTextBox.Multiline = true;
            detailsTextBox.Name = "detailsTextBox";
            detailsTextBox.ReadOnly = true;
            detailsTextBox.ScrollBars = ScrollBars.Both;
            detailsTextBox.Size = new System.Drawing.Size(800, 300);
            detailsTextBox.TabIndex = 0;
            detailsTextBox.TextChanged += detailsTextBox_TextChanged;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new System.Drawing.Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(detailsTextBox);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(dataGridView1);
            splitContainer1.Size = new System.Drawing.Size(800, 600);
            splitContainer1.SplitterDistance = 300;
            splitContainer1.TabIndex = 0;
            // 
            // QueryDetailForm
            // 
            ClientSize = new System.Drawing.Size(800, 600);
            Controls.Add(splitContainer1);
            Name = "QueryDetailForm";
            StartPosition = FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox detailsTextBox;
        private System.Windows.Forms.SplitContainer splitContainer1;

        private void detailsTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
} 