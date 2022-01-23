using Microsoft.EntityFrameworkCore;

namespace GeekShopping.CartAPI.Model.Context
{
    public class PostgressContext : DbContext
    {
        public PostgressContext() { }
        public PostgressContext(DbContextOptions<PostgressContext> options) : base(options) { }
        
        public DbSet<Product> Products { get; set; }
        public DbSet<CartHeader> CartHeaders { get; set; }
        public DbSet<CartDetail> CartDetails { get; set; }
    }
}
