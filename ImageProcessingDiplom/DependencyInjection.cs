using ImageProcessingDiplom.OpenCvServices;
using Microsoft.Extensions.DependencyInjection;

namespace ImageProcessingDiplom
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<HammingProvider>();
            services.AddTransient<ManhattanDictanceProvider>();

            return services;
        }
    }
}
