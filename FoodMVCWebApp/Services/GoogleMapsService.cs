using System;
using FoodMVCWebApp.Clients;
using FoodMVCWebApp.Interfaces;
using FoodMVCWebApp.Models;
using Microsoft.Extensions.Options;

namespace FoodMVCWebApp.Services
{
    public class GoogleMapsService : IMapsService
	{
        private readonly GoogleMapsSettings mapsSettings;
        private readonly AddressesClient addressesClient;

        public GoogleMapsService(IOptions<GoogleMapsSettings> mapsSettings, AddressesClient addressesClient)
		{
			this.mapsSettings = mapsSettings.Value;
            this.addressesClient = addressesClient;
		}

        public async Task<IEnumerable<AddressDTO>> GetAddresses()
        {
            var addressList = await addressesClient.GetAddressesAsync();
            return addressList;
        }

        public async Task<string> GetKKey()
        {
            return mapsSettings.ApiKey;
        }
    }
}

