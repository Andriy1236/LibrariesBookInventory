using System;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;

namespace LibrariesBookInventory.DomainServices.Implementation
{
    public abstract class BaseRepository<T> : IDisposable where T : class
    {
        private bool _disposed;
        private readonly DbContext _context;
        protected readonly DbSet<T> Set;
        protected BaseRepository(DbContext context) 
        {
            _context = context;
            Set = _context.Set<T>();
        }


        public async Task SaveChanges(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
