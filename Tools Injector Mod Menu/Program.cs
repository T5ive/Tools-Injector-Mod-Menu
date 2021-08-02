using System;
using System.Threading;
using System.Windows.Forms;

namespace Tools_Injector_Mod_Menu
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (new Mutex(true, "Tools Injector Mod Menu", out var createdNew))
            {
                if (createdNew)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new FrmMain());
                }
                else
                {
                    MessageBox.Show(@"[Detected] - There is same tools that still running on your windows!", "TFive", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    Application.Exit();
                }
            }
        }
    }
}
