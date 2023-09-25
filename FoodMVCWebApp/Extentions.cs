using System;
using System.IO;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;

namespace FoodMVCWebApp
{
	public static class Extentions
	{
        public static void AddDriveService(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment appEnvironment)
        {
            ClientSecrets clientSecrets = new ClientSecrets
            {
                ClientId = configuration["Authentication:Google:ClientId"], // <- change
                ClientSecret = configuration["Authentication:Google:ClientSecret"] // <- change
            };

            /* set where to save access token. The token stores the user's access and refresh tokens, and is created automatically when the authorization flow completes for the first time. */
            string tokenPath = configuration["GoogleDrive:TokenPath"]; // <- change

            string[] scopes = { DriveService.Scope.Drive };
            UserCredential credential;
            var CSPath = "";
            using (var stream = new FileStream(Path.Combine(CSPath, "client_secret.json"), FileMode.Open, FileAccess.Read))
            {
                string folderPath = "~/Content/";
                string filePath = Path.Combine(folderPath, "DriveServiceCredentials.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(filePath, true)).Result;
            }

            DriveService service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "FoodMVCWebApp"
            });
            services.AddSingleton<DriveService>(service);
        }
    }
}

