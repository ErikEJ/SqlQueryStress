using CommandLine;

namespace SQLQueryStress
{
    public class CommandLineOptions
    {
        [Option('c', "settingsFile",
                HelpText = "File name of saved session settings\r\n")]
        public string SettingsFile { get; set; } = string.Empty;

        [Option('u', "unattended",
                HelpText = "Run unattended (start, run settings file and quit)")]
        public bool Unattended { get; set; } = false;

        [Option('r', "results",
                HelpText = "Autosave results to specified file")]
        public string ResultsAutoSaveFileName { get; set; } = string.Empty;

        [Option('d', "dbserver",
                HelpText = "Database Server")]
        public string DbServer { get; set; } = string.Empty;

        [Option('t', "threads",
                HelpText = "Number of threads in unattended mode")]
        public int NumberOfThreads { get; set; } = -1;
    }
}