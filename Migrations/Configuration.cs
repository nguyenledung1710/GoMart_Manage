namespace GoMartApplication.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<GoMartApplication.DTO.GoMart_Manage>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;   // ← bật cho phép mất dữ liệu
            ContextKey = "GoMartApplication.DTO.GoMart_Manage";
        }

        protected override void Seed(GoMartApplication.DTO.GoMart_Manage context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
