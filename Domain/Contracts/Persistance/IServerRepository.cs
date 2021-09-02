using Domain.Entities;
using System.Threading.Tasks;

namespace Domain.Contracts.Persistance
{
    public interface IServerRepository : IAsyncRepository<Server>
    {
        Task ModifyGuildPrefix(ulong id, string prefix);
        Task<string> GetGuildPrefix(ulong id);
        Task ModifyWelcomeAsync(ulong id, ulong channelId);

        Task ClearWelcomeAsync(ulong id);
        Task<ulong> GetWelcomeAsync(ulong id);
        Task ModifyBackgroundAsync(ulong id, string url);
        Task ClearBackgroundAsync(ulong id);
        Task<string> GetBackgroundAsync(ulong id);
    }
}
