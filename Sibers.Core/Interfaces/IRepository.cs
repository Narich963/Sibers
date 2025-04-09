using System.Linq.Expressions;

namespace Sibers.Core.Interfaces;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task<T?> Get(int id);
    Task Create(T entity);
    void Update(T entity);
    void Delete(int id);
    Task SaveAsync();
    IQueryable<T> GetAllAsQueryable();
    Task<int> Count();
}
