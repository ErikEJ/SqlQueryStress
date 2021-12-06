using CommandLine;
using CommandLine.Text;
using SQLQueryStress;
using System;
using System.Collections.Generic;
using System.IO;

namespace SqlQueryStressCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new CommandLineOptions();
            var parserResult = Parser.Default.ParseArguments<CommandLineOptions>(args);

            parserResult
                .WithParsed(options => Run(options))
                .WithNotParsed(errors => DisplayHelp(parserResult, errors));
        }

        private static void Run(CommandLineOptions options)
        {
            if (File.Exists(options.SettingsFile))
            {
                var settings = OpenConfigFile(options.SettingsFile);

                var runner = new LoadRunner(settings, options);

                runner.Run();
            }
            else
            {
                throw new ArgumentException($"Settings file could not be found: {options.SettingsFile}");
            }
        }

        private static QueryStressSettings OpenConfigFile(string fileName)
        {
            try
            {
                var contents = File.ReadAllText(fileName);
                return JsonSerializer.ReadToObject<QueryStressSettings>(contents);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unable to read settings file {fileName}: {ex.Message}");
            }

            return null;
        }

        private static void DisplayHelp<T>(ParserResult<T> result, IEnumerable<Error> errors)
        {
            if (errors.IsVersion())
            {
                Console.WriteLine(HelpText.AutoBuild(result));
            }
            else if (errors.IsHelp())
            {
                var helpText = HelpText.AutoBuild(result, help =>
                {
                    help.AdditionalNewLineAfterOption = true;
                    help.AddPreOptionsLine("Check for updates at: https://github.com/ErikEJ/SqlQueryStress");
                    help.AddPreOptionsLine("Sample usage:");
                    help.AddPreOptionsLine("sqlstresscmd -s saved.json -t 32");
                    return HelpText.DefaultParsingErrorsHandler(result, help);
                }, e => e);

                Console.WriteLine(helpText);
            }
            else // Parser error
            {
                Console.WriteLine(HelpText.AutoBuild(result));
            }
        }
    }
}
