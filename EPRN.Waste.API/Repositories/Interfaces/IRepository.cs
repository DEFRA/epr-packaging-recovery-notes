using EPRN.Waste.API.Models;
using System.Linq.Expressions;

namespace EPRN.Waste.API.Repositories.Interfaces
{
    public interface IRepository
    {
        Task<T> GetById<T>(int id) where T : IdBaseEntity;

        IEnumerable<T> List<T>() where T : IdBaseEntity;

        IEnumerable<T> List<T>(Expression<Func<T, bool>> predicate) where T : IdBaseEntity;

        Task<int> Add<T>(T entity) where T : IdBaseEntity;

        Task Delete<T>(T entity) where T : IdBaseEntity;

        Task Update<T>(T entity) where T : IdBaseEntity;

        bool LazyLoading { set; }
    }
}