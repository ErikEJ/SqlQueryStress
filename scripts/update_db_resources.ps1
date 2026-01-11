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
        ///   Looks up a localized string similar to Server.
        /// </summary>
        internal static string Server {
            get {
                return ResourceManager.GetString("Server", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Password.
        /// </summary>
        internal static string Password {
            get {
                return ResourceManager.GetString("Password", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Login.
        /// </summary>
        internal static string Login {
            get {
                return ResourceManager.GetString("Login", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Authentication.
        /// </summary>
        internal static string Authentication {
            get {
                return ResourceManager.GetString("Authentication", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cancel.
        /// </summary>
        internal static string DatabaseSelectCancel {
            get {
                return ResourceManager.GetString("DatabaseSelectCancel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Test Connection.
        /// </summary>
        internal static string TestConnection {
            get {
                return ResourceManager.GetString("TestConnection", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to OK.
        /// </summary>
        internal static string DatabaseSelectOK {
            get {
                return ResourceManager.GetString("DatabaseSelectOK", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Default Database.
        /// </summary>
        internal static string DefaultDatabase {
            get {
                return ResourceManager.GetString("DefaultDatabase", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Main Load Settings.
        /// </summary>
        internal static string MainLoadSettings {
            get {
                return ResourceManager.GetString("MainLoadSettings", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to auto-refresh.
        /// </summary>
        internal static string AutoRefresh {
            get {
                return ResourceManager.GetString("AutoRefresh", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Additional Parameters.
        /// </summary>
        internal static string AdditionalParameters {
            get {
                return ResourceManager.GetString("AdditionalParameters", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Encrypt.
        /// </summary>
        internal static string Encrypt {
            get {
                return ResourceManager.GetString("Encrypt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Application Intent.
        /// </summary>
        internal static string ApplicationIntent {
            get {
                return ResourceManager.GetString("ApplicationIntent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Parameterization Settings.
        /// </summary>
        internal static string ParameterizationSettings {
            get {
                return ResourceManager.GetString("ParameterizationSettings", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Share Connection Settings.
        /// </summary>
        internal static string ShareConnectionSettings {
            get {
                return ResourceManager.GetString("ShareConnectionSettings", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Trust Server Certificate.
        /// </summary>
        internal static string TrustServerCertificate {
            get {
                return ResourceManager.GetString("TrustServerCertificate", resourceCulture);
            }
        }
        
"@

# Insert new resources
if ($insertIndex -gt -1) {
  $content = $content[0..($insertIndex - 1)] + $newResources + $content[$insertIndex..($content.Count - 1)]
}

# Write back
$content | Out-File "e:\source\SqlQueryStress\src\SQLQueryStress\Properties\Resources.Designer.cs" -Encoding utf8

Write-Host "Resources.Designer.cs updated with DatabaseSelect resources"
