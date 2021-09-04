using Domain.Contracts.Persistance;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Persistance.Repositories
{
    public class ServerRepository : BaseRepository<Server>, IServerRepository
    {

        public ServerRepository(TutorialContext dbContext) : base(dbContext) { }

        public async Task ModifyGuildPrefix(ulong id, string prefix)
        {
            var server = await _dbContext.Servers
                .FindAsync(id);

            if (server == null)
            {
                _dbContext.Add(new Server { Id = id, Prefix = prefix });
            }
            else
                server.Prefix = prefix;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<string> GetGuildPrefix(ulong id)
        {
            var prefix = await _dbContext.Servers
                .Where(x => x.Id == id)
                .Select(x => x.Prefix)
                .FirstOrDefaultAsync();

            return prefix;
        }

        public async Task ModifyWelcomeAsync(ulong id, ulong channelId)
        {
            var server = await _dbContext.Servers
                .FindAsync(id);

            if (server == null)
                _dbContext.Add(new Server { Id = id, Welcome = channelId });
            else
                server.Welcome = channelId;
            await _dbContext.SaveChangesAsync();
        }

        public async Task ClearWelcomeAsync(ulong id)
        {
            var server = await _dbContext.Servers
                .FindAsync(id);
            server.Welcome = 0;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ulong> GetWelcomeAsync(ulong id)
        {
            var server = await _dbContext.Servers
                .FindAsync(id);
            return await Task.FromResult(server.Welcome);
        }

        public async Task ModifyLogsAsync(ulong id, ulong channelId)
        {
            var server = await _dbContext.Servers
                .FindAsync(id);

            if (server == null)
                _dbContext.Add(new Server { Id = id, Logs = channelId });
            else
                server.Logs = channelId;
            await _dbContext.SaveChangesAsync();
        }

        public async Task ClearLogsAsync(ulong id)
        {
            var server = await _dbContext.Servers
                .FindAsync(id);
            server.Logs = 0;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ulong> GetLogsAsync(ulong id)
        {
            var server = await _dbContext.Servers
                .FindAsync(id);
            return await Task.FromResult(server.Logs);
        }

        public async Task ModifyBackgroundAsync(ulong id, string url)
        {
            var server = await _dbContext.Servers
                .FindAsync(id);

            if (server == null)
                _dbContext.Add(new Server { Id = id, Background = url });
            else
                server.Background = url;
            await _dbContext.SaveChangesAsync();
        }

        public async Task ClearBackgroundAsync(ulong id)
        {
            var server = await _dbContext.Servers
                .FindAsync(id);
            server.Background = null;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<string> GetBackgroundAsync(ulong id)
        {
            var server = await _dbContext.Servers
                .FindAsync(id);
            return await Task.FromResult(server.Background);
        }
    }
}
