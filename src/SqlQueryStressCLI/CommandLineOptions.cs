using CommandLine;
using System.IO;

namespace SQLQueryStress
{
    public class CommandLineOptions
    {
        [Option('s', "settingsFile",
                HelpText = "File name of saved session settings")]
        public string SettingsFile { get; set; } = string.Empty;

        [Option('d', "dbserver",
                HelpText = "Database Server")]
        public string DbServer { get; set; } = string.Empty;

        [Option('t', "threads",
        HelpText = "Number of threads, default 1")]
        public int? NumberOfThreads { get; set; }

        [Option('i', "input",
                HelpText = "Path to .sql script to execute")]
        public FileInfo Input { get; set; }

        [Option('x', "xtract",
                HelpText = "Extract sample.json file to current folder")]
        public bool ExtractSample { get; set; }



        //TODO Implement
        [Option('r', "results",
        HelpText = "Autosave results to specified file")]
        public string ResultsAutoSaveFileName { get; set; } = string.Empty;
    }
}