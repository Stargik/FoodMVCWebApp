using System;
using FoodMVCWebApp.Interfaces;
using FoodMVCWebApp.Models;
using MassTransit;
using static FoodAddressWEbApi.Contracts;

namespace FoodMVCWebApp.Consumers
{
	public class AddressCreatedConsumer : IConsumer<AddressCreated>
	{
        private readonly IAddressRepository addressRepository;

        public AddressCreatedConsumer(IAddressRepository addressRepository)
		{
            this.addressRepository = addressRepository;
		}

        public async Task Consume(ConsumeContext<AddressCreated> context)
        {
            var message = context.Message;

            var item = await addressRepository.GetAsync(message.Id);

            if (item != null)
            {
                return;
            }

            item = new AddressDTO
            {
                Id = message.Id,
                Name = message.Name,
                Latitude = message.Latitude,
                Longitude = message.Longitude
            };

            await addressRepository.CreateAsync(item);
        }
    }
}

