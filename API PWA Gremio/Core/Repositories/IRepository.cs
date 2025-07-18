using Microsoft.EntityFrameworkCore;
using PWA_GREMIO_API.Core.Entities;
using PWA_GREMIO_API.Core.Entities.Users;
using System.Linq.Expressions;

namespace PWA_GREMIO_API.Core.Repositories
{
    public interface IRepository<TE, TI> where TE : EntityBase<TI>
    {
        Task<IList<TE>> GetAll();
        Task<TE?> Get(TI id);
        void Update(TE entity);
        Task<TE> Add(TE entity);
        void Delete(TE entity);
        Task<bool> Any(Expression<Func<TE, bool>> predicate);

        Task<TProyected?> GetProyected<TProyected>(Expression<Func<TE, bool>> predicate, Expression<Func<TE, TProyected>> proyection);
        Task<IEnumerable<TProyected?>> GetProyectedMany<TProyected>(Expression<Func<TE, bool>> predicate, Expression<Func<TE, TProyected>> proyection);
        void DeleteMany(IEnumerable<TE> entities);
        void DeleteMany(TE[] entities);
        Task<IEnumerable<TE>> AddMany(TE[] entities);
        Task<IEnumerable<TE>> AddMany(IEnumerable<TE> entities);
    }

}
