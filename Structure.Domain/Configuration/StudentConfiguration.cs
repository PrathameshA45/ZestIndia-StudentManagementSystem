using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Structure.Data.Entities;

namespace Structure.Domain.Configurations;

public class StudentConfiguration
    : IEntityTypeConfiguration<Student>
{
    public void Configure(
        EntityTypeBuilder<Student> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasMaxLength(150)
            .IsRequired();

        builder.HasIndex(x => x.Email)
            .IsUnique();

        builder.Property(x => x.Course)
            .HasMaxLength(100)
            .IsRequired();
    }
}