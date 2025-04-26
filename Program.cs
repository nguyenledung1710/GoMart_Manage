using GoMartApplication.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoMartApplication
{
    static class Program
    {
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

            // Ép EF apply migration ngay lập tức
            using (var ctx = new GoMart_Manage())
                ctx.Database.Initialize(force: true);

            Application.Run(new Form1());
        }
    }
}
