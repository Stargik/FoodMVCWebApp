using FoodMVCWebApp.Data;
using FoodMVCWebApp.Interfaces;
using FoodMVCWebApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using FoodMVCWebApp.Areas.Identity.Data;
using FoodMVCWebApp.Areas.Identity.Data;

namespace FoodMVCWebApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        builder.Services.AddDbContext<FoodDbContext>(option =>
            option.UseSqlServer(builder.Configuration.GetConnectionString(SettingStrings.FoodDbConnection))
        );

        builder.Services.AddDbContext<FoodMVCWebAppIdentityDbContext>(options => options.UseSqlServer(
            builder.Configuration.GetConnectionString(SettingStrings.IdentityFoodDbConnection)
        ));

        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<FoodMVCWebAppIdentityDbContext>();

        builder.Services.Configure<StaticFilesSettings>(builder.Configuration.GetSection(SettingStrings.StaticFilesSection));
        builder.Services.AddTransient<IImageService, FilesystemImageService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.MigrateDatabase();
        app.MigrateIdentityDatabase();
        app.MapRazorPages();
        app.Run();
    }
}

