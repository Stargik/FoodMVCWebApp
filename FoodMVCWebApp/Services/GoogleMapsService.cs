using System;
using FoodMVCWebApp.Interfaces;
using FoodMVCWebApp.Models;
using Microsoft.Extensions.Options;

namespace FoodMVCWebApp.Services
{
    public class GoogleMapsService : IMapsService
	{
        private readonly GoogleMapsSettings mapsSettings;

        public GoogleMapsService(IOptions<GoogleMapsSettings> mapsSettings)
		{
			this.mapsSettings = mapsSettings.Value;
		}

        public async Task<IEnumerable<AddressDTO>> GetAddresses()
        {
            var addressList = new List<AddressDTO>
            {
            new AddressDTO
            {
                Id = 1,
                Name = "Adderess1",
                Latitude = "50.516684",
                Longitude = "30.619365"
            },
            new AddressDTO
            {
                Id = 1,
                Name = "Adderess1",
                Latitude = "50.519161",
                Longitude = "30.610138"
            },
            new AddressDTO
            {
                Id = 1,
                Name = "Adderess1",
                Latitude = "50.520072",
                Longitude = "30.611265"
            }
            };
            return addressList;
        }

        public async Task<string> GetKKey()
        {
            return mapsSettings.ApiKey;
        }
    }
}

