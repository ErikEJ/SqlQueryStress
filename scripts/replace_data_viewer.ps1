# Read file
$content = Get-Content "e:\source\SqlQueryStress\src\SQLQueryStress\DataViewer.Designer.cs"

# Add using statement
$content = $content -replace 'namespace SQLQueryStress\r?\n{', 'namespace SQLQueryStress`n{`n    using SQLQueryStress.Properties;'

# Replace text
$content = $content -replace 'this\.Text = "Form2";', 'this.Text = Resources.DataViewerTitle;'

# Write back
$content | Out-File "e:\source\SqlQueryStress\src\SQLQueryStress\DataViewer.Designer.cs" -Encoding utf8

Write-Host "DataViewer.Designer.cs updated"
