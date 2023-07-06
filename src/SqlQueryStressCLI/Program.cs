using CommandLine;
using CommandLine.Text;
using SQLQueryStress;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

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
            if (options.ExtractSample)
            {
                ExtractSample();
            }

            if (File.Exists(options.SettingsFile))
            {
                var settings = OpenConfigFile(options.SettingsFile);

                var runner = new LoadRunner(settings, options);

                runner.Run();
            }
            else
            {
                Console.Error.WriteLine($"Settings file could not be found, or not specified: {options.SettingsFile}");
            }
        }

        private static void ExtractSample()
        {
            var names = Assembly.GetEntryAssembly().GetManifestResourceNames();

            using (var stream = Assembly.GetEntryAssembly().GetManifestResourceStream("sqlstresscmd.sample.json"))
            using (var reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                var path = Path.GetFullPath("sample.json");
                if (!File.Exists(path))
                {
                    File.WriteAllText(path, result, System.Text.Encoding.UTF8);
                    Console.WriteLine($"{path} saved.");
                }
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
            if (errors != null && errors.Any() && errors.First().StopsProcessing)
            {
                return;
            }
            else if (errors.IsVersion())
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
