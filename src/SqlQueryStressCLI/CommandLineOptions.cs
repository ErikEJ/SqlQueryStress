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



        [Option('r', "results",
                HelpText = "Autosave results to the specified file (CSV or JSON)")]
        public string ResultsAutoSaveFileName { get; set; } = string.Empty;

        [Option('f', "format",
                HelpText = "Result output format: csv or json (defaults to file extension)")]
        public string ResultFormat { get; set; } = string.Empty;
    }
}