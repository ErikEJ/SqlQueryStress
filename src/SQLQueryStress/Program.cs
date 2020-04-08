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

            CommandLineOptions options = new CommandLineOptions();
            ICommandLineParser parser = new CommandLineParser();
            StringWriter writer = new StringWriter();
            parser.ParseArguments(args, options, writer);

            if (writer.GetStringBuilder().Length > 0)
            {
                MessageBox.Show(writer.GetStringBuilder().ToString());
            }
            else
            {
                Form1 f = new Form1(options)
                {
                    StartPosition = FormStartPosition.CenterScreen
                };
                Application.Run(f);
            }
        }

        private static Assembly OnResolveAssembly(object sender, ResolveEventArgs args)
        {
            string dllName = new AssemblyName(args.Name).Name + ".dll";
            Assembly assem = Assembly.GetExecutingAssembly();
            string resourceName = assem.GetManifestResourceNames().FirstOrDefault(rn => rn.EndsWith(dllName));
            if (resourceName == null) return null; // Not found, maybe another handler will find it
            using (Stream stream = assem.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    return null;
                }
                byte[] assemblyData = new byte[stream.Length];
                stream.Read(assemblyData, 0, assemblyData.Length);
                return Assembly.Load(assemblyData);
            }
        }
    }
}