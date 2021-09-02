using Domain.Contracts.Persistance;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories
{
    public class AutoRoleRepository : BaseRepository<AutoRole>, IAutoRoleRepository
    {
        public AutoRoleRepository(TutorialContext dbContext) : base(dbContext) { }

        public async Task<List<AutoRole>> GetAutoRolesAsync(ulong id)
        {
            var autoRoles = await _dbContext.AutoRoles
                .Where(x => x.ServerId == id)
                .ToListAsync();

            return await Task.FromResult(autoRoles);
        }

        public async Task AddAutoRoleAsync(ulong id, ulong roleId)
        {
            var server = await _dbContext.Servers
                .FindAsync(id);

            if (server == null)
            {
                _dbContext.Add(new Server { Id = id });
            }
            _dbContext.Add(new AutoRole { RoleId = roleId, ServerId = id });
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveAutoRoleAsync(ulong id, ulong roleId)
        {
            var autoRole = await _dbContext.AutoRoles
                .Where(x => x.RoleId == roleId)
                .FirstOrDefaultAsync();

            _dbContext.Remove(autoRole);
            await _dbContext.SaveChangesAsync();
        }

        public async Task ClearAutoRolesAsync(List<AutoRole> autoRole)
        {
            _dbContext.RemoveRange(autoRole);
            await _dbContext.SaveChangesAsync();
        }
    }
}
