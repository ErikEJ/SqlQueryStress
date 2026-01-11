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
        ///   Looks up a localized string similar to Parameter Query.
        /// </summary>
        internal static string ParameterQuery {
            get {
                return ResourceManager.GetString("ParameterQuery", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Get Columns.
        /// </summary>
        internal static string GetColumns {
            get {
                return ResourceManager.GetString("GetColumns", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to OK.
        /// </summary>
        internal static string ParamWindowOK {
            get {
                return ResourceManager.GetString("ParamWindowOK", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cancel.
        /// </summary>
        internal static string ParamWindowCancel {
            get {
                return ResourceManager.GetString("ParamWindowCancel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Parameter Mappings.
        /// </summary>
        internal static string ParameterMappings {
            get {
                return ResourceManager.GetString("ParameterMappings", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Database.
        /// </summary>
        internal static string ParamWindowDatabase {
            get {
                return ResourceManager.GetString("ParamWindowDatabase", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Parameter Substitution.
        /// </summary>
        internal static string ParamWindowTitle {
            get {
                return ResourceManager.GetString("ParamWindowTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Form2.
        /// </summary>
        internal static string DataViewerTitle {
            get {
                return ResourceManager.GetString("DataViewerTitle", resourceCulture);
            }
        }
        
"@

# Insert new resources
if ($insertIndex -gt -1) {
  $content = $content[0..($insertIndex - 1)] + $newResources + $content[$insertIndex..($content.Count - 1)]
}

# Write back
$content | Out-File "e:\source\SqlQueryStress\src\SQLQueryStress\Properties\Resources.Designer.cs" -Encoding utf8

Write-Host "Resources.Designer.cs updated with final resources"
