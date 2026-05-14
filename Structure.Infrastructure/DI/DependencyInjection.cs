using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Structure.Domain.Context;
using Structure.Infrastructure.Mapping;
using Structure.Infrastructure.Security;
using Structure.MediatR.Behaviors;
using Structure.Repository;
using Structure.Repository.UnitOfWork;

namespace Structure.Infrastructure.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString(
                    "DefaultConnection")));

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(
                typeof(Structure.MediatR.Student.Commands
                    .CreateStudentCommand).Assembly);
        });

        services.AddValidatorsFromAssembly(
            typeof(Structure.MediatR.Student.Commands
                .CreateStudentCommand).Assembly);

        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>));

        services.AddScoped<
            IStudentRepository,
            StudentRepository>();

        services.AddScoped<
            IUnitOfWork,
            UnitOfWork>();

        services.AddAutoMapper(
            typeof(MappingProfile));

        services.Configure<JwtSettings>(
            configuration.GetSection("JwtSettings"));

        services.AddScoped<JwtTokenGenerator>();

        return services;
    }
}