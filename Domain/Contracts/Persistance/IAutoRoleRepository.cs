using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Contracts.Persistance
{
    public interface IAutoRoleRepository
    {
        Task AddAutoRoleAsync(ulong id, ulong roleId);
        Task ClearAutoRolesAsync(List<AutoRole> autoRoles);
        Task<List<AutoRole>> GetAutoRolesAsync(ulong id);
        Task RemoveAutoRoleAsync(ulong id, ulong roleId);
    }
}