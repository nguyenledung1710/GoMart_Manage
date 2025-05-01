namespace GoMartApplication.DTO
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class GoMart_Manage : DbContext
    {

        public GoMart_Manage()
            : base("name=GoMart_Manage")
        {
           // Database.SetInitializer<GoMart_Manage>(new CreateDB());
        }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<BillDetail> BillDetails { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Configure precision for decimals
            modelBuilder.Entity<Product>()
                        .Property(p => p.ProdPrice)
                        .HasPrecision(18, 2);

            modelBuilder.Entity<BillDetail>()
                        .Property(bd => bd.Price)
                        .HasPrecision(18, 2);
            modelBuilder.Entity<BillDetail>()
                        .Property(bd => bd.Total)
                        .HasPrecision(18, 2);

            modelBuilder.Entity<Bill>()
                        .Property(b => b.TotalAmt)
                        .HasPrecision(18, 2);

            base.OnModelCreating(modelBuilder);
        }
    }
}