using System;
using FoodMVCWebApp.Interfaces;
using FoodMVCWebApp.Models;
using MassTransit;
using static FoodAddressWEbApi.Contracts;

namespace FoodMVCWebApp.Consumers
{
    public class AddressDeletedConsumer : IConsumer<AddressDeleted>
	{
        private readonly IAddressRepository addressRepository;

        public AddressDeletedConsumer(IAddressRepository addressRepository)
        {
            this.addressRepository = addressRepository;
        }

        public async Task Consume(ConsumeContext<AddressDeleted> context)
        {
            var message = context.Message;

            var item = await addressRepository.GetAsync(message.Id);

            if (item == null)
            {
                return;
            }

            await addressRepository.DeleteAsync(message.Id);

        }
    }
}

