namespace GoMartApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConfigureTotalAmtPrecision : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Bills", "TotalAmt", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Bills", "TotalAmt", c => c.Double(nullable: false));
        }
    }
}
