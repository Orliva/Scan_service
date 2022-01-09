using Microsoft.Extensions.DependencyInjection;
using Scan_service.Service;

namespace Scan_service.Extensions
{
    /// <summary>
    /// Методы расширения для добавления сервисов
    /// </summary>
    public static class ExtensionServices
    {
        public static void AddScanService(this IServiceCollection services)
        {
            services.AddTransient<IScanService, ScanService>();
        }

        public static void AddCommandService(this IServiceCollection service)
        {
            service.AddSingleton<CommandContextService>();
        }
    }
}
