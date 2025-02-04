using Elixir.Entities;
using Microsoft.EntityFrameworkCore;

namespace Elixir.DATA
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }


        public DbSet<AppUser> Users { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<StoreAddres> StoreAddress { get; set; }
        public DbSet<UserStore> UserStores { get; set; }
        public DbSet<ProductComment> ProductComments { get; set; }
        public DbSet<StoreCategory> StoreCategories { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Notifications> Notifications { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ProductInOrder> ProductInOrders { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<UserSave> UserSaves { get; set; }
        public DbSet<UserCategory> UserCategories { get; set; }
        public DbSet<CommonQuestion> CommonQuestions { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<ReportProduct> ReportProducts { get; set; }







        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            AppUser[] x = {
                new() {
                    CreationDate=DateTime.UtcNow,
                    FullName="Admin",
                    UserName="Admin",
                    Role=UserRole.SuperAdmin,
                    Password=BCrypt.Net.BCrypt.HashPassword("12345678"),
                    PhoneNumber=["01234567891"],
                    Id=Guid.NewGuid()

                }

            };
            modelBuilder.Entity<AppUser>().HasData(x);


            // seed data 

        }

    }
}