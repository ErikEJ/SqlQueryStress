﻿using CommandLine;
using CommandLine.Text;

namespace SQLQueryStress
{
    public class CommandLineOptions
    {
        private readonly HeadingInfo _headingInfo = new HeadingInfo(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());

        [Option("c", null,
                HelpText = "File name of saved session settings\r\n")]
        public string SettingsFile { get; set; } = string.Empty;

        [Option("u", null,
                HelpText = "Run unattended (start, run settings file and quit)")]
        public bool Unattended { get; set; } = false;

        [Option("r", null,
                HelpText = "Autosave results to specified file")]
        public string ResultsAutoSaveFileName { get; set; } = string.Empty;

        [Option("d", null,
                HelpText = "Database Server")]
        public string DbServer { get; set; } = string.Empty;

        [Option("t", null,
                HelpText = "Number of threads in unattended mode")]
        public int NumberOfThreads { get; set; } = -1;

        [HelpOption("?", null,
                HelpText = "Display this help screen")]
        public string GetUsage()
        {
            HelpText help = new HelpText(_headingInfo)
            {
                Copyright = new CopyrightInfo("Adam Machanic", 2006)
            };
            help.AddPreOptionsLine("Check for updates at: https://github.com/ErikEJ/SqlQueryStress");
            help.AddPreOptionsLine(string.Empty);
            help.AddPreOptionsLine("Sample usage:");
            help.AddPreOptionsLine("SqlQueryStress -c saved.SqlStress -u -t 32 -d sqldb.perfenv.mycompany.com -r results.csv");
            help.AddOptions(this);
            return help;
        }
    }


}
