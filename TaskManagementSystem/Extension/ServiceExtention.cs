using TaskManagementSystem.interfaces;
using TaskManagementSystem.Repository;
using TaskManagementSystem.services;
using TaskManagementSystem.Utility.CacheService;

namespace TaskManagementSystem.Extension;

public static class ServiceExtention
{

    public static IServiceCollection AddService(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeRePository, EmployeeRepository>();
        services.AddScoped<IEmployeeServices, EmployeeService>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IProjectServices, ProjectService>();
        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<ITaskServices, TaskService>();
        services.AddScoped<ICacheService, CacheService>();

        return services;
    }
}
