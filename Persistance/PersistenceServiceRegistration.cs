using Domain.Contracts.Persistance;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistance.Repositories;

namespace Persistance
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistanceServices(this IServiceCollection services)
        {
            services.AddDbContext<TutorialContext>();
            services.AddSingleton(typeof(IAsyncRepository<>), typeof(BaseRepository<>));

            services.AddSingleton<IServerRepository, ServerRepository>();
            services.AddSingleton<IRankRepository, RankRepository>();
            services.AddSingleton<IAutoRoleRepository, AutoRoleRepository>();
            return services;
        }
    }
}
