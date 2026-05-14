using Microsoft.EntityFrameworkCore;
using Structure.Data.Entities;
using Structure.Domain.Configurations;

namespace Structure.Domain.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Student> Students => Set<Student>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(
            new StudentConfiguration());

        modelBuilder.Entity<Student>()
            .HasQueryFilter(x => !x.IsDeleted);
    }
}