using System;
using FoodMVCWebApp.Models;

namespace FoodMVCWebApp.Clients
{
	public class AddressesClient
	{
		private readonly HttpClient httpClient;

		public AddressesClient(HttpClient httpClient)
		{
			this.httpClient = httpClient;
		}

		public async Task<IReadOnlyCollection<AddressDTO>> GetAddressesAsync()
		{
			var addresses = await httpClient.GetFromJsonAsync<IReadOnlyCollection<AddressDTO>>("api/Addresses");
			return addresses;
		}
	}
}

