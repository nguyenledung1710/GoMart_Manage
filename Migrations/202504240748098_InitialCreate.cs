namespace GoMartApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        AdminId = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false, maxLength: 100),
                        FullName = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.AdminId);
            
            CreateTable(
                "dbo.Bills",
                c => new
                    {
                        Bill_ID = c.String(nullable: false, maxLength: 50),
                        SellerID = c.Int(nullable: false),
                        SellDate = c.DateTime(nullable: false),
                        TotalAmt = c.Double(nullable: false),
                        Admin_AdminId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Bill_ID)
                .ForeignKey("dbo.Sellers", t => t.SellerID, cascadeDelete: true)
                .ForeignKey("dbo.Admins", t => t.Admin_AdminId)
                .Index(t => t.SellerID)
                .Index(t => t.Admin_AdminId);
            
            CreateTable(
                "dbo.BillDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Bill_ID = c.String(maxLength: 50),
                        ProdID = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Qty = c.Int(nullable: false),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bills", t => t.Bill_ID)
                .ForeignKey("dbo.Products", t => t.ProdID, cascadeDelete: true)
                .Index(t => t.Bill_ID)
                .Index(t => t.ProdID);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProdID = c.Int(nullable: false, identity: true),
                        ProdName = c.String(nullable: false, maxLength: 200),
                        ProdCatID = c.Int(nullable: false),
                        ProdPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProdQty = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProdID)
                .ForeignKey("dbo.Categories", t => t.ProdCatID, cascadeDelete: true)
                .Index(t => t.ProdCatID);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CatID = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(nullable: false, maxLength: 100),
                        CategoryDesc = c.String(),
                    })
                .PrimaryKey(t => t.CatID);
            
            CreateTable(
                "dbo.Sellers",
                c => new
                    {
                        SellerId = c.Int(nullable: false, identity: true),
                        SellerName = c.String(nullable: false, maxLength: 100),
                        SellerAge = c.Int(nullable: false),
                        SellerPhone = c.String(maxLength: 20),
                        SellerPass = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.SellerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bills", "Admin_AdminId", "dbo.Admins");
            DropForeignKey("dbo.Bills", "SellerID", "dbo.Sellers");
            DropForeignKey("dbo.BillDetails", "ProdID", "dbo.Products");
            DropForeignKey("dbo.Products", "ProdCatID", "dbo.Categories");
            DropForeignKey("dbo.BillDetails", "Bill_ID", "dbo.Bills");
            DropIndex("dbo.Products", new[] { "ProdCatID" });
            DropIndex("dbo.BillDetails", new[] { "ProdID" });
            DropIndex("dbo.BillDetails", new[] { "Bill_ID" });
            DropIndex("dbo.Bills", new[] { "Admin_AdminId" });
            DropIndex("dbo.Bills", new[] { "SellerID" });
            DropTable("dbo.Sellers");
            DropTable("dbo.Categories");
            DropTable("dbo.Products");
            DropTable("dbo.BillDetails");
            DropTable("dbo.Bills");
            DropTable("dbo.Admins");
        }
    }
}
