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
            label1 = new System.Windows.Forms.Label();
            server_textBox = new System.Windows.Forms.TextBox();
            authentication_comboBox = new System.Windows.Forms.ComboBox();
            login_textBox = new System.Windows.Forms.TextBox();
            password_textBox = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            cancel_button = new System.Windows.Forms.Button();
            test_button = new System.Windows.Forms.Button();
            ok_button = new System.Windows.Forms.Button();
            db_comboBox = new System.Windows.Forms.ComboBox();
            label5 = new System.Windows.Forms.Label();
            groupBox1 = new System.Windows.Forms.GroupBox();
            encrypt_Combo = new System.Windows.Forms.ComboBox();
            label11 = new System.Windows.Forms.Label();
            appintent_combo = new System.Windows.Forms.ComboBox();
            appintent_check = new System.Windows.Forms.CheckBox();
            groupBox2 = new System.Windows.Forms.GroupBox();
            pm_encrypt_Combo = new System.Windows.Forms.ComboBox();
            label12 = new System.Windows.Forms.Label();
            pm_appintent_combo = new System.Windows.Forms.ComboBox();
            pm_appintent_check = new System.Windows.Forms.CheckBox();
            pm_test_button = new System.Windows.Forms.Button();
            shareSettings_checkBox = new System.Windows.Forms.CheckBox();
            pm_db_comboBox = new System.Windows.Forms.ComboBox();
            label6 = new System.Windows.Forms.Label();
            pm_password_textBox = new System.Windows.Forms.TextBox();
            label7 = new System.Windows.Forms.Label();
            pm_server_textBox = new System.Windows.Forms.TextBox();
            label8 = new System.Windows.Forms.Label();
            label9 = new System.Windows.Forms.Label();
            pm_login_textBox = new System.Windows.Forms.TextBox();
            pm_authentication_comboBox = new System.Windows.Forms.ComboBox();
            label10 = new System.Windows.Forms.Label();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label1.Location = new System.Drawing.Point(10, 75);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(64, 20);
            label1.TabIndex = 0;
            label1.Text = "Server";
            // 
            // server_textBox
            // 
            server_textBox.Location = new System.Drawing.Point(16, 106);
            server_textBox.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            server_textBox.Name = "server_textBox";
            server_textBox.Size = new System.Drawing.Size(382, 31);
            server_textBox.TabIndex = 0;
            // 
            // authentication_comboBox
            // 
            authentication_comboBox.FormattingEnabled = true;
            authentication_comboBox.Items.AddRange(new object[] { "Integrated Authentication", "SQL Server Authentication", "Azure Active Directory - Universal with MFA" });
            authentication_comboBox.Location = new System.Drawing.Point(16, 181);
            authentication_comboBox.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            authentication_comboBox.Name = "authentication_comboBox";
            authentication_comboBox.Size = new System.Drawing.Size(382, 33);
            authentication_comboBox.TabIndex = 1;
            // 
            // login_textBox
            // 
            login_textBox.Location = new System.Drawing.Point(16, 258);
            login_textBox.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            login_textBox.Name = "login_textBox";
            login_textBox.Size = new System.Drawing.Size(382, 31);
            login_textBox.TabIndex = 2;
            // 
            // password_textBox
            // 
            password_textBox.Location = new System.Drawing.Point(16, 332);
            password_textBox.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            password_textBox.Name = "password_textBox";
            password_textBox.Size = new System.Drawing.Size(382, 31);
            password_textBox.TabIndex = 3;
            password_textBox.UseSystemPasswordChar = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label2.Location = new System.Drawing.Point(16, 302);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(91, 20);
            label2.TabIndex = 5;
            label2.Text = "Password";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label3.Location = new System.Drawing.Point(10, 228);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(55, 20);
            label3.TabIndex = 6;
            label3.Text = "Login";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label4.Location = new System.Drawing.Point(10, 150);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(129, 20);
            label4.TabIndex = 7;
            label4.Text = "Authentication";
            // 
            // cancel_button
            // 
            cancel_button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancel_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            cancel_button.Location = new System.Drawing.Point(717, 724);
            cancel_button.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            cancel_button.Name = "cancel_button";
            cancel_button.Size = new System.Drawing.Size(133, 44);
            cancel_button.TabIndex = 3;
            cancel_button.Text = "Cancel";
            cancel_button.UseVisualStyleBackColor = true;
            cancel_button.Click += cancel_button_Click;
            // 
            // test_button
            // 
            test_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            test_button.Location = new System.Drawing.Point(202, 619);
            test_button.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            test_button.Name = "test_button";
            test_button.Size = new System.Drawing.Size(200, 44);
            test_button.TabIndex = 5;
            test_button.Text = "Test Connection";
            test_button.UseVisualStyleBackColor = true;
            test_button.Click += test_button_Click;
            // 
            // ok_button
            // 
            ok_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            ok_button.Location = new System.Drawing.Point(573, 724);
            ok_button.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            ok_button.Name = "ok_button";
            ok_button.Size = new System.Drawing.Size(133, 44);
            ok_button.TabIndex = 2;
            ok_button.Text = "OK";
            ok_button.UseVisualStyleBackColor = true;
            ok_button.Click += ok_button_Click;
            // 
            // db_comboBox
            // 
            db_comboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            db_comboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            db_comboBox.FormattingEnabled = true;
            db_comboBox.Location = new System.Drawing.Point(16, 408);
            db_comboBox.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            db_comboBox.Name = "db_comboBox";
            db_comboBox.Size = new System.Drawing.Size(382, 33);
            db_comboBox.TabIndex = 4;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label5.Location = new System.Drawing.Point(10, 378);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(156, 20);
            label5.TabIndex = 12;
            label5.Text = "Default Database";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(encrypt_Combo);
            groupBox1.Controls.Add(label11);
            groupBox1.Controls.Add(appintent_combo);
            groupBox1.Controls.Add(appintent_check);
            groupBox1.Controls.Add(db_comboBox);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(password_textBox);
            groupBox1.Controls.Add(test_button);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(server_textBox);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(login_textBox);
            groupBox1.Controls.Add(authentication_comboBox);
            groupBox1.Controls.Add(label3);
            groupBox1.Location = new System.Drawing.Point(20, 22);
            groupBox1.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(4, 6, 4, 6);
            groupBox1.Size = new System.Drawing.Size(410, 675);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Main Load Settings";
            // 
            // encrypt_Combo
            // 
            encrypt_Combo.FormattingEnabled = true;
            encrypt_Combo.Items.AddRange(new object[] { "False", "True", "Strict" });
            encrypt_Combo.Location = new System.Drawing.Point(16, 565);
            encrypt_Combo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            encrypt_Combo.Name = "encrypt_Combo";
            encrypt_Combo.Size = new System.Drawing.Size(382, 33);
            encrypt_Combo.TabIndex = 17;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label11.Location = new System.Drawing.Point(10, 532);
            label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(73, 20);
            label11.TabIndex = 16;
            label11.Text = "Encrypt";
            // 
            // appintent_combo
            // 
            appintent_combo.Enabled = false;
            appintent_combo.FormattingEnabled = true;
            appintent_combo.Items.AddRange(new object[] { "ReadWrite ", "ReadOnly" });
            appintent_combo.Location = new System.Drawing.Point(16, 492);
            appintent_combo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            appintent_combo.Name = "appintent_combo";
            appintent_combo.Size = new System.Drawing.Size(382, 33);
            appintent_combo.TabIndex = 14;
            // 
            // appintent_check
            // 
            appintent_check.AutoSize = true;
            appintent_check.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            appintent_check.Location = new System.Drawing.Point(16, 454);
            appintent_check.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            appintent_check.Name = "appintent_check";
            appintent_check.Size = new System.Drawing.Size(177, 24);
            appintent_check.TabIndex = 13;
            appintent_check.Text = "Application Intent";
            appintent_check.UseVisualStyleBackColor = true;
            appintent_check.CheckedChanged += appintent_check_CheckedChanged;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(pm_encrypt_Combo);
            groupBox2.Controls.Add(label12);
            groupBox2.Controls.Add(pm_appintent_combo);
            groupBox2.Controls.Add(pm_appintent_check);
            groupBox2.Controls.Add(pm_test_button);
            groupBox2.Controls.Add(shareSettings_checkBox);
            groupBox2.Controls.Add(pm_db_comboBox);
            groupBox2.Controls.Add(label6);
            groupBox2.Controls.Add(pm_password_textBox);
            groupBox2.Controls.Add(label7);
            groupBox2.Controls.Add(pm_server_textBox);
            groupBox2.Controls.Add(label8);
            groupBox2.Controls.Add(label9);
            groupBox2.Controls.Add(pm_login_textBox);
            groupBox2.Controls.Add(pm_authentication_comboBox);
            groupBox2.Controls.Add(label10);
            groupBox2.Location = new System.Drawing.Point(440, 22);
            groupBox2.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new System.Windows.Forms.Padding(4, 6, 4, 6);
            groupBox2.Size = new System.Drawing.Size(410, 675);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Parameterization Settings";
            // 
            // pm_encrypt_Combo
            // 
            pm_encrypt_Combo.FormattingEnabled = true;
            pm_encrypt_Combo.Items.AddRange(new object[] { "Optional", "Mandatory", "Strict" });
            pm_encrypt_Combo.Location = new System.Drawing.Point(14, 565);
            pm_encrypt_Combo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pm_encrypt_Combo.Name = "pm_encrypt_Combo";
            pm_encrypt_Combo.Size = new System.Drawing.Size(382, 33);
            pm_encrypt_Combo.TabIndex = 18;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label12.Location = new System.Drawing.Point(11, 533);
            label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label12.Name = "label12";
            label12.Size = new System.Drawing.Size(73, 20);
            label12.TabIndex = 18;
            label12.Text = "Encrypt";
            // 
            // pm_appintent_combo
            // 
            pm_appintent_combo.Enabled = false;
            pm_appintent_combo.FormattingEnabled = true;
            pm_appintent_combo.Items.AddRange(new object[] { "ReadWrite ", "ReadOnly" });
            pm_appintent_combo.Location = new System.Drawing.Point(14, 492);
            pm_appintent_combo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pm_appintent_combo.Name = "pm_appintent_combo";
            pm_appintent_combo.Size = new System.Drawing.Size(382, 33);
            pm_appintent_combo.TabIndex = 16;
            // 
            // pm_appintent_check
            // 
            pm_appintent_check.AutoSize = true;
            pm_appintent_check.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            pm_appintent_check.Location = new System.Drawing.Point(14, 454);
            pm_appintent_check.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pm_appintent_check.Name = "pm_appintent_check";
            pm_appintent_check.Size = new System.Drawing.Size(177, 24);
            pm_appintent_check.TabIndex = 15;
            pm_appintent_check.Text = "Application Intent";
            pm_appintent_check.UseVisualStyleBackColor = true;
            pm_appintent_check.CheckedChanged += pm_appintent_check_CheckedChanged;
            // 
            // pm_test_button
            // 
            pm_test_button.Enabled = false;
            pm_test_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            pm_test_button.Location = new System.Drawing.Point(196, 619);
            pm_test_button.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            pm_test_button.Name = "pm_test_button";
            pm_test_button.Size = new System.Drawing.Size(200, 44);
            pm_test_button.TabIndex = 5;
            pm_test_button.Text = "Test Connection";
            pm_test_button.UseVisualStyleBackColor = true;
            pm_test_button.Click += pm_test_button_Click;
            // 
            // shareSettings_checkBox
            // 
            shareSettings_checkBox.AutoSize = true;
            shareSettings_checkBox.Checked = true;
            shareSettings_checkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            shareSettings_checkBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            shareSettings_checkBox.Location = new System.Drawing.Point(20, 36);
            shareSettings_checkBox.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            shareSettings_checkBox.Name = "shareSettings_checkBox";
            shareSettings_checkBox.Size = new System.Drawing.Size(259, 24);
            shareSettings_checkBox.TabIndex = 13;
            shareSettings_checkBox.Text = "Share Connection Settings";
            shareSettings_checkBox.UseVisualStyleBackColor = true;
            shareSettings_checkBox.CheckedChanged += shareSettings_checkBox_CheckedChanged;
            // 
            // pm_db_comboBox
            // 
            pm_db_comboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            pm_db_comboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            pm_db_comboBox.Enabled = false;
            pm_db_comboBox.FormattingEnabled = true;
            pm_db_comboBox.Location = new System.Drawing.Point(16, 408);
            pm_db_comboBox.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            pm_db_comboBox.Name = "pm_db_comboBox";
            pm_db_comboBox.Size = new System.Drawing.Size(382, 33);
            pm_db_comboBox.TabIndex = 4;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label6.Location = new System.Drawing.Point(10, 378);
            label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(156, 20);
            label6.TabIndex = 12;
            label6.Text = "Default Database";
            // 
            // pm_password_textBox
            // 
            pm_password_textBox.Enabled = false;
            pm_password_textBox.Location = new System.Drawing.Point(16, 332);
            pm_password_textBox.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            pm_password_textBox.Name = "pm_password_textBox";
            pm_password_textBox.Size = new System.Drawing.Size(382, 31);
            pm_password_textBox.TabIndex = 3;
            pm_password_textBox.UseSystemPasswordChar = true;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label7.Location = new System.Drawing.Point(16, 302);
            label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(91, 20);
            label7.TabIndex = 5;
            label7.Text = "Password";
            // 
            // pm_server_textBox
            // 
            pm_server_textBox.Enabled = false;
            pm_server_textBox.Location = new System.Drawing.Point(16, 106);
            pm_server_textBox.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            pm_server_textBox.Name = "pm_server_textBox";
            pm_server_textBox.Size = new System.Drawing.Size(382, 31);
            pm_server_textBox.TabIndex = 0;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label8.Location = new System.Drawing.Point(10, 75);
            label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(64, 20);
            label8.TabIndex = 0;
            label8.Text = "Server";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label9.Location = new System.Drawing.Point(10, 150);
            label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(129, 20);
            label9.TabIndex = 7;
            label9.Text = "Authentication";
            // 
            // pm_login_textBox
            // 
            pm_login_textBox.Enabled = false;
            pm_login_textBox.Location = new System.Drawing.Point(16, 258);
            pm_login_textBox.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            pm_login_textBox.Name = "pm_login_textBox";
            pm_login_textBox.Size = new System.Drawing.Size(382, 31);
            pm_login_textBox.TabIndex = 2;
            // 
            // pm_authentication_comboBox
            // 
            pm_authentication_comboBox.Enabled = false;
            pm_authentication_comboBox.FormattingEnabled = true;
            pm_authentication_comboBox.Items.AddRange(new object[] { "Integrated Authentication", "SQL Server Authentication", "Azure Active Directory - Universal with MFA" });
            pm_authentication_comboBox.Location = new System.Drawing.Point(16, 181);
            pm_authentication_comboBox.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            pm_authentication_comboBox.Name = "pm_authentication_comboBox";
            pm_authentication_comboBox.Size = new System.Drawing.Size(382, 33);
            pm_authentication_comboBox.TabIndex = 1;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label10.Location = new System.Drawing.Point(10, 228);
            label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(55, 20);
            label10.TabIndex = 6;
            label10.Text = "Login";
            // 
            // DatabaseSelect
            // 
            AcceptButton = ok_button;
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = cancel_button;
            ClientSize = new System.Drawing.Size(873, 783);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(ok_button);
            Controls.Add(cancel_button);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "DatabaseSelect";
            ShowIcon = false;
            ShowInTaskbar = false;
            Text = "Database Select";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
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
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox encrypt_Combo;
        private System.Windows.Forms.ComboBox pm_encrypt_Combo;
        private System.Windows.Forms.Label label12;
    }
}