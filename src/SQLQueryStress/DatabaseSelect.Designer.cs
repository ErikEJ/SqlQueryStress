namespace SQLQueryStress
{
    partial class DatabaseSelect
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
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
      this.labelServerName = new System.Windows.Forms.Label();
      this.server_textBox = new System.Windows.Forms.TextBox();
      this.authentication_comboBox = new System.Windows.Forms.ComboBox();
      this.login_textBox = new System.Windows.Forms.TextBox();
      this.password_textBox = new System.Windows.Forms.TextBox();
      this.labelPassword = new System.Windows.Forms.Label();
      this.labelLogin = new System.Windows.Forms.Label();
      this.labelAuthentication = new System.Windows.Forms.Label();
      this.cancel_button = new System.Windows.Forms.Button();
      this.ButtonTestConnection = new System.Windows.Forms.Button();
      this.ok_button = new System.Windows.Forms.Button();
      this.db_comboBox = new System.Windows.Forms.ComboBox();
      this.labeldefaultDatabase = new System.Windows.Forms.Label();
      this.groupBoxDatabaseSettings = new System.Windows.Forms.GroupBox();
      this.appintent_combo = new System.Windows.Forms.ComboBox();
      this.appintent_check = new System.Windows.Forms.CheckBox();
      this.groupBoxParamSettings = new System.Windows.Forms.GroupBox();
      this.pm_appintent_combo = new System.Windows.Forms.ComboBox();
      this.pm_appintent_check = new System.Windows.Forms.CheckBox();
      this.pm_test_button = new System.Windows.Forms.Button();
      this.shareSettings_checkBox = new System.Windows.Forms.CheckBox();
      this.pm_db_comboBox = new System.Windows.Forms.ComboBox();
      this.labelParamDefaultDatabase = new System.Windows.Forms.Label();
      this.pm_password_textBox = new System.Windows.Forms.TextBox();
      this.labelParamPassword = new System.Windows.Forms.Label();
      this.pm_server_textBox = new System.Windows.Forms.TextBox();
      this.label8 = new System.Windows.Forms.Label();
      this.label9 = new System.Windows.Forms.Label();
      this.pm_login_textBox = new System.Windows.Forms.TextBox();
      this.pm_authentication_comboBox = new System.Windows.Forms.ComboBox();
      this.labelParameterizationLogin = new System.Windows.Forms.Label();
      this.groupBoxDatabaseSettings.SuspendLayout();
      this.groupBoxParamSettings.SuspendLayout();
      this.SuspendLayout();
      // 
      // labelServerName
      // 
      this.labelServerName.AutoSize = true;
      this.labelServerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
      this.labelServerName.Location = new System.Drawing.Point(7, 45);
      this.labelServerName.Name = "labelServerName";
      this.labelServerName.Size = new System.Drawing.Size(44, 13);
      this.labelServerName.TabIndex = 0;
      this.labelServerName.Text = "Server";
      // 
      // server_textBox
      // 
      this.server_textBox.Location = new System.Drawing.Point(11, 64);
      this.server_textBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.server_textBox.Name = "server_textBox";
      this.server_textBox.Size = new System.Drawing.Size(268, 23);
      this.server_textBox.TabIndex = 0;
      this.server_textBox.Text = "localhost";
      // 
      // authentication_comboBox
      // 
      this.authentication_comboBox.FormattingEnabled = true;
      this.authentication_comboBox.Items.AddRange(new object[] {
            "Integrated Authentication",
            "SQL Server Authentication"});
      this.authentication_comboBox.Location = new System.Drawing.Point(11, 109);
      this.authentication_comboBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.authentication_comboBox.Name = "authentication_comboBox";
      this.authentication_comboBox.Size = new System.Drawing.Size(268, 23);
      this.authentication_comboBox.TabIndex = 1;
      // 
      // login_textBox
      // 
      this.login_textBox.Location = new System.Drawing.Point(11, 154);
      this.login_textBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.login_textBox.Name = "login_textBox";
      this.login_textBox.Size = new System.Drawing.Size(268, 23);
      this.login_textBox.TabIndex = 2;
      // 
      // password_textBox
      // 
      this.password_textBox.Location = new System.Drawing.Point(11, 200);
      this.password_textBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.password_textBox.Name = "password_textBox";
      this.password_textBox.Size = new System.Drawing.Size(268, 23);
      this.password_textBox.TabIndex = 3;
      this.password_textBox.UseSystemPasswordChar = true;
      // 
      // labelPassword
      // 
      this.labelPassword.AutoSize = true;
      this.labelPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
      this.labelPassword.Location = new System.Drawing.Point(11, 182);
      this.labelPassword.Name = "labelPassword";
      this.labelPassword.Size = new System.Drawing.Size(61, 13);
      this.labelPassword.TabIndex = 5;
      this.labelPassword.Text = "Password";
      // 
      // labelLogin
      // 
      this.labelLogin.AutoSize = true;
      this.labelLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
      this.labelLogin.Location = new System.Drawing.Point(7, 136);
      this.labelLogin.Name = "labelLogin";
      this.labelLogin.Size = new System.Drawing.Size(38, 13);
      this.labelLogin.TabIndex = 6;
      this.labelLogin.Text = "Login";
      // 
      // labelAuthentication
      // 
      this.labelAuthentication.AutoSize = true;
      this.labelAuthentication.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
      this.labelAuthentication.Location = new System.Drawing.Point(7, 90);
      this.labelAuthentication.Name = "labelAuthentication";
      this.labelAuthentication.Size = new System.Drawing.Size(89, 13);
      this.labelAuthentication.TabIndex = 7;
      this.labelAuthentication.Text = "Authentication";
      // 
      // cancel_button
      // 
      this.cancel_button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancel_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
      this.cancel_button.Location = new System.Drawing.Point(502, 379);
      this.cancel_button.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.cancel_button.Name = "cancel_button";
      this.cancel_button.Size = new System.Drawing.Size(93, 26);
      this.cancel_button.TabIndex = 3;
      this.cancel_button.Text = "Cancel";
      this.cancel_button.UseVisualStyleBackColor = true;
      this.cancel_button.Click += new System.EventHandler(this.cancel_button_Click);
      // 
      // ButtonTestConnection
      // 
      this.ButtonTestConnection.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
      this.ButtonTestConnection.Location = new System.Drawing.Point(138, 322);
      this.ButtonTestConnection.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.ButtonTestConnection.Name = "ButtonTestConnection";
      this.ButtonTestConnection.Size = new System.Drawing.Size(140, 26);
      this.ButtonTestConnection.TabIndex = 5;
      this.ButtonTestConnection.Text = "Test Connection";
      this.ButtonTestConnection.UseVisualStyleBackColor = true;
      this.ButtonTestConnection.Click += new System.EventHandler(this.ButtonTestConnection_Click);
      // 
      // ok_button
      // 
      this.ok_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
      this.ok_button.Location = new System.Drawing.Point(402, 379);
      this.ok_button.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.ok_button.Name = "ok_button";
      this.ok_button.Size = new System.Drawing.Size(93, 26);
      this.ok_button.TabIndex = 2;
      this.ok_button.Text = "OK";
      this.ok_button.UseVisualStyleBackColor = true;
      this.ok_button.Click += new System.EventHandler(this.ok_button_Click);
      // 
      // db_comboBox
      // 
      this.db_comboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
      this.db_comboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.db_comboBox.FormattingEnabled = true;
      this.db_comboBox.Location = new System.Drawing.Point(11, 244);
      this.db_comboBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.db_comboBox.Name = "db_comboBox";
      this.db_comboBox.Size = new System.Drawing.Size(268, 23);
      this.db_comboBox.TabIndex = 4;
      // 
      // labeldefaultDatabase
      // 
      this.labeldefaultDatabase.AutoSize = true;
      this.labeldefaultDatabase.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
      this.labeldefaultDatabase.Location = new System.Drawing.Point(7, 226);
      this.labeldefaultDatabase.Name = "labeldefaultDatabase";
      this.labeldefaultDatabase.Size = new System.Drawing.Size(106, 13);
      this.labeldefaultDatabase.TabIndex = 12;
      this.labeldefaultDatabase.Text = "Default Database";
      // 
      // groupBoxDatabaseSettings
      // 
      this.groupBoxDatabaseSettings.Controls.Add(this.appintent_combo);
      this.groupBoxDatabaseSettings.Controls.Add(this.appintent_check);
      this.groupBoxDatabaseSettings.Controls.Add(this.db_comboBox);
      this.groupBoxDatabaseSettings.Controls.Add(this.labeldefaultDatabase);
      this.groupBoxDatabaseSettings.Controls.Add(this.password_textBox);
      this.groupBoxDatabaseSettings.Controls.Add(this.ButtonTestConnection);
      this.groupBoxDatabaseSettings.Controls.Add(this.labelPassword);
      this.groupBoxDatabaseSettings.Controls.Add(this.server_textBox);
      this.groupBoxDatabaseSettings.Controls.Add(this.labelServerName);
      this.groupBoxDatabaseSettings.Controls.Add(this.labelAuthentication);
      this.groupBoxDatabaseSettings.Controls.Add(this.login_textBox);
      this.groupBoxDatabaseSettings.Controls.Add(this.authentication_comboBox);
      this.groupBoxDatabaseSettings.Controls.Add(this.labelLogin);
      this.groupBoxDatabaseSettings.Location = new System.Drawing.Point(14, 14);
      this.groupBoxDatabaseSettings.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.groupBoxDatabaseSettings.Name = "groupBoxDatabaseSettings";
      this.groupBoxDatabaseSettings.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.groupBoxDatabaseSettings.Size = new System.Drawing.Size(287, 358);
      this.groupBoxDatabaseSettings.TabIndex = 0;
      this.groupBoxDatabaseSettings.TabStop = false;
      this.groupBoxDatabaseSettings.Text = "Main Load Settings";
      // 
      // appintent_combo
      // 
      this.appintent_combo.Enabled = false;
      this.appintent_combo.FormattingEnabled = true;
      this.appintent_combo.Items.AddRange(new object[] {
            "ReadWrite ",
            "ReadOnly"});
      this.appintent_combo.Location = new System.Drawing.Point(11, 296);
      this.appintent_combo.Margin = new System.Windows.Forms.Padding(2);
      this.appintent_combo.Name = "appintent_combo";
      this.appintent_combo.Size = new System.Drawing.Size(268, 23);
      this.appintent_combo.TabIndex = 14;
      // 
      // appintent_check
      // 
      this.appintent_check.AutoSize = true;
      this.appintent_check.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
      this.appintent_check.Location = new System.Drawing.Point(11, 272);
      this.appintent_check.Margin = new System.Windows.Forms.Padding(2);
      this.appintent_check.Name = "appintent_check";
      this.appintent_check.Size = new System.Drawing.Size(126, 17);
      this.appintent_check.TabIndex = 13;
      this.appintent_check.Text = "Application Intent";
      this.appintent_check.UseVisualStyleBackColor = true;
      this.appintent_check.CheckedChanged += new System.EventHandler(this.Appintent_check_CheckedChanged);
      // 
      // groupBoxParamSettings
      // 
      this.groupBoxParamSettings.Controls.Add(this.pm_appintent_combo);
      this.groupBoxParamSettings.Controls.Add(this.pm_appintent_check);
      this.groupBoxParamSettings.Controls.Add(this.pm_test_button);
      this.groupBoxParamSettings.Controls.Add(this.shareSettings_checkBox);
      this.groupBoxParamSettings.Controls.Add(this.pm_db_comboBox);
      this.groupBoxParamSettings.Controls.Add(this.labelParamDefaultDatabase);
      this.groupBoxParamSettings.Controls.Add(this.pm_password_textBox);
      this.groupBoxParamSettings.Controls.Add(this.labelParamPassword);
      this.groupBoxParamSettings.Controls.Add(this.pm_server_textBox);
      this.groupBoxParamSettings.Controls.Add(this.label8);
      this.groupBoxParamSettings.Controls.Add(this.label9);
      this.groupBoxParamSettings.Controls.Add(this.pm_login_textBox);
      this.groupBoxParamSettings.Controls.Add(this.pm_authentication_comboBox);
      this.groupBoxParamSettings.Controls.Add(this.labelParameterizationLogin);
      this.groupBoxParamSettings.Location = new System.Drawing.Point(308, 14);
      this.groupBoxParamSettings.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.groupBoxParamSettings.Name = "groupBoxParamSettings";
      this.groupBoxParamSettings.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.groupBoxParamSettings.Size = new System.Drawing.Size(287, 358);
      this.groupBoxParamSettings.TabIndex = 1;
      this.groupBoxParamSettings.TabStop = false;
      this.groupBoxParamSettings.Text = "Parameterization Settings";
      // 
      // pm_appintent_combo
      // 
      this.pm_appintent_combo.Enabled = false;
      this.pm_appintent_combo.FormattingEnabled = true;
      this.pm_appintent_combo.Items.AddRange(new object[] {
            "ReadWrite ",
            "ReadOnly"});
      this.pm_appintent_combo.Location = new System.Drawing.Point(10, 296);
      this.pm_appintent_combo.Margin = new System.Windows.Forms.Padding(2);
      this.pm_appintent_combo.Name = "pm_appintent_combo";
      this.pm_appintent_combo.Size = new System.Drawing.Size(268, 23);
      this.pm_appintent_combo.TabIndex = 16;
      // 
      // pm_appintent_check
      // 
      this.pm_appintent_check.AutoSize = true;
      this.pm_appintent_check.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
      this.pm_appintent_check.Location = new System.Drawing.Point(10, 272);
      this.pm_appintent_check.Margin = new System.Windows.Forms.Padding(2);
      this.pm_appintent_check.Name = "pm_appintent_check";
      this.pm_appintent_check.Size = new System.Drawing.Size(126, 17);
      this.pm_appintent_check.TabIndex = 15;
      this.pm_appintent_check.Text = "Application Intent";
      this.pm_appintent_check.UseVisualStyleBackColor = true;
      this.pm_appintent_check.CheckedChanged += new System.EventHandler(this.pm_appintent_check_CheckedChanged);
      // 
      // pm_test_button
      // 
      this.pm_test_button.Enabled = false;
      this.pm_test_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
      this.pm_test_button.Location = new System.Drawing.Point(138, 322);
      this.pm_test_button.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.pm_test_button.Name = "pm_test_button";
      this.pm_test_button.Size = new System.Drawing.Size(140, 26);
      this.pm_test_button.TabIndex = 5;
      this.pm_test_button.Text = "Test Connection";
      this.pm_test_button.UseVisualStyleBackColor = true;
      this.pm_test_button.Click += new System.EventHandler(this.pm_test_button_Click);
      // 
      // shareSettings_checkBox
      // 
      this.shareSettings_checkBox.AutoSize = true;
      this.shareSettings_checkBox.Checked = true;
      this.shareSettings_checkBox.CheckState = System.Windows.Forms.CheckState.Checked;
      this.shareSettings_checkBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
      this.shareSettings_checkBox.Location = new System.Drawing.Point(14, 22);
      this.shareSettings_checkBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.shareSettings_checkBox.Name = "shareSettings_checkBox";
      this.shareSettings_checkBox.Size = new System.Drawing.Size(177, 17);
      this.shareSettings_checkBox.TabIndex = 13;
      this.shareSettings_checkBox.Text = "Share Connection Settings";
      this.shareSettings_checkBox.UseVisualStyleBackColor = true;
      this.shareSettings_checkBox.CheckedChanged += new System.EventHandler(this.shareSettings_checkBox_CheckedChanged);
      // 
      // pm_db_comboBox
      // 
      this.pm_db_comboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
      this.pm_db_comboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.pm_db_comboBox.Enabled = false;
      this.pm_db_comboBox.FormattingEnabled = true;
      this.pm_db_comboBox.Location = new System.Drawing.Point(11, 244);
      this.pm_db_comboBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.pm_db_comboBox.Name = "pm_db_comboBox";
      this.pm_db_comboBox.Size = new System.Drawing.Size(268, 23);
      this.pm_db_comboBox.TabIndex = 4;
      // 
      // labelParamDefaultDatabase
      // 
      this.labelParamDefaultDatabase.AutoSize = true;
      this.labelParamDefaultDatabase.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
      this.labelParamDefaultDatabase.Location = new System.Drawing.Point(7, 226);
      this.labelParamDefaultDatabase.Name = "labelParamDefaultDatabase";
      this.labelParamDefaultDatabase.Size = new System.Drawing.Size(106, 13);
      this.labelParamDefaultDatabase.TabIndex = 12;
      this.labelParamDefaultDatabase.Text = "Default Database";
      // 
      // pm_password_textBox
      // 
      this.pm_password_textBox.Enabled = false;
      this.pm_password_textBox.Location = new System.Drawing.Point(11, 200);
      this.pm_password_textBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.pm_password_textBox.Name = "pm_password_textBox";
      this.pm_password_textBox.Size = new System.Drawing.Size(268, 23);
      this.pm_password_textBox.TabIndex = 3;
      this.pm_password_textBox.UseSystemPasswordChar = true;
      // 
      // labelParamPassword
      // 
      this.labelParamPassword.AutoSize = true;
      this.labelParamPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
      this.labelParamPassword.Location = new System.Drawing.Point(11, 182);
      this.labelParamPassword.Name = "labelParamPassword";
      this.labelParamPassword.Size = new System.Drawing.Size(61, 13);
      this.labelParamPassword.TabIndex = 5;
      this.labelParamPassword.Text = "Password";
      // 
      // pm_server_textBox
      // 
      this.pm_server_textBox.Enabled = false;
      this.pm_server_textBox.Location = new System.Drawing.Point(11, 64);
      this.pm_server_textBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.pm_server_textBox.Name = "pm_server_textBox";
      this.pm_server_textBox.Size = new System.Drawing.Size(268, 23);
      this.pm_server_textBox.TabIndex = 0;
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
      this.label8.Location = new System.Drawing.Point(7, 45);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(44, 13);
      this.label8.TabIndex = 0;
      this.label8.Text = "Server";
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
      this.label9.Location = new System.Drawing.Point(7, 90);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(89, 13);
      this.label9.TabIndex = 7;
      this.label9.Text = "Authentication";
      // 
      // pm_login_textBox
      // 
      this.pm_login_textBox.Enabled = false;
      this.pm_login_textBox.Location = new System.Drawing.Point(11, 154);
      this.pm_login_textBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.pm_login_textBox.Name = "pm_login_textBox";
      this.pm_login_textBox.Size = new System.Drawing.Size(268, 23);
      this.pm_login_textBox.TabIndex = 2;
      // 
      // pm_authentication_comboBox
      // 
      this.pm_authentication_comboBox.Enabled = false;
      this.pm_authentication_comboBox.FormattingEnabled = true;
      this.pm_authentication_comboBox.Items.AddRange(new object[] {
            "Integrated Authentication",
            "SQL Server Authentication"});
      this.pm_authentication_comboBox.Location = new System.Drawing.Point(11, 109);
      this.pm_authentication_comboBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.pm_authentication_comboBox.Name = "pm_authentication_comboBox";
      this.pm_authentication_comboBox.Size = new System.Drawing.Size(268, 23);
      this.pm_authentication_comboBox.TabIndex = 1;
      // 
      // labelParameterizationLogin
      // 
      this.labelParameterizationLogin.AutoSize = true;
      this.labelParameterizationLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
      this.labelParameterizationLogin.Location = new System.Drawing.Point(7, 136);
      this.labelParameterizationLogin.Name = "labelParameterizationLogin";
      this.labelParameterizationLogin.Size = new System.Drawing.Size(38, 13);
      this.labelParameterizationLogin.TabIndex = 6;
      this.labelParameterizationLogin.Text = "Login";
      // 
      // DatabaseSelect
      // 
      this.AcceptButton = this.ok_button;
      this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.cancel_button;
      this.ClientSize = new System.Drawing.Size(611, 417);
      this.Controls.Add(this.groupBoxParamSettings);
      this.Controls.Add(this.groupBoxDatabaseSettings);
      this.Controls.Add(this.ok_button);
      this.Controls.Add(this.cancel_button);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "DatabaseSelect";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.Text = "Database Select";
      this.groupBoxDatabaseSettings.ResumeLayout(false);
      this.groupBoxDatabaseSettings.PerformLayout();
      this.groupBoxParamSettings.ResumeLayout(false);
      this.groupBoxParamSettings.PerformLayout();
      this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelServerName;
        private System.Windows.Forms.TextBox server_textBox;
        private System.Windows.Forms.ComboBox authentication_comboBox;
        private System.Windows.Forms.TextBox login_textBox;
        private System.Windows.Forms.TextBox password_textBox;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.Label labelLogin;
        private System.Windows.Forms.Label labelAuthentication;
        private System.Windows.Forms.Button cancel_button;
        private System.Windows.Forms.Button ButtonTestConnection;
        private System.Windows.Forms.Button ok_button;
        private System.Windows.Forms.ComboBox db_comboBox;
        private System.Windows.Forms.Label labeldefaultDatabase;
        private System.Windows.Forms.GroupBox groupBoxDatabaseSettings;
        private System.Windows.Forms.GroupBox groupBoxParamSettings;
        private System.Windows.Forms.CheckBox shareSettings_checkBox;
        private System.Windows.Forms.ComboBox pm_db_comboBox;
        private System.Windows.Forms.Label labelParamDefaultDatabase;
        private System.Windows.Forms.TextBox pm_password_textBox;
        private System.Windows.Forms.Label labelParamPassword;
        private System.Windows.Forms.TextBox pm_server_textBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox pm_login_textBox;
        private System.Windows.Forms.ComboBox pm_authentication_comboBox;
        private System.Windows.Forms.Label labelParameterizationLogin;
        private System.Windows.Forms.Button pm_test_button;
        private System.Windows.Forms.CheckBox appintent_check;
        private System.Windows.Forms.ComboBox appintent_combo;
        private System.Windows.Forms.ComboBox pm_appintent_combo;
        private System.Windows.Forms.CheckBox pm_appintent_check;
    }
}