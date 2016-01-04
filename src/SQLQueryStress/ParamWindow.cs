using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace SQLQueryStress
{
    public partial class ParamWindow : Form
    {
        //load query defined in the main form
        private string outerQuery;

        //Variables from the load query
        private string[] queryVariables;

        //parameter values from the parameter query defined in this form
        private Dictionary<string, string> paramValues = new Dictionary<string,string>();

        //Query Stress Settings
        private Form1.QueryStressSettings settings;

        public ParamWindow( Form1.QueryStressSettings settings,
                            string outerQuery)
        {
            InitializeComponent();

            this.settings = settings;

            this.outerQuery = outerQuery;

            this.paramQueryTextBox.Text = (string)settings.paramQuery.Clone();

            columnMapGrid.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            columnMapGrid.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            columnMapGrid.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;

            //TODO: Which event to handle?!?!
            columnMapGrid.CellEndEdit += new DataGridViewCellEventHandler(columnMapGrid_CellValueChanged);

            if ((outerQuery.Length > 0) && (paramQueryTextBox.Text.Length > 0))
            {
                this.getColumnsButton_Click("constructor", null);
            }
        }

        private void columnMapGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //handle changes to the parameter column
            if (e.ColumnIndex == 2)
            {
                DataGridViewRow theRow = columnMapGrid.Rows[e.RowIndex];
                DataGridViewComboBoxCell combo = (DataGridViewComboBoxCell)(theRow.Cells[2]);

                if (combo.Value != null)
                {
                    string colType = paramValues[(string)combo.Value];
                    theRow.Cells[1].Value = colType;
                }
                else
                {
                    theRow.Cells[1].Value = "";
                }
            }
        }

        private string[] getParams()
        {
            //Find all SQL variables:
            //'@', preceeded by '=', ',', or any white space character
            //then, any "word" character
            //Finally, '=', ',', or any white space, repeated 0 or more times 
            //(in the case of end-of-string, will be 0 times)
            Regex r = new Regex(@"(?<=[=,\s\(])@\w{1,}(?=[=,\s\)]?)");

            List<string> output = new List<string>();

            foreach (Match m in r.Matches(outerQuery))
            {
                string lowerVal = m.Value.ToLower();
                if (!output.Contains(lowerVal))
                    output.Add(m.Value.ToLower());
            }

            if (output.Count == 0)
                MessageBox.Show("No variables were identified in the main query. Variables must be used there before parameter substitution can be used.");

            return (output.ToArray());
        }

        private void getColumnsButton_Click(object sender, EventArgs e)
        {
            this.queryVariables = this.getParams();

            SqlDataReader reader = null;

            DatabaseSelect.ConnectionInfo DBInfo = settings.shareDBSettings ? settings.mainDBConnectionInfo : settings.paramDBConnectionInfo;

            if (!DBInfo.TestConnection())
            {
                MessageBox.Show("You must set valid database connection information. Click the Database button to configure the settings.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(DBInfo.ConnectionString))
            {
                try
                {
                    SqlCommand comm = new SqlCommand(paramQueryTextBox.Text, conn);
                    conn.Open();
                    reader = comm.ExecuteReader(CommandBehavior.SchemaOnly);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                if (reader != null)
                {
                    columnMapGrid.Rows.Clear();
                    paramValues.Clear();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        paramValues.Add(reader.GetName(i), reader.GetDataTypeName(i));
                    }

                    reader.Dispose();

                    foreach (string variable in queryVariables)
                    {
                        int colOrdinal = columnMapGrid.Rows.Add();
                        DataGridViewRow row = columnMapGrid.Rows[colOrdinal];
                        row.Cells[0].Value = variable;
                        row.Cells[0].ReadOnly = true;

                        //placeholder for columntype
                        row.Cells[1].Value = "";
                        row.Cells[1].ReadOnly = true;

                        DataGridViewComboBoxCell combo = new DataGridViewComboBoxCell();

                        combo.Items.Add("");

                        bool checkParam = false;
                        if ((sender.GetType() == typeof(System.String)) && 
                            ((string)sender == "constructor") &&
                            settings.paramMappings.ContainsKey(variable))
                            checkParam = true;

                        foreach (string paramName in paramValues.Keys)
                        {
                            combo.Items.Add(paramName);

                            if (checkParam)
                            {
                                if (settings.paramMappings[variable] == paramName)
                                {
                                    combo.Value = paramName;
                                    row.Cells[1].Value = paramValues[paramName];
                                }
                            }
                        }

                        row.Cells[2] = combo;
                    }
                }
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void database_button_Click(object sender, EventArgs e)
        {
            DatabaseSelect dbSelect = new DatabaseSelect(settings);
            dbSelect.StartPosition = FormStartPosition.CenterParent;
            dbSelect.ShowDialog();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            settings.paramQuery = this.paramQueryTextBox.Text;

            Dictionary<string, string> localParamMappings = new Dictionary<string, string>();
            foreach (DataGridViewRow row in this.columnMapGrid.Rows)
            {
                if ((string)row.Cells[2].Value != "")
                    localParamMappings.Add((string)row.Cells[0].Value, (string)row.Cells[2].Value);
            }

            settings.paramMappings = localParamMappings;

            this.Dispose();
        }
    }
}