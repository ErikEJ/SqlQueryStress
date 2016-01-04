using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SQLQueryStress
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            Form1 f = null;

            if (args.Length > 0)
                f = new Form1(args[0]);
            else
                f = new Form1();

            f.StartPosition = FormStartPosition.CenterScreen;
            Application.Run(f);
        }
    }
}