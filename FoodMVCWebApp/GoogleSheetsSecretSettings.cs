using System;
using System.Text.Json.Serialization;

namespace FoodMVCWebApp
{
	public class GoogleSheetsSecretSettings
	{
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("private_key")]
        public string PrivateKey { get; set; }

        [JsonPropertyName("private_key_id")]
        public string PrivateKeyId { get; set; }

        [JsonPropertyName("project_id")]
        public string ProjectId { get; set; }

        [JsonPropertyName("client_email")]
        public string ClientEmail { get; set; }

        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }

        [JsonPropertyName("auth_uri")]
        public string AuthURI { get; set; }

        [JsonPropertyName("token_uri")]
        public string TokenURI { get; set; }

        [JsonPropertyName("auth_provider_x509_cert_url")]
        public string AuthProviderCertURL { get; set; }

        [JsonPropertyName("client_x509_cert_url")]
        public string ClientCertURL { get; set; }

        [JsonPropertyName("universe_domain")]
        public string UniverseDomain { get; set; }
    }
}

