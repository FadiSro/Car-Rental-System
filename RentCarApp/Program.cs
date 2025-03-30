using System;
using System.Windows.Forms;

namespace RentCarApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        public static LogIn frmLogin = null;
        public static Manager frmManager = null;
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            frmLogin = new LogIn();
            Application.Run(frmLogin);
        }
    }
}
