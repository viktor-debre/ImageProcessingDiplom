﻿using ImageProcessingDiplom.Interfaces;
using ImageProcessingDiplom.OpenCvServices;
using Microsoft.Extensions.DependencyInjection;

namespace ImageProcessingDiplom
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IHammingProvider, HammingProvider>();
            services.AddTransient<IManhattanDictanceProvider, ManhattanDictanceProvider>();
            services.AddTransient<MedoidFinder>();

            return services;
        }
    }
}
