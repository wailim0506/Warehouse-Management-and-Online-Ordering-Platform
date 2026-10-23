using System;
using System.Windows.Forms;
using LMCIS.controller.Utilities;
using LMCIS.System_page;

namespace LMCIS
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                new Log();
                Configuration.CreateAvatarFolder();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                using (var login = new Login())
                    Application.Run(login);
            }
            catch (Exception error)
            {
                Log.LogException(error, "LMCIS");
                MessageBox.Show("The application could not start. Please check the application log.",
                    "Legend Motor Company Integrated System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
