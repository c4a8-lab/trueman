using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Trueman.Core.Config;
using Trueman.Core.Services;

namespace Trueman.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCommonServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();

            var appConfig = new AppConfig();
            configuration.GetSection(nameof(AppConfig)).Bind(appConfig);
            //Create singleton from instance
            services.AddSingleton<AppConfig>(appConfig);

            services.AddScoped<IGraphClientProvider, GraphClientProvider>();
            services.AddScoped<UserDeviceManagementService>();
            services.AddScoped<UserGraphService>();
            services.AddScoped<WindowsAutopilotGraphService>();
        }
    }
}
