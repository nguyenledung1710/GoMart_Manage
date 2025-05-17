using GoMartApplication.DTO;
using System;
using System.Data.Entity;
using System.Drawing;
using System.Windows.Forms;

namespace GoMartApplication
{
    static class Program
    {
        public static Size DefaultFormSize;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Database.SetInitializer(
              new MigrateDatabaseToLatestVersion<
                GoMart_Manage,
                GoMartApplication.Migrations.Configuration
              >()
            );

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var ctx = new GoMart_Manage())
                ctx.Database.Initialize(force: true);
            var main = new frmMain();
            DefaultFormSize = main.Size;
            Application.Run(new Form1());
        }
    }
}
