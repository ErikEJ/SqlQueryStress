using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SQLQueryStress
{
    public partial class DataViewer : Form
    {
        private DataTable dataView;

        public DataTable DataView
        {
            get
            {
                return(this.dataView);
            }

            set
            {
                this.dataView = value;
            }
        }

        public DataViewer()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {          
            dataGridView1.DataSource = this.dataView;

            int columnWidth = (dataGridView1.Width-41) / (this.dataView.Columns.Count);

            foreach (DataGridViewColumn col in dataGridView1.Columns)
                col.Width = columnWidth;
        }

        public void Repaint()
        {
            dataGridView1.Invalidate();
        }
    }
}