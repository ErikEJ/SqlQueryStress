# 讀取檔案內容
$content = Get-Content "e:\source\SqlQueryStress\src\SQLQueryStress\FormMain.Designer.cs"

# 替換所有文字
$content = $content -replace 'this\.label1\.Text = "Query";', 'this.label1.Text = Resources.QueryLabel;'
$content = $content -replace 'this\.fileToolStripMenuItem\.Text = "File";', 'this.fileToolStripMenuItem.Text = Resources.FileMenu;'
$content = $content -replace 'this\.optionsToolStripMenuItem\.Text = "Options";', 'this.optionsToolStripMenuItem.Text = Resources.OptionsMenu;'
$content = $content -replace 'this\.saveSettingsToolStripMenuItem\.Text = "Save Settings";', 'this.saveSettingsToolStripMenuItem.Text = Resources.SaveSettings;'
$content = $content -replace 'this\.loadSettingsToolStripMenuItem\.Text = "Load Settings";', 'this.loadSettingsToolStripMenuItem.Text = Resources.LoadSettings;'
$content = $content -replace 'this\.saveBenchMarkToolStripMenuItem\.Text = "Save BenchMark";', 'this.saveBenchMarkToolStripMenuItem.Text = Resources.SaveBenchmark;'
$content = $content -replace 'this\.toCsvToolStripMenuItem\.Text = "To Csv";', 'this.toCsvToolStripMenuItem.Text = Resources.ToCsv;'
$content = $content -replace 'this\.toTextToolStripMenuItem\.Text = "To Text";', 'this.toTextToolStripMenuItem.Text = Resources.ToText;'
$content = $content -replace 'this\.toClipboardToolStripMenuItem\.Text = "To Clipboard";', 'this.toClipboardToolStripMenuItem.Text = Resources.ToClipboard;'
$content = $content -replace 'this\.exitToolStripMenuItem\.Text = "Exit";', 'this.exitToolStripMenuItem.Text = Resources.Exit;'
$content = $content -replace 'this\.helpToolStripMenuItem\.Text = "Help";', 'this.helpToolStripMenuItem.Text = Resources.HelpMenu;'
$content = $content -replace 'this\.aboutToolStripMenuItem\.Text = "About";', 'this.aboutToolStripMenuItem.Text = Resources.About;'
$content = $content -replace 'this\.go_button\.Text = "GO";', 'this.go_button.Text = Resources.GoButton;'
$content = $content -replace 'this\.label2\.Text = "Number of Iterations";', 'this.label2.Text = Resources.NumberOfIterations;'
$content = $content -replace 'this\.label3\.Text = "Number of Threads";', 'this.label3.Text = Resources.NumberOfThreads;'
$content = $content -replace 'this\.cancel_button\.Text = "Cancel";', 'this.cancel_button.Text = Resources.CancelButton;'
$content = $content -replace 'this\.label4\.Text = "Iterations Completed";', 'this.label4.Text = Resources.IterationsCompleted;'
$content = $content -replace 'this\.label5\.Text = "Client Seconds/Iteration \(Avg\)";', 'this.label5.Text = Resources.ClientSecondsIteration;'
$content = $content -replace 'this\.label6\.Text = "Progress";', 'this.label6.Text = Resources.Progress;'
$content = $content -replace 'this\.label7\.Text = "Total Exceptions";', 'this.label7.Text = Resources.TotalExceptions;'
$content = $content -replace 'this\.label8\.Text = "Elapsed Time";', 'this.label8.Text = Resources.ElapsedTime;'
$content = $content -replace 'this\.database_button\.Text = "Database";', 'this.database_button.Text = Resources.Database;'
$content = $content -replace 'this\.activeThreads_label\.Text = "Active Threads";', 'this.activeThreads_label.Text = Resources.ActiveThreads;'
$content = $content -replace 'this\.label9\.Text = "CPU Seconds/Iteration \(Avg\)";', 'this.label9.Text = Resources.CPUSecondsIteration;'
$content = $content -replace 'this\.label12\.Text = "Logical Reads/Iteration \(Avg\)";', 'this.label12.Text = Resources.LogicalReadsIteration;'
$content = $content -replace 'this\.label10\.Text = "Actual Seconds/Iteration \(Avg\)";', 'this.label10.Text = Resources.ActualSecondsIteration;'
$content = $content -replace 'this\.param_button\.Text = "Parameter Substitution";', 'this.param_button.Text = Resources.ParameterSubstitution;'
$content = $content -replace 'this\.btnFreeCache\.Text = "Free Cache";', 'this.btnFreeCache.Text = Resources.FreeCache;'
$content = $content -replace 'this\.btnCleanBuffer\.Text = "Clean Buffers";', 'this.btnCleanBuffer.Text = Resources.CleanBuffers;'
$content = $content -replace 'this\.label11\.Text = "Delay between queries \(ms\)";', 'this.label11.Text = Resources.DelayBetweenQueries;'

# 寫回檔案
$content | Out-File "e:\source\SqlQueryStress\src\SQLQueryStress\FormMain.Designer.cs" -Encoding utf8

Write-Host "替換完成！"
