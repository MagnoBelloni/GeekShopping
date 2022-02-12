using Microsoft.EntityFrameworkCore;

namespace GeekShopping.OrderAPI.Model.Context
{
    public class PostgressContext : DbContext
    {
        public PostgressContext() { }
        public PostgressContext(DbContextOptions<PostgressContext> options) : base(options) { }
        
        public DbSet<OrderHeader> Headers { get; set; }
        public DbSet<OrderDetail> Details { get; set; }
    }
}
