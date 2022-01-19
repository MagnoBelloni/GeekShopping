using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.IdentityServer.ModelEntity.Context
{
    public class PostgressContext : IdentityDbContext<ApplicationUser>
    {
        public PostgressContext(DbContextOptions<PostgressContext> options) : base(options) { }

    }
}
