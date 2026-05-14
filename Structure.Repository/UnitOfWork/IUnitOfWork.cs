using Structure.Repository.Interfaces;
using Structure.Repository;

namespace Structure.Repository.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IStudentRepository Students { get; }

    IRepository<T> Repository<T>() where T : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}