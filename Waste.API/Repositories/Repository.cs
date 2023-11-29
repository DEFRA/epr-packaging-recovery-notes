using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Waste.API.Models;
using Waste.API.Repositories.Interfaces;
using WasteManagement.API.Data;

namespace Waste.API.Repositories
{
    public sealed class Repository : IRepository
    {
        private readonly WasteContext _wasteContext;

        public Repository(WasteContext wasteContext)
        {
            _wasteContext = wasteContext ?? throw new ArgumentNullException(nameof(wasteContext));
        }
        public async Task<int> Add<T>(T entity)
            where T : IdBaseEntity
        {
            _wasteContext.Add(entity);
            await _wasteContext.SaveChangesAsync();

            return entity.Id;
        }

        public async Task Delete<T>(T entity)
            where T : IdBaseEntity
        {
            _wasteContext.Remove(entity);
            await _wasteContext.SaveChangesAsync();
        }

        public async Task<T> GetById<T>(int id)
            where T : IdBaseEntity
        {
            return await _wasteContext.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
        }

        public IEnumerable<T> List<T>()
            where T : IdBaseEntity
        {
            return _wasteContext
                .Set<T>()
                .AsQueryable();
        }

        public IEnumerable<T> List<T>(Expression<Func<T, bool>> predicate) 
            where T : IdBaseEntity
        {
            return _wasteContext
                .Set<T>()
                .Where(predicate)
                .AsEnumerable();
        }

        public async Task Update<T>(T entity) 
            where T : IdBaseEntity
        {
            _wasteContext.Entry(entity).State = EntityState.Modified;
            await _wasteContext.SaveChangesAsync();
        }
    }
}
