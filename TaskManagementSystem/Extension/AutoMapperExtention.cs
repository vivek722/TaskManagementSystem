namespace TaskManagementSystem.Extension;

public static class AutoMapperExtention
{

    public static IServiceCollection AutoMapExtention(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AutoMapperProfile));
        return services;
    }
}
