namespace GoMartApplication.DTO
{
    using System.Data.Entity;

    public class GoMart_Manage : DbContext
    {

        public GoMart_Manage()
            : base("name=GoMart_Manage")
        {
        }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<BillDetail> BillDetails { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
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