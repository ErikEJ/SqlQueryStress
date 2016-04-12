using System;
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
            AppDomain.CurrentDomain.AssemblyResolve += OnResolveAssembly;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var f = args.Length > 0 ? new Form1(args[0]) : new Form1();

            f.StartPosition = FormStartPosition.CenterScreen;
            Application.Run(f);
        }

        private static Assembly OnResolveAssembly(object sender, ResolveEventArgs args)
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SQLQueryStress.Resources.ICSharpCode.AvalonEdit.dll"))
            {
                if (stream == null)
                    return null;
                var assemblyRawBytes = new byte[stream.Length];
                stream.Read(assemblyRawBytes, 0, assemblyRawBytes.Length);
                return Assembly.Load(assemblyRawBytes);
            }
        }
    }
}