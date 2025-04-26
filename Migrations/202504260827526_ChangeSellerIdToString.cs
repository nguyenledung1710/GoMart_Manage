namespace GoMartApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeSellerIdToString : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Bills", "SellerID", "dbo.Sellers");
            DropIndex("dbo.Bills", new[] { "SellerID" });
            // 1. Thêm cột tạm
            AddColumn("dbo.Sellers", "NewSellerId", c => c.String(nullable: false, maxLength: 128));
            // 2. Copy data
            Sql("UPDATE dbo.Sellers SET NewSellerId = SellerId");
            // 3. Xóa PK & cột cũ
            DropPrimaryKey("dbo.Sellers");
            DropColumn("dbo.Sellers", "SellerId");
            // 4. Rename cột tạm
            RenameColumn("dbo.Sellers", "NewSellerId", "SellerId");
            AddPrimaryKey("dbo.Sellers", "SellerId");
            // 5. Cập nhật bảng Bills
            AlterColumn("dbo.Bills", "SellerID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Bills", "SellerID");
            AddForeignKey("dbo.Bills", "SellerID", "dbo.Sellers", "SellerId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bills", "Admin_AdminId", c => c.String(maxLength: 50));
            DropForeignKey("dbo.Bills", "SellerID", "dbo.Sellers");
            DropIndex("dbo.Bills", new[] { "SellerID" });
            DropPrimaryKey("dbo.Sellers");
            AlterColumn("dbo.Sellers", "SellerId", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Bills", "SellerID", c => c.Int(nullable: false));
            DropColumn("dbo.Admins", "address");
            AddPrimaryKey("dbo.Sellers", "SellerId");
            CreateIndex("dbo.Bills", "Admin_AdminId");
            CreateIndex("dbo.Bills", "SellerID");
            AddForeignKey("dbo.Bills", "SellerID", "dbo.Sellers", "SellerId", cascadeDelete: true);
            AddForeignKey("dbo.Bills", "Admin_AdminId", "dbo.Admins", "AdminId");
        }
    }
}
