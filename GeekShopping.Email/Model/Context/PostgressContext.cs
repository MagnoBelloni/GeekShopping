using Microsoft.EntityFrameworkCore;

namespace GeekShopping.Email.Model.Context
{
    public class PostgressContext : DbContext
    {
        public PostgressContext() { }
        public PostgressContext(DbContextOptions<PostgressContext> options) : base(options) { }
        
        public DbSet<EmailLog> Emails { get; set; }
    }
}
