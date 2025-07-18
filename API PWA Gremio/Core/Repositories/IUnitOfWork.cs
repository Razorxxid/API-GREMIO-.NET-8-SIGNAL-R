
using PWA_GREMIO_API.Core.Entities.Users;

namespace PWA_GREMIO_API.Core.Repositories
{
    public interface IUnitOfWork
    {
        Task<int> Commit();

        
        IRepository<TE, TI> GetRepository<TE, TI>() where TE : EntityBase<TI>;  

        void DetachAllEntities();

        void Dispose();
    }

}
