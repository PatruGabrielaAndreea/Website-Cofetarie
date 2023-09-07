using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectLab.Models.Entities;

namespace ProjectLab.Data
{
    public class ApplicationDBContext : IdentityDbContext<User>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        {
        }

        #region Required
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CartProductItem>()
                .HasKey(cpi => new
                {
                    cpi.ProductId,
                    cpi.UserId
                });

            modelBuilder.Entity<OrderProductItem>()
               .HasKey(opi => new
               {
                   opi.ProductId,
                   opi.OrderId
               });
        }
        #endregion

        public DbSet<ProductType> ProductType { get; set; }

        public DbSet<OrderProductItem> OrderProduct { get; set; }

        public DbSet<Order> Order { get; set; }

        public DbSet<OrderStatus> OrderStatus { get; set; }

        public DbSet<CartProductItem> ShoppingCartProduct { get; set; }

        public DbSet<Product> Product { get; set; }
    }
}
