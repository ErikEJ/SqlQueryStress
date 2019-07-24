using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using CommandLine;

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
            ICommandLineParser parser = new CommandLineParser();
            var writer = new StringWriter();
            parser.ParseArguments(args, options, writer);
            
            if (writer.GetStringBuilder().Length > 0)
            {
                MessageBox.Show(writer.GetStringBuilder().ToString());
            }
            else
            {
                var f = new Form1(options)
                {
                    StartPosition = FormStartPosition.CenterScreen
                };
                Application.Run(f);
            }
        }

        private static Assembly OnResolveAssembly(object sender, ResolveEventArgs args)
        {
            var dllName = new AssemblyName(args.Name).Name + ".dll";
            var assem = Assembly.GetExecutingAssembly();
            var resourceName = assem.GetManifestResourceNames().FirstOrDefault(rn => rn.EndsWith(dllName));
            if (resourceName == null) return null; // Not found, maybe another handler will find it
            using (var stream = assem.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    return null;
                }
                var assemblyData = new byte[stream.Length];
                stream.Read(assemblyData, 0, assemblyData.Length);
                return Assembly.Load(assemblyData);
            }
        }
    }
}