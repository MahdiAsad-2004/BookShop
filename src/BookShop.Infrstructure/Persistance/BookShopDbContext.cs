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
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<EBook> EBooks { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<PasswordHistory> PasswordHistories { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<User_Permission> User_Permissions { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Product_Discount> Product_Discounts { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleClaim> RoleClaims { get; set; }
        public DbSet<Translator> Translators { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<User_Role> User_Roles { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }



        #endregion

        public class MuDbQuery
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }




        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=.;Database=BookShopDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;");
        //}

    }
}
