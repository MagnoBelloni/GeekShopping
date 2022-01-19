using GeekShopping.IdentityServer.Configuration;
using GeekShopping.IdentityServer.ModelEntity;
using GeekShopping.IdentityServer.ModelEntity.Context;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace GeekShopping.IdentityServer.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly PostgressContext _context;
        private readonly UserManager<ApplicationUser> _user;
        private readonly RoleManager<IdentityRole> _role;

        public DbInitializer(PostgressContext context, UserManager<ApplicationUser> user, RoleManager<IdentityRole> role)
        {
            _context = context;
            _user = user;
            _role = role;
        }

        public async void Initialize()
        {
            if (await _role.FindByNameAsync(IdentityConfiguration.Admin) != null)
                return;

            await _role.CreateAsync(new IdentityRole(IdentityConfiguration.Admin));
            await _role.CreateAsync(new IdentityRole(IdentityConfiguration.Client));

            ApplicationUser admin = new ApplicationUser()
            {
                UserName = "magno-admin",
                Email = "magno-admin@email.com.br",
                EmailConfirmed = true,
                PhoneNumber = "+55 (11) 12345-6789",
                FirstName = "Magno",
                LastName =  "Admin"
            };

            await _user.CreateAsync(admin, "Magno123$");
            await _user.AddToRoleAsync(admin, IdentityConfiguration.Admin);

            await _user.AddClaimsAsync(admin, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, $"{admin.FirstName} {admin.LastName}"),
                new Claim(JwtClaimTypes.GivenName, admin.FirstName),
                new Claim(JwtClaimTypes.FamilyName, admin.LastName),
                new Claim(JwtClaimTypes.Role, IdentityConfiguration.Admin)
            }); 
            
            ApplicationUser client = new ApplicationUser()
            {
                UserName = "magno-client",
                Email = "magno-client@email.com.br",
                EmailConfirmed = true,
                PhoneNumber = "+55 (11) 12345-6789",
                FirstName = "Magno",
                LastName =  "Client"
            };

            await _user.CreateAsync(client, "Magno123$");
            await _user.AddToRoleAsync(client, IdentityConfiguration.Client);

            await _user.AddClaimsAsync(client, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, $"{client.FirstName} {client.LastName}"),
                new Claim(JwtClaimTypes.GivenName, client.FirstName),
                new Claim(JwtClaimTypes.FamilyName, client.LastName),
                new Claim(JwtClaimTypes.Role, IdentityConfiguration.Client)
            });
        }
    }
}
