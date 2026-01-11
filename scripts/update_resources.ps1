# Read Resources.Designer.cs
$content = Get-Content "e:\source\SqlQueryStress\src\SQLQueryStress\Properties\Resources.Designer.cs"

# Find the position before the QueryStressIcon property
$insertIndex = -1
for ($i = 0; $i -lt $content.Count; $i++) {
  if ($content[$i] -match "internal static System\.Drawing\.Icon QueryStressIcon") {
    $insertIndex = $i
    # Find the start of the previous method
    for ($j = $i - 1; $j -ge 0; $j--) {
      if ($content[$j] -match "/// <summary>") {
        $insertIndex = $j
        break
      }
    }
    break
  }
}

# Define new resource properties
$newResources = @"
        /// <summary>
        ///   Looks up a localized string similar to Query.
        /// </summary>
        internal static string QueryLabel {
            get {
                return ResourceManager.GetString("QueryLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to File.
        /// </summary>
        internal static string FileMenu {
            get {
                return ResourceManager.GetString("FileMenu", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Options.
        /// </summary>
        internal static string OptionsMenu {
            get {
                return ResourceManager.GetString("OptionsMenu", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Save Settings.
        /// </summary>
        internal static string SaveSettings {
            get {
                return ResourceManager.GetString("SaveSettings", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Load Settings.
        /// </summary>
        internal static string LoadSettings {
            get {
                return ResourceManager.GetString("LoadSettings", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Save BenchMark.
        /// </summary>
        internal static string SaveBenchmark {
            get {
                return ResourceManager.GetString("SaveBenchmark", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to To Csv.
        /// </summary>
        internal static string ToCsv {
            get {
                return ResourceManager.GetString("ToCsv", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to To Text.
        /// </summary>
        internal static string ToText {
            get {
                return ResourceManager.GetString("ToText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to To Clipboard.
        /// </summary>
        internal static string ToClipboard {
            get {
                return ResourceManager.GetString("ToClipboard", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Exit.
        /// </summary>
        internal static string Exit {
            get {
                return ResourceManager.GetString("Exit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Help.
        /// </summary>
        internal static string HelpMenu {
            get {
                return ResourceManager.GetString("HelpMenu", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to About.
        /// </summary>
        internal static string About {
            get {
                return ResourceManager.GetString("About", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to GO.
        /// </summary>
        internal static string GoButton {
            get {
                return ResourceManager.GetString("GoButton", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Number of Iterations.
        /// </summary>
        internal static string NumberOfIterations {
            get {
                return ResourceManager.GetString("NumberOfIterations", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Number of Threads.
        /// </summary>
        internal static string NumberOfThreads {
            get {
                return ResourceManager.GetString("NumberOfThreads", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cancel.
        /// </summary>
        internal static string CancelButton {
            get {
                return ResourceManager.GetString("CancelButton", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Iterations Completed.
        /// </summary>
        internal static string IterationsCompleted {
            get {
                return ResourceManager.GetString("IterationsCompleted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Client Seconds/Iteration (Avg).
        /// </summary>
        internal static string ClientSecondsIteration {
            get {
                return ResourceManager.GetString("ClientSecondsIteration", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Progress.
        /// </summary>
        internal static string Progress {
            get {
                return ResourceManager.GetString("Progress", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Total Exceptions.
        /// </summary>
        internal static string TotalExceptions {
            get {
                return ResourceManager.GetString("TotalExceptions", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Elapsed Time.
        /// </summary>
        internal static string ElapsedTime {
            get {
                return ResourceManager.GetString("ElapsedTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Database.
        /// </summary>
        internal static string Database {
            get {
                return ResourceManager.GetString("Database", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Active Threads.
        /// </summary>
        internal static string ActiveThreads {
            get {
                return ResourceManager.GetString("ActiveThreads", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ....
        /// </summary>
        internal static string ExceptionsButton {
            get {
                return ResourceManager.GetString("ExceptionsButton", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CPU Seconds/Iteration (Avg).
        /// </summary>
        internal static string CPUSecondsIteration {
            get {
                return ResourceManager.GetString("CPUSecondsIteration", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Logical Reads/Iteration (Avg).
        /// </summary>
        internal static string LogicalReadsIteration {
            get {
                return ResourceManager.GetString("LogicalReadsIteration", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Actual Seconds/Iteration (Avg).
        /// </summary>
        internal static string ActualSecondsIteration {
            get {
                return ResourceManager.GetString("ActualSecondsIteration", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Parameter Substitution.
        /// </summary>
        internal static string ParameterSubstitution {
            get {
                return ResourceManager.GetString("ParameterSubstitution", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Free Cache.
        /// </summary>
        internal static string FreeCache {
            get {
                return ResourceManager.GetString("FreeCache", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Clean Buffers.
        /// </summary>
        internal static string CleanBuffers {
            get {
                return ResourceManager.GetString("CleanBuffers", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Delay between queries (ms).
        /// </summary>
        internal static string DelayBetweenQueries {
            get {
                return ResourceManager.GetString("DelayBetweenQueries", resourceCulture);
            }
        }
        
"@

# Insert new resources
if ($insertIndex -gt -1) {
  $content = $content[0..($insertIndex - 1)] + $newResources + $content[$insertIndex..($content.Count - 1)]
}

# Write back
$content | Out-File "e:\source\SqlQueryStress\src\SQLQueryStress\Properties\Resources.Designer.cs" -Encoding utf8

Write-Host "Resources.Designer.cs updated"
