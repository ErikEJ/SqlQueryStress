using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
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
            AppDomain.CurrentDomain.AssemblyResolve += OnResolveAssembly;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var options = new CommandLineOptions();

            var parserResult = Parser.Default.ParseArguments<CommandLineOptions>(args);

            parserResult
                .WithParsed(options => Run(options))
                .WithNotParsed(errors => DisplayHelp(parserResult, errors));
        }

        private static void Run(CommandLineOptions options)
        {
            using var form1 = new Form1(options)
            {
                StartPosition = FormStartPosition.CenterScreen
            };
            Application.Run(form1);
        }

        private static void DisplayHelp<T>(ParserResult<T> result, IEnumerable<Error> errors)
        {
            AttachConsole(-1);

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
            else
            {
                var builder = SentenceBuilder.Create();
                var errorMessages = HelpText.RenderParsingErrorsTextAsLines(result, builder.FormatError, builder.FormatMutuallyExclusiveSetErrors, 1);

                var errorList = errorMessages.Select(msg => new ArgumentException(msg)).ToList();

                foreach (var message in errorMessages)
                {
                    Console.WriteLine(message);
                }
            }
        }

        private static Assembly OnResolveAssembly(object sender, ResolveEventArgs args)
        {
            var dllName = new AssemblyName(args.Name).Name + ".dll";
            var assem = Assembly.GetExecutingAssembly();
            var resourceName = assem.GetManifestResourceNames().FirstOrDefault(rn => rn.EndsWith(dllName, StringComparison.OrdinalIgnoreCase));
            if (resourceName == null) return null; // Not found, maybe another handler will find it

            using var stream = assem.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                return null;
            }
            var assemblyData = new byte[stream.Length];
            stream.Read(assemblyData, 0, assemblyData.Length);
            return Assembly.Load(assemblyData);
        }

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        private static extern bool AttachConsole(int processId);
    }
}