using CommandLine;
using CommandLine.Text;
using SQLQueryStress.Properties;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

namespace SQLQueryStress
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            var options = new CommandLineOptions();
            var parserResult = Parser.Default.ParseArguments<CommandLineOptions>(args);

            parserResult
                .WithParsed(options => Run(options))
                .WithNotParsed(errors => DisplayHelp(parserResult, errors));
        }

        private static void Run(CommandLineOptions options)
        {
            AppDomain.CurrentDomain.AssemblyResolve += OnResolveAssembly;
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using var FormMain = new FormMain(options)
            {
                StartPosition = FormStartPosition.CenterScreen
            };
            Application.Run(FormMain);
        }

        private static void DisplayHelp<T>(ParserResult<T> result, IEnumerable<Error> errors)
        {
            NativeMethods.AttachParentConsole();

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
                    help.AddPreOptionsLine("SqlQueryStress -c saved.SqlStress -u -t 32 -d sqldb.perfenv.mycompany.com -r results.csv");
                    return HelpText.DefaultParsingErrorsHandler(result, help);
                }, e => e);

                Console.WriteLine(helpText);
            }
            else // Parser error
            {
                Console.WriteLine(HelpText.AutoBuild(result));
            }

            Console.WriteLine(Resources.EnterToContinue); //TODO It might be replaced with send enter key, but it needs implementing more native methods
        }

        private static Assembly OnResolveAssembly(object sender, ResolveEventArgs args)
        {
            var dllName = new AssemblyName(args.Name).Name + ".dll";
            var assem = Assembly.GetExecutingAssembly();
            var resourceName = Array.Find(assem.GetManifestResourceNames(), rn => rn.EndsWith(dllName, StringComparison.OrdinalIgnoreCase));
            if (resourceName == null) return null; // Not found, maybe another handler will find it

            using var stream = assem.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                return null;
            }
            var assemblyData = new byte[stream.Length];
            stream.ReadExactly(assemblyData);
            return Assembly.Load(assemblyData);
        }
    }
}
