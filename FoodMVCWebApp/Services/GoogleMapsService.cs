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
        private readonly IAddressRepository addressRepository;

        public GoogleMapsService(IOptions<GoogleMapsSettings> mapsSettings, AddressesClient addressesClient, IAddressRepository addressRepository)
		{
			this.mapsSettings = mapsSettings.Value;
            this.addressesClient = addressesClient;
            this.addressRepository = addressRepository;
		}

        public async Task<IEnumerable<AddressDTO>> GetAddresses()
        {
            var addressList = await addressRepository.GetAllAsync();
            return addressList;
        }

        public async Task<string> GetKey()
        {
            return mapsSettings.ApiKey;
        }
    }
}

