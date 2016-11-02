using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Generic;

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

            Dictionary<string, string> arguments = GetCommandLineArguments(args);

            string configFileName;
            bool unattendedMode;
            int numThreads;

            configFileName = arguments["C"];
            try { unattendedMode = Convert.ToBoolean(arguments["U"]); } catch { unattendedMode = false; }
            try { numThreads = Convert.ToInt32(arguments["T"]); } catch { numThreads = 0; }

            var f = new Form1(configFileName, unattendedMode, numThreads);
            f.StartPosition = FormStartPosition.CenterScreen;
            Application.Run(f);
        }

        private static Dictionary<string, string> GetCommandLineArguments (string[] args)
        {
            /* allow commind line aguments, named or position based
             * > SQLQueryStress.exe "fast.sqlstress" true 75
             * > SQLQueryStress.exe -C "fast.sqlstress" -U true -T 75
             * > SQLQueryStress.exe -C "fast.sqlstress" -U -T 75
            */
            string[] ParamOrder = { "C", "U", "T" };
            string key, value = "";
            bool isFlag = false;
            Dictionary<string, string> arguments = new Dictionary<string, string>();
            // set default values
            arguments["C"] = "";        // config file
            arguments["U"] = "false";   // run in unattended mode
            arguments["T"] = "0";       // number of threads - overrides value in config file

            if (args.Length > 0)
            {
                // see if using named arguments
                if (args[0].Length > 0 && (args[0].Substring(0, 1) == "-" || args[0].Substring(0, 1) == "/"))
                {
                    // using name based
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (args[i].Substring(0, 1) == "-" || args[i].Substring(0, 1) == "/")
                        {
                            key = args[i].Substring(1).ToUpper();
                            value = "";
                            isFlag = true;

                            // peak at next argument to see if value or another key
                            if (i + 1 < args.Length)
                            {
                                value = args[i + 1];
                                isFlag = false;
                                if (value.Substring(0, 1) == "-" || value.Substring(0, 1) == "/")
                                    // it could be another key, if it's valid, and the current key can be a flag
                                    if (ParamOrder.Contains(value.Substring(1)) && key == "U")
                                        isFlag = true;
                            }
                                
                            if (isFlag)
                            {
                                // for valid flag parameters we apply true value
                                if (key == "U") value = "true";
                            }
                            // since it's not a flag, skip the next argument as we got the value from it
                            else i++;    

                            // if we got a valid key, add to our arguments
                            if (ParamOrder.Contains(key)) arguments[key] = value;
                        }
                    }
                }
                else
                {
                    // using position based, so assign in order
                    for (int i = 0; (i < args.Length && i < ParamOrder.Length); i++)
                        arguments[ParamOrder[i]] = args[i];

                }

            }

            return arguments;
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