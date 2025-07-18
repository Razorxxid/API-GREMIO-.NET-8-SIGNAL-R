using Microsoft.EntityFrameworkCore;
using PWA_GREMIO_API.Core.Entities;
using PWA_GREMIO_API.Core.Entities.Users;
using PWA_GREMIO_API.Core.Repositories;
using PWA_GREMIO_API.Infraestructure.Data;
using PWA_GREMIO_API.Infraestructure.Repository;

namespace PWA_GREMIO_API.Infraestructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DefaultContext _context;

        public UnitOfWork(DefaultContext context)
        {
            _context = context;
        }

      
        public async Task<int> Commit()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void DetachAllEntities()
        {
            foreach (var entry in _context.ChangeTracker.Entries())
            {
                entry.State = EntityState.Detached;
            }
        }
        public IRepository<TE, TI> GetRepository<TE, TI>() where TE : EntityBase<TI>
        {
            return new Repository<TE, TI>(_context);
        }
    }
}
