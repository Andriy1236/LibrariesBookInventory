using LibrariesBookInventory.DomainServices.Interfaces;
using LibrariesBookInventory.DomainServices.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using LibrariesBookInventory.Domain.Models;

namespace LibrariesBookInventory.DomainServices.Implementation
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        public BookRepository(DbContext context) : base(context)
        {
        }

        public void Create(Book book)
        {
            Set.Add(book);
        }
        public async Task Delete(long id, CancellationToken cancellationToken)
        {
            var entity = await Get(id, cancellationToken);

            Set.Remove(entity);
        }

        public async Task<Book> Get(long id, CancellationToken cancellationToken)
        {
            return await Set.FindAsync(cancellationToken, id);
        }

        public async Task<IEnumerable<Book>> GetAll(CancellationToken cancellationToken)
        {
            return await Set.ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Book>> Search(
        SearchBooksParameters parameters,
        CancellationToken cancellationToken)
    {
        var pageSizeParam = new SqlParameter("@PageSize", parameters.PageSize);
        var pageNumberParam = new SqlParameter("@PageNumber", parameters.PageNumber);
        var searchTextParam = new SqlParameter("@SearchText", parameters.SearchText);
        var sortColumnParam = parameters.SortColumn == null
            ? new SqlParameter("@SortColumn", DBNull.Value)
            : new SqlParameter("@SortColumn", parameters.SortColumn);
        
        var sortDirectionParam = parameters.SortDirection == null
            ? new SqlParameter("@SortDirection", DBNull.Value)
            : new SqlParameter("@SortDirection", parameters.SortDirection);

        return await Set.SqlQuery("EXECUTE GetBooks @PageSize, @PageNumber, @SearchText, @SortColumn, @SortDirection",
            pageSizeParam, pageNumberParam, searchTextParam, sortColumnParam, sortDirectionParam).AsNoTracking().ToListAsync(cancellationToken);
    }

        public async Task Update(Book book, CancellationToken cancellationToken)
        {
            var dbBook = await Get(book.Id, cancellationToken); 

            dbBook.Author = book.Author;
            dbBook.Title = book.Title;
            dbBook.CategoryId = book.CategoryId;
            dbBook.ISBN = book.ISBN;
            dbBook.PublicationYear = book.PublicationYear; 
            dbBook.Quantity = book.Quantity;    
        }
    }
}