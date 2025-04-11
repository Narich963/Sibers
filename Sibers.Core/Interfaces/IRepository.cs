using System.Linq.Expressions;

namespace Sibers.Core.Interfaces;

public interface IRepository<T> where T : class
{
    /// <summary>
    /// Get all entities
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Get one entity by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<T?> Get(int id);

    /// <summary>
    /// Creates an entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task Create(T entity);

    /// <summary>
    /// Updates an entity
    /// </summary>
    /// <param name="entity"></param>
    void Update(T entity);

    /// <summary>
    /// Delete an entity
    /// </summary>
    /// <param name="id"></param>
    void Delete(int id);

    /// <summary>
    /// Save changes to database
    /// </summary>
    /// <returns></returns>
    Task SaveAsync();

    /// <summary>
    /// Get all entities as query
    /// </summary>
    /// <returns></returns>
    IQueryable<T> GetAllAsQueryable();

    /// <summary>
    /// Get Count of Entity
    /// </summary>
    /// <returns></returns>
    Task<int> Count();
}
