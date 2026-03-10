namespace TaskManagementSystem.Extension;

public static class RedishExtention
{
    public static IServiceCollection AddRedishCache(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
            options.InstanceName = "TaskManagementSystem:";
        });
        return services;
    }
}
