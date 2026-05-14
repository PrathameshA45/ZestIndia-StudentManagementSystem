using Microsoft.EntityFrameworkCore;
using Structure.Data.Entities;
using Structure.Domain.Context;
using Structure.Repository.Interfaces;
using Structure.Repository;

namespace Structure.Repository;

public class StudentRepository : Repository<Student>, IStudentRepository
{
    private readonly ApplicationDbContext _context;

    public StudentRepository(ApplicationDbContext context)
        : base(context)
    {
        _context = context;
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        var normalizedEmail = email.Trim().ToLower();

        return await _context.Students
            .AsNoTracking()
            .AnyAsync(x =>
                x.Email.ToLower() == normalizedEmail &&
                !x.IsDeleted);
    }

    public Task<Student?> GetByEmailAsync(string email)
    {
        throw new NotImplementedException();
    }
}