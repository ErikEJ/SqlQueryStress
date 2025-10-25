using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.IO;

namespace SQLQueryStress
{
    /// <summary>
    /// Helper class to manage SQL connection settings from querysettings.sql file
    /// </summary>
    internal static class ConnectionSettingsHelper
    {
        private static readonly Lazy<string> _querySettings = new Lazy<string>(LoadQuerySettings);

        private const string DefaultSettings = @"
-- Default SQL Server Management Studio query execution settings
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULL_DFLT_ON ON;
SET ANSI_PADDING ON;
SET ANSI_WARNINGS ON;
SET ANSI_NULLS ON;
SET ARITHABORT ON;
SET CONCAT_NULL_YIELDS_NULL ON;";

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
#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
                        command.CommandText = settingsToApply;
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities
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
            return _querySettings.Value;
        }

        /// <summary>
        /// Loads query settings from the querysettings.sql file
        /// </summary>
        private static string LoadQuerySettings()
        {
            var exepath = Environment.ProcessPath;

            // Try to find querysettings.sql in the application directory
            var appPath = Path.GetDirectoryName(exepath);
            var settingsFile = Path.Combine(appPath, "querysettings.sql");

            if (File.Exists(settingsFile))
            {
                return File.ReadAllText(settingsFile);
            }

            throw new FileNotFoundException($"'{settingsFile}' not found in application directory.");

            // return DefaultSettings;
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
