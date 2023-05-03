using System.Collections.Generic;
using System.Threading.Tasks;

namespace NiteDeliveryService.Shared.DAL.Interfaces
{
    public interface IBaseRepository<T, K>
    {
        Task<T> Create(T entity);
        Task<T> Read(K id);
        Task<bool> Update(T entity);
        Task<bool> Delete(K id);
        Task<IEnumerable<T>> GetAll();
    }
}
