using System;
using System.Threading.Tasks;

namespace NiteDeliveryService.Shared.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public Task SaveAsync();
    }
}
