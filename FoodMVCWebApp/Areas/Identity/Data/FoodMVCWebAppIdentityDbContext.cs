using FoodMVCWebApp.Areas.Identity.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FoodMVCWebApp.Areas.Identity.Data;

public class FoodMVCWebAppIdentityDbContext : IdentityDbContext<IdentityUser>
{
    public FoodMVCWebAppIdentityDbContext(DbContextOptions<FoodMVCWebAppIdentityDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new RoleConfiguration());
    }
}
