namespace GoMartApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAddressToAdmin : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Admins", "Address", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Admins", "Address");
        }
    }
}
