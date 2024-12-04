using Microsoft.Data.SqlClient;
using SQLQueryStress.Properties;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SQLQueryStress
{
    public partial class DatabaseSelect : Form
    {
        private readonly ConnectionInfo _localMainConnectionInfo;
        private readonly QueryStressSettings _settings;
        private ConnectionInfo _localParamConnectionInfo;

        public DatabaseSelect(QueryStressSettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _localMainConnectionInfo = (ConnectionInfo)settings.MainDbConnectionInfo.Clone();

            if (settings.ShareDbSettings)
            {
                _localParamConnectionInfo = (ConnectionInfo)settings.MainDbConnectionInfo.Clone();
            }
            else
            {
                _localParamConnectionInfo = (ConnectionInfo)settings.ParamDbConnectionInfo.Clone();
            }

            InitializeComponent();

            server_textBox.Text = _localMainConnectionInfo.Server;
            trustcert_check.Checked = _localMainConnectionInfo.TrustServerCertificate;
            additionalParameters_textBox.Text = _localMainConnectionInfo.AdditionalParameters;

            if (SqlConnectionEncryptOption.TryParse(_localMainConnectionInfo.EncryptOption, out SqlConnectionEncryptOption encrypt))
            {
                encrypt_Combo.SelectedItem = encrypt.ToString();
            }

            if (!_localMainConnectionInfo.RequiresPassword)
            {
                authentication_comboBox.SelectedIndex = _localMainConnectionInfo.IntegratedAuth ? 0 : 2;
                login_textBox.Enabled = false;
                password_textBox.Enabled = false;
            }
            else
            {
                authentication_comboBox.SelectedIndex = 1;
                login_textBox.Text = _localMainConnectionInfo.Login;
                password_textBox.Text = _localMainConnectionInfo.Password;
            }

            if (_localMainConnectionInfo.Database.Length > 0)
            {
                db_comboBox.Items.Add(_localMainConnectionInfo.Database);
                db_comboBox.SelectedIndex = 0;
            }

            if (_localMainConnectionInfo.ApplicationIntent > 0)
            {
                appintent_check.Checked = true;
                appintent_combo.SelectedItem = _localMainConnectionInfo.ApplicationIntent;
            }

            if (!settings.ShareDbSettings)
            {
                pm_server_textBox.Text = _localParamConnectionInfo.Server;
                pm_additionalParameters_textBox.Text = _localParamConnectionInfo.AdditionalParameters;
                pm_encrypt_Combo.SelectedText = _localParamConnectionInfo.EncryptOption;
                pm_trustcert_check.Checked = _localParamConnectionInfo.TrustServerCertificate;

                if (!_localParamConnectionInfo.RequiresPassword)
                {
                    pm_authentication_comboBox.SelectedIndex = 0;
                    pm_login_textBox.Enabled = false;
                    pm_password_textBox.Enabled = false;
                }
                else
                {
                    pm_authentication_comboBox.SelectedIndex = 1;
                    pm_login_textBox.Text = _localParamConnectionInfo.Login;
                    pm_password_textBox.Text = _localParamConnectionInfo.Password;
                }

                if (_localParamConnectionInfo.Database.Length > 0)
                {
                    pm_db_comboBox.Items.Add(_localParamConnectionInfo.Database);
                    pm_db_comboBox.SelectedIndex = 0;
                }
                if (_localParamConnectionInfo.ApplicationIntent > 0)
                {
                    pm_appintent_check.Checked = true;
                    pm_appintent_combo.SelectedItem = _localParamConnectionInfo.ApplicationIntent;
                }
            }
            else
            {
                pm_authentication_comboBox.SelectedIndex = 0;
            }

            shareSettings_checkBox.Checked = settings.ShareDbSettings;

            authentication_comboBox.SelectedIndexChanged += authentication_comboBox_SelectedIndexChanged;
            pm_authentication_comboBox.SelectedIndexChanged += pm_authentication_comboBox_SelectedIndexChanged;

            db_comboBox.Enter += Db_comboBox_Enter;
            db_comboBox.Leave += Db_comboBox_Leave;

            pm_db_comboBox.Enter += Db_comboBox_Enter;
            pm_db_comboBox.Leave += Db_comboBox_Leave;

            server_textBox.KeyDown += Server_textBox_KeyDown;
            server_textBox.TextChanged += Server_textBox_TextChanged;

            Server_textBox_TextChanged(null, null);
        }

        private void Server_textBox_TextChanged(object sender, EventArgs e)
        {
            ok_button.Enabled = !string.IsNullOrEmpty(server_textBox.Text);
        }

        private void Server_textBox_KeyDown(object sender, KeyEventArgs e)
        {
            db_comboBox.SelectedIndex = -1;
        }

        private void Db_comboBox_Leave(object sender, EventArgs e)
        {
            var cbSender = ((ComboBox)sender);
            if ((cbSender.SelectedValue == null || cbSender.Text != cbSender.SelectedItem.ToString()) && cbSender.Items.Contains(cbSender.Text))
            {
                cbSender.SelectedItem = cbSender.Text;
            }
        }

        private void Db_comboBox_Enter(object sender, EventArgs e)
        {
            var cbSender = ((ComboBox)sender);
            var _prevSelectedValue = cbSender.SelectedValue != null ? cbSender.SelectedValue.ToString() : string.Empty;
            ReloadDatabaseList(sender);

            if (cbSender.Items.Contains(_prevSelectedValue))
            {
                cbSender.SelectedItem = _prevSelectedValue;
            }
        }

        private void ReloadDatabaseList(object objDatabaseParam)
        {
            var dbComboboxParam = (ComboBox)objDatabaseParam;
            var selectedComboBoxItem = (string)dbComboboxParam.SelectedItem;
            string connectionString;
            if (dbComboboxParam == db_comboBox)
            {
                SaveLocalSettings();
                connectionString = _localMainConnectionInfo.ConnectionString;
            }
            else
            {
                pm_saveLocalSettings();
                connectionString = _localParamConnectionInfo.ConnectionString;
            }

            var sql = "SELECT databases.name FROM sys.databases WHERE databases.state = 0 ORDER BY databases.name";

            using var conn = new SqlConnection(connectionString);
            using var sqlCommand = new SqlCommand(sql, conn);

            var databases = new List<string>();

            try
            {
                conn.Open();

                var reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    databases.Add((string)reader[0]);
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 40615)
                    return;
                if (ex.Number == 18456) // login failed. This helps with connecting to Azure databases
                    return;
                if (ex.Number != 4060)
                {
                    MessageBox.Show($"{Resources.ConnFail}{Environment.NewLine}{ex.Message}", Resources.AppTitle);

                    if (dbComboboxParam == db_comboBox)
                    {
                        server_textBox.Focus();
                    }
                    else
                    {
                        pm_server_textBox.Focus();
                    }
                }
                else
                {
                    //Clear the db, try again
                    db_comboBox.Text = string.Empty;
                    pm_db_comboBox.Text = string.Empty;
                    dbComboboxParam.Items.Clear();
                    ReloadDatabaseList(dbComboboxParam);
                    return;
                }
            }

            dbComboboxParam.DataSource = databases.ToArray();

            if (selectedComboBoxItem != null)
            {
                if (dbComboboxParam.Items.Contains(selectedComboBoxItem))
                {
                    dbComboboxParam.SelectedItem = selectedComboBoxItem;
                }
            }
        }

        private void authentication_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (authentication_comboBox.SelectedIndex == 0)
            {
                login_textBox.Enabled = false;
                password_textBox.Enabled = false;
            }
            else if (authentication_comboBox.SelectedIndex == 2)
            {
                login_textBox.Enabled = true;
                password_textBox.Enabled = false;
            }
            else
            {
                login_textBox.Enabled = true;
                password_textBox.Enabled = true;
            }
        }

        private void cancel_button_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void ok_button_Click(object sender, EventArgs e)
        {
            SaveLocalSettings();
            pm_saveLocalSettings();

            _localMainConnectionInfo.CopyTo(_settings.MainDbConnectionInfo);
            _localParamConnectionInfo.CopyTo(_settings.ParamDbConnectionInfo);
            _settings.ShareDbSettings = shareSettings_checkBox.Checked;

            Dispose();
        }

        private void pm_authentication_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pm_authentication_comboBox.SelectedIndex == 0)
            {
                pm_login_textBox.Enabled = false;
                pm_password_textBox.Enabled = false;
            }
            else if (pm_authentication_comboBox.SelectedIndex == 2)
            {
                pm_login_textBox.Enabled = true;
                pm_password_textBox.Enabled = false;
            }
            else
            {
                pm_login_textBox.Enabled = true;
                pm_password_textBox.Enabled = true;
            }
        }

        private void pm_saveLocalSettings()
        {
            if (!shareSettings_checkBox.Checked)
            {
                _localParamConnectionInfo.Server = pm_server_textBox.Text;
                _localParamConnectionInfo.IntegratedAuth = pm_authentication_comboBox.SelectedIndex == 0;
                _localParamConnectionInfo.AzureMFA = pm_authentication_comboBox.SelectedIndex == 2;

                if (SqlConnectionEncryptOption.TryParse(pm_encrypt_Combo.Text, out SqlConnectionEncryptOption encrypt))
                {
                    _localParamConnectionInfo.Encrypt = encrypt;
                }

                _localParamConnectionInfo.TrustServerCertificate = pm_trustcert_check.Checked;

                if (!_localParamConnectionInfo.RequiresPassword && !_localParamConnectionInfo.AzureMFA)
                {
                    _localParamConnectionInfo.Login = string.Empty;
                    _localParamConnectionInfo.Password = string.Empty;
                }
                else if (_localParamConnectionInfo.AzureMFA)
                {
                    _localParamConnectionInfo.Login = pm_login_textBox.Text;
                    _localParamConnectionInfo.Password = string.Empty;
                }
                else
                {
                    _localParamConnectionInfo.Login = pm_login_textBox.Text;
                    _localParamConnectionInfo.Password = pm_password_textBox.Text;
                }

                _localParamConnectionInfo.Database = pm_db_comboBox.Text;

                _localParamConnectionInfo.AdditionalParameters = additionalParameters_textBox.Text;

                if (pm_appintent_check.Checked)
                {

                    _ = Enum.TryParse(pm_appintent_combo.Text, out ApplicationIntent applicationIntent);

                    _localParamConnectionInfo.ApplicationIntent = applicationIntent;
                }
            }
            else
                _localParamConnectionInfo = new ConnectionInfo();
        }

        private void pm_test_button_Click(object sender, EventArgs e)
        {
            pm_saveLocalSettings();

            MessageBox.Show(_localParamConnectionInfo.TestConnection() ? Resources.ConnSucc : Resources.ConnFail, Resources.AppTitle);
        }

        private void SaveLocalSettings()
        {
            _localMainConnectionInfo.Server = server_textBox.Text;
            _localMainConnectionInfo.IntegratedAuth = authentication_comboBox.SelectedIndex == 0;

            if (SqlConnectionEncryptOption.TryParse(encrypt_Combo.Text, out SqlConnectionEncryptOption encrypt))
            {
                _localMainConnectionInfo.Encrypt = encrypt;
            }

            _localMainConnectionInfo.TrustServerCertificate = trustcert_check.Checked;

            _localMainConnectionInfo.AzureMFA = authentication_comboBox.SelectedIndex == 2;

            if (!_localMainConnectionInfo.RequiresPassword && !_localMainConnectionInfo.AzureMFA)
            {
                _localMainConnectionInfo.Login = string.Empty;
                _localMainConnectionInfo.Password = string.Empty;
            }
            else if (_localMainConnectionInfo.AzureMFA)
            {
                _localMainConnectionInfo.Login = login_textBox.Text;
                _localMainConnectionInfo.Password = string.Empty;
            }
            else
            {
                _localMainConnectionInfo.Login = login_textBox.Text;
                _localMainConnectionInfo.Password = password_textBox.Text;
            }

            if (appintent_check.Checked)
            {
                _ = Enum.TryParse(appintent_combo.Text, out ApplicationIntent applicationIntent);

                _localMainConnectionInfo.ApplicationIntent = applicationIntent;
            }

            _localMainConnectionInfo.Database = db_comboBox.Text;

            _localMainConnectionInfo.AdditionalParameters = additionalParameters_textBox.Text;
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

        private void test_button_Click(object sender, EventArgs e)
        {
            SaveLocalSettings();

            MessageBox.Show(_localMainConnectionInfo.TestConnection() ? Resources.ConnSucc : Resources.ConnFail, Resources.AppTitle);
        }


        private void appintent_check_CheckedChanged(object sender, EventArgs e)
        {
            appintent_combo.Enabled = appintent_check.Checked;

            appintent_combo.DataSource = Enum.GetValues(typeof(ApplicationIntent));
        }

        private void pm_appintent_check_CheckedChanged(object sender, EventArgs e)
        {
            pm_appintent_combo.Enabled = pm_appintent_check.Checked;

            pm_appintent_combo.DataSource = Enum.GetValues(typeof(ApplicationIntent));
        }
    }
}
