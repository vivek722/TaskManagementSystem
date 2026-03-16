using Serilog;

namespace TaskManagementSystem.Extension;

public static class SerilogExtension
    {
        public static IServiceCollection ConfigureSerilog(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithThreadId()
                .WriteTo.Console()
                .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            return services;
        }
}
