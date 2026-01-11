# Read file
$content = Get-Content "e:\source\SqlQueryStress\src\SQLQueryStress\ParamWindow.Designer.cs"

# Add using statement
$content = $content -replace 'namespace SQLQueryStress\r?\n{', 'namespace SQLQueryStress`n{`n    using SQLQueryStress.Properties;'

# Replace text
$content = $content -replace 'this\.label1\.Text = "Parameter Query";', 'this.label1.Text = Resources.ParameterQuery;'
$content = $content -replace 'this\.getColumnsButton\.Text = "Get Columns";', 'this.getColumnsButton.Text = Resources.GetColumns;'
$content = $content -replace 'this\.okButton\.Text = "OK";', 'this.okButton.Text = Resources.ParamWindowOK;'
$content = $content -replace 'this\.cancelButton\.Text = "Cancel";', 'this.cancelButton.Text = Resources.ParamWindowCancel;'
$content = $content -replace 'this\.label2\.Text = "Parameter Mappings";', 'this.label2.Text = Resources.ParameterMappings;'
$content = $content -replace 'this\.database_button\.Text = "Database";', 'this.database_button.Text = Resources.ParamWindowDatabase;'
$content = $content -replace 'this\.Text = "Parameter Substitution";', 'this.Text = Resources.ParamWindowTitle;'

# Write back
$content | Out-File "e:\source\SqlQueryStress\src\SQLQueryStress\ParamWindow.Designer.cs" -Encoding utf8

Write-Host "ParamWindow.Designer.cs updated"
