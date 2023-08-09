using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Vts.DAL;
public class VtsContext : IdentityDbContext
{

    public DbSet<User> Users => Set<User>();
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<OrderStatus> OrdersStatus { get; set; }
    public VtsContext(DbContextOptions<VtsContext> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder Builder)
    {
        base.OnModelCreating(Builder);

        Builder.Entity<IdentityUser>().UseTptMappingStrategy();
        Builder.Entity<OrderDetail>().HasKey(x =>new { x.Id ,x.ItemId});


    }


   
}

