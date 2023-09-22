using System;
using FoodMVCWebApp.Interfaces;
using FoodMVCWebApp.Models;
using MassTransit;
using static FoodAddressWEbApi.Contracts;

namespace FoodMVCWebApp.Consumers
{
    public class AddressUpdatedConsumer : IConsumer<AddressUpdated>
	{
        private readonly IAddressRepository addressRepository;

        public AddressUpdatedConsumer(IAddressRepository addressRepository)
        {
            this.addressRepository = addressRepository;
        }

        public async Task Consume(ConsumeContext<AddressUpdated> context)
        {
            var message = context.Message;

            var item = await addressRepository.GetAsync(message.Id);

            if (item == null)
            {
                item = new AddressDTO
                {
                    Id = message.Id,
                    Name = message.Name,
                    Latitude = message.Latitude,
                    Longitude = message.Longitude
                };

                await addressRepository.CreateAsync(item);
            }
            else
            {
                item.Name = message.Name;
                item.Latitude = message.Latitude;
                item.Longitude = message.Longitude;

                await addressRepository.UpdateAsync(item);
            }
        }
    }
}

