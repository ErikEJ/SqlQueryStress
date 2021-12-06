using CommandLine;

namespace SQLQueryStress
{
    public class CommandLineOptions
    {
        [Option('s', "settingsFile",
                HelpText = "File name of saved session settings\r\n", 
                Required = true)]
        public string SettingsFile { get; set; } = string.Empty;

        [Option('d', "dbserver",
                HelpText = "Database Server")]
        public string DbServer { get; set; } = string.Empty;

        [Option('t', "threads",
                HelpText = "Number of threads in unattended mode")]
        public int NumberOfThreads { get; set; } = 1;

        //TODO Implement
        [Option('r', "results",
        HelpText = "Autosave results to specified file")]
        public string ResultsAutoSaveFileName { get; set; } = string.Empty;
    }
}