using LibrariesBookInventory.DomainServices.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Threading;
using LibrariesBookInventory.Domain.Models;

namespace LibrariesBookInventory.DomainServices.Implementation
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(DbContext context) : base(context)
        {
        }
        public void Create(Category category)
        {
            Set.Add(category);
        }

        public async Task Delete(int id, CancellationToken cancellationToken)
        {
            var entity = await Get(id, cancellationToken);

            Set.Remove(entity);
        }

        public async Task<Category> Get(int id, CancellationToken cancellationToken)
        {
            return await Set.FindAsync(cancellationToken, id);
        }

        public async Task<IEnumerable<Category>> GetAll(CancellationToken cancellationToken)
        {
            return await Set.ToListAsync(cancellationToken);
        }

        public async Task Update(Category category, CancellationToken cancellationToken)
        {
            var dbCategory = await Get(category.Id, cancellationToken);

            dbCategory.Description = category.Description;
            dbCategory.Name = category.Name;
        }
    }
}