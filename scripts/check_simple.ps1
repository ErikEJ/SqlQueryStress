# Check untranslated text
Write-Host "Checking for untranslated text..." -ForegroundColor Green

Set-Location "e:\source\SqlQueryStress\src\SQLQueryStress"

Get-ChildItem -Name "*Designer.cs" | ForEach-Object {
  Write-Host "`nFile: $_" -ForegroundColor Cyan
    
  $content = Get-Content $_
  $found = $false
    
  for ($i = 0; $i -lt $content.Count; $i++) {
    if ($content[$i] -match 'Text = "' -and $content[$i] -notmatch 'Resources') {
      Write-Host "  Line $($i+1): $($content[$i])" -ForegroundColor Red
      $found = $true
    }
  }
    
  if (-not $found) {
    Write-Host "  OK - All translated" -ForegroundColor Green
  }
}

Write-Host "`nDone!" -ForegroundColor Green
