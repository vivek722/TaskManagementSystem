namespace TaskManagementSystem.Extension;

public static class JSonSerilizeExtention
{
    public static IServiceCollection AddJsonSerilize(this IServiceCollection services)
    {
        services.AddControllers()
             .AddJsonOptions(options =>
             {
                 options.JsonSerializerOptions.ReferenceHandler =
                     System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
             });
        return services;
    }
}
