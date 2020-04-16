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
            this.label1 = new System.Windows.Forms.Label();
            this.server_textBox = new System.Windows.Forms.TextBox();
            this.authentication_comboBox = new System.Windows.Forms.ComboBox();
            this.login_textBox = new System.Windows.Forms.TextBox();
            this.password_textBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cancel_button = new System.Windows.Forms.Button();
            this.test_button = new System.Windows.Forms.Button();
            this.ok_button = new System.Windows.Forms.Button();
            this.db_comboBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.appintent_combo = new System.Windows.Forms.ComboBox();
            this.appintent_check = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pm_test_button = new System.Windows.Forms.Button();
            this.shareSettings_checkBox = new System.Windows.Forms.CheckBox();
            this.pm_db_comboBox = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.pm_password_textBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.pm_server_textBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.pm_login_textBox = new System.Windows.Forms.TextBox();
            this.pm_authentication_comboBox = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.pm_appintent_combo = new System.Windows.Forms.ComboBox();
            this.pm_appintent_check = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 60);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server";
            // 
            // server_textBox
            // 
            this.server_textBox.Location = new System.Drawing.Point(14, 85);
            this.server_textBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.server_textBox.Name = "server_textBox";
            this.server_textBox.Size = new System.Drawing.Size(344, 26);
            this.server_textBox.TabIndex = 0;
            // 
            // authentication_comboBox
            // 
            this.authentication_comboBox.FormattingEnabled = true;
            this.authentication_comboBox.Items.AddRange(new object[] {
            "Integrated Authentication",
            "SQL Server Authentication"});
            this.authentication_comboBox.Location = new System.Drawing.Point(14, 145);
            this.authentication_comboBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.authentication_comboBox.Name = "authentication_comboBox";
            this.authentication_comboBox.Size = new System.Drawing.Size(344, 28);
            this.authentication_comboBox.TabIndex = 1;
            // 
            // login_textBox
            // 
            this.login_textBox.Location = new System.Drawing.Point(14, 206);
            this.login_textBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.login_textBox.Name = "login_textBox";
            this.login_textBox.Size = new System.Drawing.Size(344, 26);
            this.login_textBox.TabIndex = 2;
            // 
            // password_textBox
            // 
            this.password_textBox.Location = new System.Drawing.Point(14, 266);
            this.password_textBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.password_textBox.Name = "password_textBox";
            this.password_textBox.Size = new System.Drawing.Size(344, 26);
            this.password_textBox.TabIndex = 3;
            this.password_textBox.UseSystemPasswordChar = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(14, 242);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Password";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(9, 182);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Login";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(9, 120);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(129, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = "Authentication";
            // 
            // cancel_button
            // 
            this.cancel_button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancel_button.Location = new System.Drawing.Point(645, 505);
            this.cancel_button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cancel_button.Name = "cancel_button";
            this.cancel_button.Size = new System.Drawing.Size(120, 35);
            this.cancel_button.TabIndex = 3;
            this.cancel_button.Text = "Cancel";
            this.cancel_button.UseVisualStyleBackColor = true;
            this.cancel_button.Click += new System.EventHandler(this.cancel_button_Click);
            // 
            // test_button
            // 
            this.test_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.test_button.Location = new System.Drawing.Point(178, 430);
            this.test_button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.test_button.Name = "test_button";
            this.test_button.Size = new System.Drawing.Size(180, 35);
            this.test_button.TabIndex = 5;
            this.test_button.Text = "Test Connection";
            this.test_button.UseVisualStyleBackColor = true;
            this.test_button.Click += new System.EventHandler(this.test_button_Click);
            // 
            // ok_button
            // 
            this.ok_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ok_button.Location = new System.Drawing.Point(517, 505);
            this.ok_button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ok_button.Name = "ok_button";
            this.ok_button.Size = new System.Drawing.Size(120, 35);
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
            this.db_comboBox.Location = new System.Drawing.Point(14, 326);
            this.db_comboBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.db_comboBox.Name = "db_comboBox";
            this.db_comboBox.Size = new System.Drawing.Size(344, 28);
            this.db_comboBox.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(9, 302);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(156, 20);
            this.label5.TabIndex = 12;
            this.label5.Text = "Default Database";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.appintent_combo);
            this.groupBox1.Controls.Add(this.appintent_check);
            this.groupBox1.Controls.Add(this.db_comboBox);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.password_textBox);
            this.groupBox1.Controls.Add(this.test_button);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.server_textBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.login_textBox);
            this.groupBox1.Controls.Add(this.authentication_comboBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(18, 18);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(369, 477);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Main Load Settings";
            // 
            // appintent_combo
            // 
            this.appintent_combo.Enabled = false;
            this.appintent_combo.FormattingEnabled = true;
            this.appintent_combo.Items.AddRange(new object[] {
            "ReadWrite ",
            "ReadOnly"});
            this.appintent_combo.Location = new System.Drawing.Point(14, 394);
            this.appintent_combo.Name = "appintent_combo";
            this.appintent_combo.Size = new System.Drawing.Size(344, 28);
            this.appintent_combo.TabIndex = 14;
            // 
            // appintent_check
            // 
            this.appintent_check.AutoSize = true;
            this.appintent_check.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.appintent_check.Location = new System.Drawing.Point(14, 363);
            this.appintent_check.Name = "appintent_check";
            this.appintent_check.Size = new System.Drawing.Size(177, 24);
            this.appintent_check.TabIndex = 13;
            this.appintent_check.Text = "Application Intent";
            this.appintent_check.UseVisualStyleBackColor = true;
            this.appintent_check.CheckedChanged += new System.EventHandler(this.appintent_check_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pm_appintent_combo);
            this.groupBox2.Controls.Add(this.pm_appintent_check);
            this.groupBox2.Controls.Add(this.pm_test_button);
            this.groupBox2.Controls.Add(this.shareSettings_checkBox);
            this.groupBox2.Controls.Add(this.pm_db_comboBox);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.pm_password_textBox);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.pm_server_textBox);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.pm_login_textBox);
            this.groupBox2.Controls.Add(this.pm_authentication_comboBox);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Location = new System.Drawing.Point(396, 18);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Size = new System.Drawing.Size(369, 477);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Parameterization Settings";
            // 
            // pm_test_button
            // 
            this.pm_test_button.Enabled = false;
            this.pm_test_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pm_test_button.Location = new System.Drawing.Point(177, 430);
            this.pm_test_button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pm_test_button.Name = "pm_test_button";
            this.pm_test_button.Size = new System.Drawing.Size(180, 35);
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
            this.shareSettings_checkBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.shareSettings_checkBox.Location = new System.Drawing.Point(18, 29);
            this.shareSettings_checkBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.shareSettings_checkBox.Name = "shareSettings_checkBox";
            this.shareSettings_checkBox.Size = new System.Drawing.Size(259, 24);
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
            this.pm_db_comboBox.Location = new System.Drawing.Point(14, 326);
            this.pm_db_comboBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pm_db_comboBox.Name = "pm_db_comboBox";
            this.pm_db_comboBox.Size = new System.Drawing.Size(344, 28);
            this.pm_db_comboBox.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(9, 302);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(156, 20);
            this.label6.TabIndex = 12;
            this.label6.Text = "Default Database";
            // 
            // pm_password_textBox
            // 
            this.pm_password_textBox.Enabled = false;
            this.pm_password_textBox.Location = new System.Drawing.Point(14, 266);
            this.pm_password_textBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pm_password_textBox.Name = "pm_password_textBox";
            this.pm_password_textBox.Size = new System.Drawing.Size(344, 26);
            this.pm_password_textBox.TabIndex = 3;
            this.pm_password_textBox.UseSystemPasswordChar = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(14, 242);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(91, 20);
            this.label7.TabIndex = 5;
            this.label7.Text = "Password";
            // 
            // pm_server_textBox
            // 
            this.pm_server_textBox.Enabled = false;
            this.pm_server_textBox.Location = new System.Drawing.Point(14, 85);
            this.pm_server_textBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pm_server_textBox.Name = "pm_server_textBox";
            this.pm_server_textBox.Size = new System.Drawing.Size(344, 26);
            this.pm_server_textBox.TabIndex = 0;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(9, 60);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 20);
            this.label8.TabIndex = 0;
            this.label8.Text = "Server";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(9, 120);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(129, 20);
            this.label9.TabIndex = 7;
            this.label9.Text = "Authentication";
            // 
            // pm_login_textBox
            // 
            this.pm_login_textBox.Enabled = false;
            this.pm_login_textBox.Location = new System.Drawing.Point(14, 206);
            this.pm_login_textBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pm_login_textBox.Name = "pm_login_textBox";
            this.pm_login_textBox.Size = new System.Drawing.Size(344, 26);
            this.pm_login_textBox.TabIndex = 2;
            // 
            // pm_authentication_comboBox
            // 
            this.pm_authentication_comboBox.Enabled = false;
            this.pm_authentication_comboBox.FormattingEnabled = true;
            this.pm_authentication_comboBox.Items.AddRange(new object[] {
            "Integrated Authentication",
            "SQL Server Authentication"});
            this.pm_authentication_comboBox.Location = new System.Drawing.Point(14, 145);
            this.pm_authentication_comboBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pm_authentication_comboBox.Name = "pm_authentication_comboBox";
            this.pm_authentication_comboBox.Size = new System.Drawing.Size(344, 28);
            this.pm_authentication_comboBox.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(9, 182);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(55, 20);
            this.label10.TabIndex = 6;
            this.label10.Text = "Login";
            // 
            // pm_appintent_combo
            // 
            this.pm_appintent_combo.Enabled = false;
            this.pm_appintent_combo.FormattingEnabled = true;
            this.pm_appintent_combo.Items.AddRange(new object[] {
            "ReadWrite ",
            "ReadOnly"});
            this.pm_appintent_combo.Location = new System.Drawing.Point(13, 394);
            this.pm_appintent_combo.Name = "pm_appintent_combo";
            this.pm_appintent_combo.Size = new System.Drawing.Size(344, 28);
            this.pm_appintent_combo.TabIndex = 16;
            // 
            // pm_appintent_check
            // 
            this.pm_appintent_check.AutoSize = true;
            this.pm_appintent_check.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pm_appintent_check.Location = new System.Drawing.Point(13, 363);
            this.pm_appintent_check.Name = "pm_appintent_check";
            this.pm_appintent_check.Size = new System.Drawing.Size(177, 24);
            this.pm_appintent_check.TabIndex = 15;
            this.pm_appintent_check.Text = "Application Intent";
            this.pm_appintent_check.UseVisualStyleBackColor = true;
            this.pm_appintent_check.CheckedChanged += new System.EventHandler(this.pm_appintent_check_CheckedChanged);
            // 
            // DatabaseSelect
            // 
            this.AcceptButton = this.ok_button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel_button;
            this.ClientSize = new System.Drawing.Size(786, 556);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ok_button);
            this.Controls.Add(this.cancel_button);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DatabaseSelect";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Database Select";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox server_textBox;
        private System.Windows.Forms.ComboBox authentication_comboBox;
        private System.Windows.Forms.TextBox login_textBox;
        private System.Windows.Forms.TextBox password_textBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button cancel_button;
        private System.Windows.Forms.Button test_button;
        private System.Windows.Forms.Button ok_button;
        private System.Windows.Forms.ComboBox db_comboBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox shareSettings_checkBox;
        private System.Windows.Forms.ComboBox pm_db_comboBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox pm_password_textBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox pm_server_textBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox pm_login_textBox;
        private System.Windows.Forms.ComboBox pm_authentication_comboBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button pm_test_button;
        private System.Windows.Forms.CheckBox appintent_check;
        private System.Windows.Forms.ComboBox appintent_combo;
        private System.Windows.Forms.ComboBox pm_appintent_combo;
        private System.Windows.Forms.CheckBox pm_appintent_check;
    }
}