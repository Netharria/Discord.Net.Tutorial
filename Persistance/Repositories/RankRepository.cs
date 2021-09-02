using Domain.Entities;
using Domain.Contracts.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories
{
    public class RankRepository : BaseRepository<Rank>, IRankRepository
    {
        public RankRepository(TutorialContext dbContext) : base(dbContext) { }

        public async Task<List<Rank>> GetRanksAsync(ulong id)
        {
            var ranks = await _dbContext.Ranks
                .Where(x => x.ServerId == id)
                .ToListAsync();

            return await Task.FromResult(ranks);
        }

        public async Task AddRankAsync(ulong id, ulong roleId)
        {
            var server = await _dbContext.Servers
                .FindAsync(id);

            if (server == null)
            {
                _dbContext.Add(new Server { Id = id });
            }
            _dbContext.Add(new Rank { RoleId = roleId, ServerId = id });
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveRankAsync(ulong id, ulong roleId)
        {
            var rank = await _dbContext.Ranks
                .Where(x => x.RoleId == roleId)
                .FirstOrDefaultAsync();

            _dbContext.Remove(rank);
            await _dbContext.SaveChangesAsync();
        }

        public async Task ClearRanksAsync(List<Rank> ranks)
        {
            _dbContext.RemoveRange(ranks);
            await _dbContext.SaveChangesAsync();
        }
    }
}
