using Microsoft.Extensions.DependencyInjection;
using Application.Services;
using System;
using Application.Utilities;
using Discord.Addons.Interactive;

namespace Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services
                .AddHostedService<CommandHandler>()
                .AddSingleton<InteractiveService>()
                .AddSingleton<Images>()
                .AddSingleton<BotService>();
            
            return services;
        }
    }
}
