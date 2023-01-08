using Chetvyorochka.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chetvyorochka.DAL
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Basket> Baskets { get; set; } = null!;
        public DbSet<ProductType> ProductTypes { get; set; } = null!;

        public ApplicationContext(DbContextOptions<ApplicationContext> options) :
            base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(t => t.Login);
            modelBuilder.Entity<User>().Property(t=>t.Login).HasColumnType("varchar(20)");
            modelBuilder.Entity<User>().Property(t => t.Name).HasColumnType("varchar(20)");
            modelBuilder.Entity<User>().Property(t => t.LastName).HasColumnType("varchar(20)");
            modelBuilder.Entity<User>().Property(t => t.UserType).HasColumnType("smallint");
            modelBuilder.Entity<User>().Property(t => t.MoneyCount).HasColumnType("decimal(10,4)");
            modelBuilder.Entity<User>().ToTable(t => t.HasCheckConstraint("CK_Users_MoneyCount", "\"MoneyCount\" >= 0"));
            modelBuilder.Entity<User>().Property(t => t.Password).HasColumnType("varchar(256)");

            modelBuilder.Entity<ProductType>().HasKey(t => t.Id);
            modelBuilder.Entity<ProductType>().Property(t => t.Id).HasColumnType("integer").ValueGeneratedOnAdd();
            modelBuilder.Entity<ProductType>().Property(t => t.Name).HasColumnType("varchar(50)");

            modelBuilder.Entity<Product>().HasKey(t => t.Id);
            modelBuilder.Entity<Product>().Property(t => t.Id).HasColumnType("integer").ValueGeneratedOnAdd();
            modelBuilder.Entity<Product>().Property(t => t.Name).HasColumnType("varchar(50)");
            modelBuilder.Entity<Product>().Property(t => t.Description).HasColumnType("varchar(50)");
            modelBuilder.Entity<Product>().Property(t => t.ProductTypeId).HasColumnType("integer");
            modelBuilder.Entity<Product>().Property(t => t.Price).HasColumnType("decimal(10,4)");
            modelBuilder.Entity<Product>().ToTable(t => t.HasCheckConstraint("CK_Products_Price", "\"Price\" > 0"));
            modelBuilder.Entity<Product>().Property(t => t.Count).HasColumnType("integer");
            modelBuilder.Entity<Product>().ToTable(t => t.HasCheckConstraint("CK_Products_Count", "\"Count\" >= 0"));

            modelBuilder.Entity<Basket>().HasKey(t => new { t.UserLogin, t.ProductId });
            modelBuilder.Entity<Basket>().Property(t => t.UserLogin).HasColumnType("varchar(20)");
            modelBuilder.Entity<Basket>().Property(t => t.ProductId).HasColumnType("integer");
            modelBuilder.Entity<Basket>().Property(t => t.ProductCount).HasColumnType("smallint");
            modelBuilder.Entity<Basket>().ToTable(t=>t.HasCheckConstraint("CK_Baskets_ProductCount", "\"ProductCount\" > 0"));
        }
    }
}
