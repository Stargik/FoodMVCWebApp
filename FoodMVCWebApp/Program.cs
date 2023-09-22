using FoodMVCWebApp.Data;
using FoodMVCWebApp.Interfaces;
using FoodMVCWebApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using FoodMVCWebApp.Areas.Identity.Data;
using FoodMVCWebApp.Clients;
using Microsoft.AspNetCore.Authentication.Google;
using MassTransit;
using System.Reflection;
using MongoDB.Driver;

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

        builder.Services.Configure<StaticFilesSettings>(builder.Configuration.GetSection(SettingStrings.StaticFilesSection));
        builder.Services.Configure<BlobStaticFilesSettings>(builder.Configuration.GetSection(SettingStrings.AzureBlobStorageSection));
        builder.Services.Configure<GoogleMapsSettings>(builder.Configuration.GetSection(SettingStrings.GoogleMaps));

        builder.Services.AddTransient<IImageService, BlobStorageImageService>();
        builder.Services.AddTransient<IMapsService, GoogleMapsService>();

        builder.Services.AddSingleton(serviceProvider =>
        {
            var mongoDbSettings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
            var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
            var projectSettings = builder.Configuration.GetSection("ProjectSettings").Get<ProjectSettings>();
            return mongoClient.GetDatabase(projectSettings.ProjectName);
        });

        builder.Services.AddSingleton<IAddressRepository, AddressRepository>();

        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<FoodMVCWebAppIdentityDbContext>();

        builder.Services.AddAuthentication().AddGoogle(googleOptions =>
        {
            googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
            googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
        });

        builder.Services.AddHttpClient<AddressesClient>(client =>
        {
            client.BaseAddress = new Uri(builder.Configuration["Clients:Addresses"]);
        });

        builder.Services.AddMassTransit(x =>
        {
            x.AddConsumers(Assembly.GetEntryAssembly());

            x.UsingRabbitMq((context, configurator) =>
            {
                var rabbitMQSettings = builder.Configuration.GetSection("RabbitMQSettings").Get<RabbitMQSettings>();
                var projectSettings = builder.Configuration.GetSection("ProjectSettings").Get<ProjectSettings>();
                configurator.Host(rabbitMQSettings.Host, (cfg) =>
                {
                    cfg.Username(rabbitMQSettings.User);
                    cfg.Password(rabbitMQSettings.Password);
                });
                configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(false));
            });
        });



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

