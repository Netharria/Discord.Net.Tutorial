using Microsoft.Extensions.DependencyInjection;
using Application.Services;
using System;
using Application.Utilities;

namespace Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services
                .AddHostedService<CommandHandler>()
                .AddSingleton<Images>()
                .AddSingleton<AutoRoleService>()
                .AddSingleton<RankService>();
            
            return services;
        }
    }
}
