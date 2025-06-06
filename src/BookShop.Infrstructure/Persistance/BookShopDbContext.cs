﻿using BookShop.Domain.Common.Entity;
using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BookShop.Infrastructure.Persistance
{
    public class BookShopDbContext : DbContext
    {
        #region constructor

        public BookShopDbContext(DbContextOptions<BookShopDbContext> dbContextOptions) : base(dbContextOptions)
        {
          
        }

        #endregion


        #region tables 

        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Author_Book> Author_Books { get; set; }
        public DbSet<Author_EBook> Author_EBooks { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<EBook> EBooks { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<PasswordHistory> PasswordHistories { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Product_Discount> Product_Discounts { get; set; }
        //public DbSet<Product_Category> Product_Categories { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Role> Roles { get; set; }
        //public DbSet<RoleClaim> RoleClaims { get; set; }
        public DbSet<Translator> Translators { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<User_Permission> User_Permissions { get; set; }
        //public DbSet<User_Role> User_Roles { get; set; }
        public DbSet<RefreshToken> UserTokens { get; set; }
        //public DbSet<UserClaim> UserClaims { get; set; }

        #endregion


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                modelBuilder.Entity(entityType.ClrType).Property<byte[]>("RowVersion").IsRowVersion();
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(string) && property.Name.EndsWith("By"))
                    {
                        property.SetColumnType("VarChar(36)");
                    }
                    //if (property.ClrType == typeof(byte[]) && property.Name == ("RowVersion"))
                    //{
                    //    property.SetColumnType("RowVersion");
                        
                    //}
                }
            }

            //foreach (var entityType in modelBuilder.Model.GetEntityTypes().Where(e => typeof(Entity<>).IsAssignableFrom(e.ClrType)))
            //{
            //    modelBuilder.Entity(entityType.ClrType)
            //        .Property<byte[]>("RowVersion")
            //        .IsRowVersion()
            //        .IsConcurrencyToken();
            //}
        }




        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=.;Database=BookShopDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;");
        //}

    }
}
