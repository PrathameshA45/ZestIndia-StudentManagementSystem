using Structure.Domain.Context;
using Structure.Repository.Interfaces;
using Structure.Repository;

namespace Structure.Repository.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly Dictionary<Type, object> _repositories;
    private IStudentRepository? _studentRepository;

    public IStudentRepository Students
    {
        get
        {
            _studentRepository ??= new StudentRepository(_context);
            return _studentRepository;
        }
    }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        _repositories = new Dictionary<Type, object>();
    }

    public IRepository<T> Repository<T>() where T : class
    {
        if (_repositories.ContainsKey(typeof(T)))
            return (IRepository<T>)_repositories[typeof(T)];

        var repository = new Repository<T>(_context);
        _repositories.Add(typeof(T), repository);
        return repository;
    }

    public async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}