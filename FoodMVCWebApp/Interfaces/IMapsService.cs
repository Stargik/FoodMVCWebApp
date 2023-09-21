using System;
using FoodMVCWebApp.Models;

namespace FoodMVCWebApp.Interfaces
{
	public interface IMapsService
	{
        public Task<string> GetKKey();
        public Task<IEnumerable<AddressDTO>> GetAddresses();
    }
}

