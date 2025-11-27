using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Infrastructure.Persistence;

public static class ConfigurePersistenceServices
{
    public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("DefaultConnection"));
        dataSourceBuilder.EnableDynamicJson();
        var dataSource = dataSourceBuilder.Build();

        services.AddDbContext<ApplicationDbContext>(options => options
            .UseNpgsql(
                dataSource,
                builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
            .UseSnakeCaseNamingConvention()
            .ConfigureWarnings(w => w.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning)));

        services.AddScoped<ApplicationDbContextInitialiser>();
        services.AddRepositories();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        // User
        services.AddScoped<UserRepository>();
        services.AddScoped<IUserRepository>(provider => provider.GetRequiredService<UserRepository>());
        services.AddScoped<IUserQueries>(provider => provider.GetRequiredService<UserRepository>());

        // Lesson
        services.AddScoped<LessonRepository>();
        services.AddScoped<ILessonRepository>(provider => provider.GetRequiredService<LessonRepository>());
        services.AddScoped<ILessonQueries>(provider => provider.GetRequiredService<LessonRepository>());

        // Course
        services.AddScoped<CourseRepository>();
        services.AddScoped<ICourseRepository>(provider => provider.GetRequiredService<CourseRepository>());
        services.AddScoped<ICourseQueries>(provider => provider.GetRequiredService<CourseRepository>());

        // Role
        services.AddScoped<RoleRepository>();
        services.AddScoped<IRoleRepository>(provider => provider.GetRequiredService<RoleRepository>());
        services.AddScoped<IRoleQueries>(provider => provider.GetRequiredService<RoleRepository>());

        // Skill
        services.AddScoped<SkillRepository>();
        services.AddScoped<ISkillRepository>(provider => provider.GetRequiredService<SkillRepository>());
        services.AddScoped<ISkillQueries>(provider => provider.GetRequiredService<SkillRepository>());

        // UserSkill
        services.AddScoped<UserSkillRepository>();
        services.AddScoped<IUserSkillRepository>(provider => provider.GetRequiredService<UserSkillRepository>());
        services.AddScoped<IUserSkillQueries>(provider => provider.GetRequiredService<UserSkillRepository>());

        // CourseSkill
        services.AddScoped<CourseSkillRepository>();
        services.AddScoped<ICourseSkillRepository>(provider => provider.GetRequiredService<CourseSkillRepository>());
        services.AddScoped<ICourseSkillQueries>(provider => provider.GetRequiredService<CourseSkillRepository>());

        // Profile
        services.AddScoped<ProfileRepository>();
        services.AddScoped<IProfileRepository>(provider => provider.GetRequiredService<ProfileRepository>());
        services.AddScoped<IProfileQueries>(provider => provider.GetRequiredService<ProfileRepository>());
    }
}
