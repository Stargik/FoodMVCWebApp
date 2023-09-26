using System;
using System.Text.Json;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Microsoft.Extensions.Options;

namespace FoodMVCWebApp
{
	public class GoogleSheetsHelper
	{
        public SheetsService Service { get; set; }
        private readonly GoogleSheetsSecretSettings googleSheetsSecretSettings;
        static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        public GoogleSheetsHelper(IOptions<GoogleSheetsSecretSettings> googleSheetsSecretSettings)
        {
            this.googleSheetsSecretSettings = googleSheetsSecretSettings.Value;
            InitializeService();
        }
        private void InitializeService()
        {
            var credential = GetCredentialsFromFile();
            Service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = googleSheetsSecretSettings.ProjectId
            });
        }
        private GoogleCredential GetCredentialsFromFile()
        {
            var googleSheetsSecretSettingsJson = JsonSerializer.Serialize(googleSheetsSecretSettings);
            GoogleCredential credential = GoogleCredential.FromJson(googleSheetsSecretSettingsJson).CreateScoped(Scopes);
            return credential;
        }
    }
}

