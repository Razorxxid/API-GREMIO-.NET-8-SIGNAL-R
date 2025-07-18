using Microsoft.EntityFrameworkCore;
using PWA_GREMIO_API.Core.Entities;
using PWA_GREMIO_API.Core.Entities.Users;
using PWA_GREMIO_API.Core.Repositories;
using PWA_GREMIO_API.Infraestructure.Data;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PWA_GREMIO_API.Infraestructure.Repository
{
    public class Repository<TE, TI> : IRepository<TE, TI> where TE : EntityBase<TI>
    {
        private DbSet<TE> _dbSet;


        public Repository(DefaultContext context)
        {
            _dbSet = context.Set<TE>();
        }

        public async Task<TE> Add(TE entity)
        {
            var valueTask = await _dbSet.AddAsync(entity);
         
            return valueTask.Entity;
        }

        public async Task<IEnumerable<TE>> AddMany(IEnumerable<TE> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            return entities;
        }   

        public async Task<IEnumerable<TE>> AddMany(TE[] entities)
        {
            await _dbSet.AddRangeAsync(entities);
            return entities;
        }

        public async Task<bool> Any(Expression<Func<TE, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public void Delete(TE entity)
        {
            _dbSet.Remove(entity);
        }

        public void DeleteMany(IEnumerable<TE> entities)
        {
               _dbSet.RemoveRange(entities);
        }

        public void DeleteMany(TE[] entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public async Task<TE?> Get(TI id)
        {
            TE? entity = null;
            if (id == null)
            {
                Console.WriteLine($"El valor de TI es nulo.");
            }
            try
            {
                // Asigna el resultado de la consulta a la variable entity
                entity = await  _dbSet.FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // Agrega logs o imprime la pila de llamadas para depurar el problema
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Error: el valor de TI es: {id}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");

                // Relanza la excepción para que la aplicación pueda manejarla adecuadamente
                throw;
            }
            return entity;
        }

        public async Task<IList<TE>> GetAll()
        {
            return await _dbSet.AsNoTracking()
                               .ToListAsync();
        }

        public void Update(TE entity)
        {
            _dbSet.Update(entity);
        }

        public async Task<TProyected?> GetProyected<TProyected>(Expression<Func<TE, bool>> predicate, Expression<Func<TE, TProyected>> proyection)
        {
          return await _dbSet.Where(predicate)
                             .Select(proyection)
                             .FirstOrDefaultAsync();
          
        }

        public async Task<IEnumerable<TProyected>> GetProyectedMany<TProyected>(
            Expression<Func<TE, bool>> predicate, Expression<Func<TE, TProyected>> proyection)
        {
            return await _dbSet.Where(predicate)
                               .Select(proyection)
                               .ToListAsync();
        }
    }
}
