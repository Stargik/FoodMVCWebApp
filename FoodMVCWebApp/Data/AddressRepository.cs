using System;
using FoodMVCWebApp.Interfaces;
using FoodMVCWebApp.Models;
using MongoDB.Driver;

namespace FoodMVCWebApp.Data
{
    public class AddressRepository : IAddressRepository
    {
        private const string collectionName = "addresses";
        private readonly IMongoCollection<AddressDTO> mongoCollection;
        private readonly FilterDefinitionBuilder<AddressDTO> filterBuilder = Builders<AddressDTO>.Filter;

        public AddressRepository(IMongoDatabase mongoDatabase)
        {
            this.mongoCollection = mongoDatabase.GetCollection<AddressDTO>(collectionName);
        }

        public async Task<IReadOnlyCollection<AddressDTO>> GetAllAsync()
        {
            return await mongoCollection.Find(filterBuilder.Empty).ToListAsync();
        }

        public async Task<AddressDTO> GetAsync(int id)
        {
            FilterDefinition<AddressDTO> filter = filterBuilder.Eq(a => a.Id, id);
            return await mongoCollection.Find(filter).FirstOrDefaultAsync();
        }


        public async Task CreateAsync(AddressDTO address)
        {
            await mongoCollection.InsertOneAsync(address);
        }

        public async Task UpdateAsync(AddressDTO address)
        {
            FilterDefinition<AddressDTO> filter = filterBuilder.Eq(a => a.Id, address.Id);
            await mongoCollection.ReplaceOneAsync(filter, address);
        }

        public async Task DeleteAsync(int id)
        {
            FilterDefinition<AddressDTO> filter = filterBuilder.Eq(a => a.Id, id);
            await mongoCollection.DeleteOneAsync(filter);
        }
    }
}

