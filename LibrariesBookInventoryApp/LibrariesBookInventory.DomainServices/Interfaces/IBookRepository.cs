using LibrariesBookInventory.DomainServices.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LibrariesBookInventory.Domain.Models;

namespace LibrariesBookInventory.DomainServices.Interfaces
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> Search(SearchBooksParameters parameters, CancellationToken cancellationToken);
        void Create(Book book);
        Task Delete(long id, CancellationToken cancellationToken);
        Task<IEnumerable<Book>> GetAll(CancellationToken cancellationToken);
        Task<Book> Get(long id, CancellationToken cancellationToken);
        Task Update(Book book, CancellationToken cancellationToken);
        Task SaveChanges(CancellationToken cancellationToken);
    }
}