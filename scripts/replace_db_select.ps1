# Read file
$content = Get-Content "e:\source\SqlQueryStress\src\SQLQueryStress\DatabaseSelect.Designer.cs"

# Add using statement
$content = $content -replace 'namespace SQLQueryStress\r?\n{', 'namespace SQLQueryStress`n{`n    using SQLQueryStress.Properties;'

# Replace text
$content = $content -replace 'this\.label1\.Text = "Server";', 'this.label1.Text = Resources.Server;'
$content = $content -replace 'this\.label2\.Text = "Password";', 'this.label2.Text = Resources.Password;'
$content = $content -replace 'this\.label3\.Text = "Login";', 'this.label3.Text = Resources.Login;'
$content = $content -replace 'this\.label4\.Text = "Authentication";', 'this.label4.Text = Resources.Authentication;'
$content = $content -replace 'this\.cancel_button\.Text = "Cancel";', 'this.cancel_button.Text = Resources.DatabaseSelectCancel;'
$content = $content -replace 'this\.test_button\.Text = "Test Connection";', 'this.test_button.Text = Resources.TestConnection;'
$content = $content -replace 'this\.ok_button\.Text = "OK";', 'this.ok_button.Text = Resources.DatabaseSelectOK;'
$content = $content -replace 'this\.label5\.Text = "Default Database";', 'this.label5.Text = Resources.DefaultDatabase;'
$content = $content -replace 'this\.groupBox1\.Text = "Main Load Settings";', 'this.groupBox1.Text = Resources.MainLoadSettings;'
$content = $content -replace 'this\.database_list_autorefresh\.Text = "auto-refresh";', 'this.database_list_autorefresh.Text = Resources.AutoRefresh;'
$content = $content -replace 'this\.label13\.Text = "Additional Parameters";', 'this.label13.Text = Resources.AdditionalParameters;'
$content = $content -replace 'this\.label11\.Text = "Encrypt";', 'this.label11.Text = Resources.Encrypt;'
$content = $content -replace 'this\.appintent_check\.Text = "Application Intent";', 'this.appintent_check.Text = Resources.ApplicationIntent;'
$content = $content -replace 'this\.groupBox2\.Text = "Parameterization Settings";', 'this.groupBox2.Text = Resources.ParameterizationSettings;'
$content = $content -replace 'this\.label14\.Text = "Additional Parameters";', 'this.label14.Text = Resources.AdditionalParameters;'
$content = $content -replace 'this\.label12\.Text = "Encrypt";', 'this.label12.Text = Resources.Encrypt;'
$content = $content -replace 'this\.pm_appintent_check\.Text = "Application Intent";', 'this.pm_appintent_check.Text = Resources.ApplicationIntent;'
$content = $content -replace 'this\.pm_test_button\.Text = "Test Connection";', 'this.pm_test_button.Text = Resources.TestConnection;'
$content = $content -replace 'this\.shareSettings_checkBox\.Text = "Share Connection Settings";', 'this.shareSettings_checkBox.Text = Resources.ShareConnectionSettings;'
$content = $content -replace 'this\.label6\.Text = "Default Database";', 'this.label6.Text = Resources.DefaultDatabase;'
$content = $content -replace 'this\.label7\.Text = "Password";', 'this.label7.Text = Resources.Password;'
$content = $content -replace 'this\.label8\.Text = "Server";', 'this.label8.Text = Resources.Server;'
$content = $content -replace 'this\.label9\.Text = "Authentication";', 'this.label9.Text = Resources.Authentication;'
$content = $content -replace 'this\.label10\.Text = "Login";', 'this.label10.Text = Resources.Login;'
$content = $content -replace 'this\.trustcert_check\.Text = "Trust Server Certificate";', 'this.trustcert_check.Text = Resources.TrustServerCertificate;'
$content = $content -replace 'this\.pm_trustcert_check\.Text = "Trust Server Certificate";', 'this.pm_trustcert_check.Text = Resources.TrustServerCertificate;'

# Write back
$content | Out-File "e:\source\SqlQueryStress\src\SQLQueryStress\DatabaseSelect.Designer.cs" -Encoding utf8

Write-Host "DatabaseSelect.Designer.cs updated"
