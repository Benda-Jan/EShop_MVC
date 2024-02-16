using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TestEShopMacNet7.Data
{
    public class ApplicationDbContext : IdentityDbContext<Models.Account.ExtendedUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public override DbSet<Models.Account.ExtendedUser> Users { get; set; } = default!;
        public DbSet<Models.Product.Category> Categories { get; set; } = default!;
        public DbSet<Models.Product.Brand> Brands { get; set; } = default!;
        public DbSet<Models.Product.Product> Products { get; set; } = default!;
        public DbSet<Models.Cart.CartItem> CartItems { get; set; } = default!;
    }
}

