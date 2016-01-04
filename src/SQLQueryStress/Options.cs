using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SQLQueryStress
{
    public partial class Options : Form
    {
        private Form1.QueryStressSettings settings;

        public Options(Form1.QueryStressSettings settings)
        {
            this.settings = settings;

            InitializeComponent();

            connectionTimeout_numericUpDown.Value = settings.connectionTimeout;
            commandTimeout_numericUpDown.Value = settings.commandTimeout;
            connectionPooling_checkBox.Checked = settings.enableConnectionPooling;
            IOStatistics_checkBox.Checked = settings.collectIOStats;
            timeStatistics_checkBox.Checked = settings.collectTimeStats;
            clientDataRetrieval_checkBox.Checked = settings.forceDataRetrieval;
        }

        private void ok_button_Click(object sender, EventArgs e)
        {
            this.settings.connectionTimeout = (int)connectionTimeout_numericUpDown.Value;
            this.settings.commandTimeout = (int)commandTimeout_numericUpDown.Value;
            this.settings.enableConnectionPooling = connectionPooling_checkBox.Checked;
            this.settings.collectIOStats = IOStatistics_checkBox.Checked;
            this.settings.collectTimeStats = timeStatistics_checkBox.Checked;
            this.settings.forceDataRetrieval = clientDataRetrieval_checkBox.Checked;

            this.Dispose();
        }

        private void cancel_button_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}