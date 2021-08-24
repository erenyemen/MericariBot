using Gecko;
using MericariBot.WinForms;
using System;
using System.Windows.Forms;

namespace MericariBot
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            



            Xpcom.Initialize("Firefox64");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmLogin());
        }
    }
}
