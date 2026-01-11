# Find untranslated hardcoded text
Write-Host "Finding untranslated text..." -ForegroundColor Green

Set-Location "e:\source\SqlQueryStress\src\SQLQueryStress"

# Check Designer files
Get-ChildItem -Name "*Designer.cs" | ForEach-Object {
  Write-Host "`nFile: $_" -ForegroundColor Cyan
    
  $lines = Get-Content $_
  $found = $false
    
  for ($i = 0; $i -lt $lines.Count; $i++) {
    if ($lines[$i] -match '\.Text = "' -and $lines[$i] -notmatch 'Resources\.') {
      Write-Host "  Line $($i+1): $($lines[$i].Trim())" -ForegroundColor Red
      $found = $true
    }
  }
    
  if (-not $found) {
    Write-Host "  OK - All translated" -ForegroundColor Green
  }
}

Write-Host "`nDone!" -ForegroundColor Green
