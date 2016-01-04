using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SQLQueryStress
{
    public partial class DatabaseSelect : Form
    {
        private Form1.QueryStressSettings settings;
        private ConnectionInfo localMainConnectionInfo;
        private ConnectionInfo localParamConnectionInfo;

        public DatabaseSelect(Form1.QueryStressSettings settings)
        {
            this.settings = settings;
            this.localMainConnectionInfo = (ConnectionInfo)settings.mainDBConnectionInfo.Clone();

            if (settings.shareDBSettings)
            {
                this.localParamConnectionInfo = (ConnectionInfo)settings.mainDBConnectionInfo.Clone();
            }
            else
            {
                this.localParamConnectionInfo = (ConnectionInfo)settings.paramDBConnectionInfo.Clone();
            }

            InitializeComponent();

            this.server_textBox.Text = localMainConnectionInfo.Server;
            
            if (localMainConnectionInfo.IntegratedAuth)
            {
                this.authentication_comboBox.SelectedIndex = 0;
                this.login_textBox.Enabled = false;
                this.password_textBox.Enabled = false;
            }
            else
            {
                this.authentication_comboBox.SelectedIndex = 1;
                this.login_textBox.Text = localMainConnectionInfo.Login;
                this.password_textBox.Text = localMainConnectionInfo.Password;
            }

            if (localMainConnectionInfo.Database.Length > 0)
            {
                this.db_comboBox.Items.Add(localMainConnectionInfo.Database);
                this.db_comboBox.SelectedIndex = 0;
            }

            if (!settings.shareDBSettings)
            {
                this.pm_server_textBox.Text = localParamConnectionInfo.Server;

                if (localParamConnectionInfo.IntegratedAuth)
                {
                    this.pm_authentication_comboBox.SelectedIndex = 0;
                    this.pm_login_textBox.Enabled = false;
                    this.pm_password_textBox.Enabled = false;
                }
                else
                {
                    this.pm_authentication_comboBox.SelectedIndex = 1;
                    this.pm_login_textBox.Text = localParamConnectionInfo.Login;
                    this.pm_password_textBox.Text = localParamConnectionInfo.Password;
                }

                if (localParamConnectionInfo.Database.Length > 0)
                {
                    this.pm_db_comboBox.Items.Add(localParamConnectionInfo.Database);
                    this.pm_db_comboBox.SelectedIndex = 0;
                }
            }
            else
                this.pm_authentication_comboBox.SelectedIndex = 0;

            this.shareSettings_checkBox.Checked = settings.shareDBSettings;

            this.authentication_comboBox.SelectedIndexChanged += new EventHandler(authentication_comboBox_SelectedIndexChanged);
            this.pm_authentication_comboBox.SelectedIndexChanged += new EventHandler(pm_authentication_comboBox_SelectedIndexChanged);

            this.db_comboBox.Click += new EventHandler(db_comboBox_Click);
            this.pm_db_comboBox.Click += new EventHandler(pm_db_comboBox_Click);
        }

        private void authentication_comboBox_SelectedIndexChanged(object sender,
            System.EventArgs e)
        {
            if (authentication_comboBox.SelectedIndex == 0)
            {
                login_textBox.Enabled = false;
                password_textBox.Enabled = false;
            }
            else
            {
                login_textBox.Enabled = true;
                password_textBox.Enabled = true;
            }
        }

        private void pm_authentication_comboBox_SelectedIndexChanged(object sender,
            System.EventArgs e)
        {
            if (pm_authentication_comboBox.SelectedIndex == 0)
            {
                pm_login_textBox.Enabled = false;
                pm_password_textBox.Enabled = false;
            }
            else
            {
                pm_login_textBox.Enabled = true;
                pm_password_textBox.Enabled = true;
            }
        }

        private void db_comboBox_Click(object sender, System.EventArgs e)
        {
            saveLocalSettings();

            string selectedItem = (string)db_comboBox.SelectedItem;

            string sql = "" +
                "SELECT name " +
                "FROM master..sysdatabases " +
                "ORDER BY name";

            using (SqlConnection conn = new SqlConnection(this.localMainConnectionInfo.ConnectionString))
            {
                SqlCommand comm = new SqlCommand(sql, conn);

                List<string> databases = new List<string>();

                try
                {
                    conn.Open();

                    SqlDataReader reader = comm.ExecuteReader();

                    while (reader.Read())
                    {
                        databases.Add((string)reader[0]); 
                    }
                }
                catch (SqlException ex)
                {
                    if (ex.Number != 4060)
                        MessageBox.Show("Connection Failed");
                    else
                    {
                        //Clear the db, try again
                        this.db_comboBox.Items.Clear();
                        this.db_comboBox_Click(null, null);
                        return;
                    }
                }

                db_comboBox.DataSource = databases.ToArray();

                if (selectedItem != null)
                    if (db_comboBox.Items.Contains(selectedItem))
                        db_comboBox.SelectedItem = selectedItem;
            }
        }

        private void pm_db_comboBox_Click(object sender, System.EventArgs e)
        {
            pm_saveLocalSettings();

            string selectedItem = (string)pm_db_comboBox.SelectedItem;

            string sql = "" +
                "SELECT name " +
                "FROM master..sysdatabases " +
                "ORDER BY name";

            using (SqlConnection conn = new SqlConnection(this.localParamConnectionInfo.ConnectionString))
            {
                SqlCommand comm = new SqlCommand(sql, conn);

                List<string> databases = new List<string>();

                try
                {
                    conn.Open();

                    SqlDataReader reader = comm.ExecuteReader();

                    while (reader.Read())
                        databases.Add((string)reader[0]);
                }
                catch (SqlException ex)
                {
                    if (ex.Number != 4060)
                        MessageBox.Show("Connection Failed");
                    else
                    {
                        //Clear the db, try again
                        this.pm_db_comboBox.Items.Clear();
                        this.pm_db_comboBox_Click(null, null);
                        return;
                    }
                }

                pm_db_comboBox.DataSource = databases.ToArray();

                if (selectedItem != null)
                    if (pm_db_comboBox.Items.Contains(selectedItem))
                        pm_db_comboBox.SelectedItem = selectedItem;
            }
        }

        [Serializable]
        public class ConnectionInfo : ICloneable
        {
            public ConnectionInfo()
            {
                Server = "";
                IntegratedAuth = true;
                Login = "";
                Password = "";
                Database = "";
            }

            public ConnectionInfo(Form1.QueryStressSettings settings)
            {
                Server = "";
                IntegratedAuth = true;
                Login = "";
                Password = "";
                Database = "";
                this.settings = settings;
            }

            public string Server;
            public bool IntegratedAuth;
            public string Login;
            public string Password;
            public string Database;
            public Form1.QueryStressSettings settings;

            public string ConnectionString
            {
                get
                {
                    SqlConnectionStringBuilder build = new SqlConnectionStringBuilder();
                    build.DataSource = this.Server;
                    build.IntegratedSecurity = this.IntegratedAuth;
                    if (!this.IntegratedAuth)
                    {
                        build.UserID = this.Login;
                        build.Password = this.Password;
                    }

                    if (this.Database.Length > 0)
                        build.InitialCatalog = this.Database;

                    if (this.settings != null)
                    {
                        build.ConnectTimeout = this.settings.connectionTimeout;
                        build.Pooling = this.settings.enableConnectionPooling;
                        build.MaxPoolSize = this.settings.numThreads * 2;
                    }

                    return(build.ConnectionString);
                }
            }

            public bool TestConnection()
            {
                if ((this.Server == "") ||
                    ((this.IntegratedAuth == false) && ((this.Login == "" || this.Password == ""))))
                    return (false);

                using (SqlConnection conn = new SqlConnection(this.ConnectionString))
                {
                    try
                    {
                        conn.Open();
                    }
                    catch
                    {
                        return (false);
                    }
                }

                return (true);
            }

            public void CopyTo(ConnectionInfo to)
            {
                to.Server = this.Server;
                to.IntegratedAuth = this.IntegratedAuth;
                to.Login = this.Login;
                to.Password = this.Password;
                to.Database = this.Database;
            }

            #region ICloneable Members

            public object Clone()
            {
                ConnectionInfo newConnInfo = new ConnectionInfo();
                this.CopyTo(newConnInfo);
 
                return (newConnInfo);
            }

            #endregion
        }

        private void saveLocalSettings()
        {
            localMainConnectionInfo.Server = server_textBox.Text;
            localMainConnectionInfo.IntegratedAuth = (authentication_comboBox.SelectedIndex == 0) ? true : false;

            if (localMainConnectionInfo.IntegratedAuth)
            {
                localMainConnectionInfo.Login = "";
                localMainConnectionInfo.Password = "";
            }
            else
            {
                localMainConnectionInfo.Login = login_textBox.Text;
                localMainConnectionInfo.Password = password_textBox.Text;
            }

            if (db_comboBox.SelectedItem != null)
                localMainConnectionInfo.Database = db_comboBox.SelectedItem.ToString();
            else
                localMainConnectionInfo.Database = "";
        }

        private void pm_saveLocalSettings()
        {
            if (!shareSettings_checkBox.Checked)
            {
                localParamConnectionInfo.Server = pm_server_textBox.Text;
                localParamConnectionInfo.IntegratedAuth = (pm_authentication_comboBox.SelectedIndex == 0) ? true : false;

                if (localParamConnectionInfo.IntegratedAuth)
                {
                    localParamConnectionInfo.Login = "";
                    localParamConnectionInfo.Password = "";
                }
                else
                {
                    localParamConnectionInfo.Login = pm_login_textBox.Text;
                    localParamConnectionInfo.Password = pm_password_textBox.Text;
                }

                if (pm_db_comboBox.SelectedItem != null)
                    localParamConnectionInfo.Database = pm_db_comboBox.SelectedItem.ToString();
                else
                    localParamConnectionInfo.Database = "";
            }
            else
                localParamConnectionInfo = new ConnectionInfo();
        }

        private void test_button_Click(object sender, EventArgs e)
        {
            saveLocalSettings();

            if (localMainConnectionInfo.TestConnection())
            {
                MessageBox.Show("Connection Succeeded");
            }
            else
            {
                MessageBox.Show("Connection Failed");
            }
        }

        private void pm_test_button_Click(object sender, EventArgs e)
        {
            pm_saveLocalSettings();

            if (localParamConnectionInfo.TestConnection())
            {
                MessageBox.Show("Connection Succeeded");
            }
            else
            {
                MessageBox.Show("Connection Failed");
            }
        }

        private void cancel_button_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void ok_button_Click(object sender, EventArgs e)
        {
            saveLocalSettings();
            pm_saveLocalSettings();

            localMainConnectionInfo.CopyTo(settings.mainDBConnectionInfo);
            localParamConnectionInfo.CopyTo(settings.paramDBConnectionInfo);
            settings.shareDBSettings = shareSettings_checkBox.Checked;

            this.Dispose();            
        }

        private void shareSettings_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (shareSettings_checkBox.Checked)
            {
                pm_server_textBox.Enabled = false;
                pm_authentication_comboBox.Enabled = false;
                pm_login_textBox.Enabled = false;
                pm_password_textBox.Enabled = false;
                pm_db_comboBox.Enabled = false;
                pm_test_button.Enabled = false;
            }
            else
            {
                pm_server_textBox.Enabled = true;
                pm_authentication_comboBox.Enabled = true;

                if (pm_authentication_comboBox.SelectedIndex == 1)
                {
                    pm_login_textBox.Enabled = true;
                    pm_password_textBox.Enabled = true;
                }
                pm_db_comboBox.Enabled = true;
                pm_test_button.Enabled = true;
            }
        }
    }
}