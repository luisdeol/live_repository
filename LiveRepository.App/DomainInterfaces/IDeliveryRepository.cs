using LiveRepository.App.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LiveRepository.App.DomainInterfaces
{
    public interface IDeliveryRepository
    {
        Task<List<Delivery>> GetAllAsync();
        Task<Delivery> GetByIdAsync(int id);
        Task<int> AddAsync(Delivery delivery);
        Task Complete(int id);
    }
}
