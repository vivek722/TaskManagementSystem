using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace TaskManagementSystem.Extension;

public static class RateLimitingExtention
{

    public static IServiceCollection AddCustomerRatelimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.AddFixedWindowLimiter("fixed", limiterOptions =>
            {
                limiterOptions.PermitLimit = 1;        // Max requests
                limiterOptions.Window = TimeSpan.FromSeconds(60);
                limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                limiterOptions.QueueLimit = 2;
            });
        });
        return services;
    }
}
