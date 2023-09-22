using FoodMVCWebApp.Models;

namespace FoodMVCWebApp.Interfaces
{
    public interface IAddressRepository
    {
        Task CreateAsync(AddressDTO address);
        Task DeleteAsync(int id);
        Task<IReadOnlyCollection<AddressDTO>> GetAllAsync();
        Task<AddressDTO> GetAsync(int id);
        Task UpdateAsync(AddressDTO address);
    }
}