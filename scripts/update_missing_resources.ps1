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
        ///   Looks up a localized string similar to Please wait while background threads are canceled.
        /// </summary>
        internal static string PleaseWaitCancel {
            get {
                return ResourceManager.GetString("PleaseWaitCancel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Database Select.
        /// </summary>
        internal static string DatabaseSelectTitle {
            get {
                return ResourceManager.GetString("DatabaseSelectTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Options.
        /// </summary>
        internal static string OptionsTitle {
            get {
                return ResourceManager.GetString("OptionsTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Parameter.
        /// </summary>
        internal static string ParameterColumn {
            get {
                return ResourceManager.GetString("ParameterColumn", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Datatype.
        /// </summary>
        internal static string DatatypeColumn {
            get {
                return ResourceManager.GetString("DatatypeColumn", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Column.
        /// </summary>
        internal static string ColumnColumn {
            get {
                return ResourceManager.GetString("ColumnColumn", resourceCulture);
            }
        }
        
"@

# Insert new resources
if ($insertIndex -gt -1) {
  $content = $content[0..($insertIndex - 1)] + $newResources + $content[$insertIndex..($content.Count - 1)]
}

# Write back
$content | Out-File "e:\source\SqlQueryStress\src\SQLQueryStress\Properties\Resources.Designer.cs" -Encoding utf8

Write-Host "Resources.Designer.cs updated with missing resources"
