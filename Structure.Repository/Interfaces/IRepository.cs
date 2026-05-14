using System.Linq.Expressions;

namespace Structure.Repository.Interfaces;

public interface IRepository<T>
    where T : class
{
    Task<IEnumerable<T>> GetAllAsync(
        int pageNumber = 1,
        int pageSize = 10);

    Task<T?> GetByIdAsync(Guid id);

    Task AddAsync(T entity);

    void Update(T entity);

    void Delete(T entity);

    Task<bool> ExistsAsync(
        Expression<Func<T, bool>> predicate);
}