using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using LibrariesBookInventory.Domain.Models;

namespace LibrariesBookInventory.DomainServices.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAll(CancellationToken cancellationToken);
        Task<Category> Get(int id, CancellationToken cancellationToken);
        Task Update(Category book, CancellationToken cancellationToken);
        Task Delete(int id, CancellationToken cancellationToken);
        void Create(Category category);
        Task SaveChanges(CancellationToken cancellationToken);
    }
}