using FoodMVCWebApp.Data;
using FoodMVCWebApp.Interfaces;
using FoodMVCWebApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
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

        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<FoodMVCWebAppIdentityDbContext>();

        builder.Services.AddAuthentication().AddGoogle(googleOptions =>
        {
            googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
            googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
        });


        builder.Services.Configure<StaticFilesSettings>(builder.Configuration.GetSection(SettingStrings.StaticFilesSection));
        builder.Services.Configure<BlobStaticFilesSettings>(builder.Configuration.GetSection(SettingStrings.AzureBlobStorageSection));

        builder.Services.AddTransient<IImageService, BlobStorageImageService>();

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



        app.UseAuthentication();
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

