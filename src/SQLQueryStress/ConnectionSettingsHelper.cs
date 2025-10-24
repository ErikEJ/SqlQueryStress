using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.IO;

namespace SQLQueryStress
{
    /// <summary>
    /// Helper class to manage SQL connection settings from querysettings.sql file
    /// </summary>
    public static class ConnectionSettingsHelper
    {
        private static string _querySettings;
        private static readonly object _lock = new object();

        /// <summary>
        /// Event handler to apply query settings when a connection is opened
        /// </summary>
        public static void OnStateChange(object sender, StateChangeEventArgs args)
        {
            if (args.OriginalState == ConnectionState.Closed
                && args.CurrentState == ConnectionState.Open)
            {
                var settingsToApply = GetQuerySettings();
                if (!string.IsNullOrEmpty(settingsToApply))
                {
                    using (SqlCommand command = ((SqlConnection)sender).CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = settingsToApply;
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        /// <summary>
        /// Gets the query settings from querysettings.sql file
        /// </summary>
        private static string GetQuerySettings()
        {
            if (_querySettings == null)
            {
                lock (_lock)
                {
                    if (_querySettings == null)
                    {
                        _querySettings = LoadQuerySettings();
                    }
                }
            }
            return _querySettings;
        }

        /// <summary>
        /// Loads query settings from the querysettings.sql file
        /// </summary>
        private static string LoadQuerySettings()
        {
            try
            {
                // Try to find querysettings.sql in the application directory
                var appPath = AppDomain.CurrentDomain.BaseDirectory;
                var settingsFile = Path.Combine(appPath, "querysettings.sql");

                if (File.Exists(settingsFile))
                {
                    return File.ReadAllText(settingsFile);
                }

                // Try one level up (for development scenarios)
                var parentPath = Directory.GetParent(appPath)?.FullName;
                if (parentPath != null)
                {
                    settingsFile = Path.Combine(parentPath, "querysettings.sql");
                    if (File.Exists(settingsFile))
                    {
                        return File.ReadAllText(settingsFile);
                    }
                }

                // Try several levels up to find the root directory
                var currentDir = appPath;
                for (int i = 0; i < 5; i++)
                {
                    var parentDir = Directory.GetParent(currentDir);
                    if (parentDir == null) break;
                    
                    currentDir = parentDir.FullName;
                    settingsFile = Path.Combine(currentDir, "querysettings.sql");
                    if (File.Exists(settingsFile))
                    {
                        return File.ReadAllText(settingsFile);
                    }
                }

                // If file not found, return default SSMS-like settings
                return @"-- Default SQL Server Management Studio query execution settings
SET QUOTED_IDENTIFIER ON
SET ANSI_NULL_DFLT_ON ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
SET ANSI_NULLS ON
SET ARITHABORT ON
SET CONCAT_NULL_YIELDS_NULL ON";
            }
            catch
            {
                // If any error occurs, return default settings
                return @"-- Default SQL Server Management Studio query execution settings
SET QUOTED_IDENTIFIER ON
SET ANSI_NULL_DFLT_ON ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
SET ANSI_NULLS ON
SET ARITHABORT ON
SET CONCAT_NULL_YIELDS_NULL ON";
            }
        }

        /// <summary>
        /// Attaches the StateChange event handler to a SqlConnection
        /// </summary>
        public static void AttachEventHandler(SqlConnection connection)
        {
            if (connection != null)
            {
                connection.StateChange += OnStateChange;
            }
        }
    }
}
