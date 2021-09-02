using Discord;
using Discord.WebSocket;
using Domain.Contracts.Persistance;
using Domain.Entities;
using Persistance.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utilities
{
    public class AutoRoleService
    {
        private readonly IAutoRoleRepository _autoRoleRepository;

        public AutoRoleService(IAutoRoleRepository autoRoleRepository)
        {
            _autoRoleRepository = autoRoleRepository;
        }

        public async Task<List<IRole>> GetAutoRolesAsync(IGuild guild)
        {
            var roles = new List<IRole>();
            var invalidAutoRoles = new List<AutoRole>();
            var autoRoles = await _autoRoleRepository.GetAutoRolesAsync(guild.Id);

            foreach (var autoRole in autoRoles)
            {
                var role = guild.Roles.FirstOrDefault(x => x.Id == autoRole.RoleId);
                if (role == null)
                {
                    invalidAutoRoles.Add(autoRole);
                }
                else
                {
                    var currentUser = await guild.GetCurrentUserAsync();
                    var hierarchy = (currentUser as SocketGuildUser).Hierarchy;
                    if (role.Position > hierarchy)
                        invalidAutoRoles.Add(autoRole);
                    else
                        roles.Add(role);
                }
            }
            if (invalidAutoRoles.Count > 0)
                await _autoRoleRepository.ClearAutoRolesAsync(invalidAutoRoles);

            return roles;
        }

        public async Task AddAutoRoleAsync(ulong guildId, ulong roleId)
        {
            await _autoRoleRepository.AddAutoRoleAsync(guildId, roleId);
        }

        public async Task RemoveAutoRoleAsync(ulong guildId, ulong roleId)
        {
            await _autoRoleRepository.RemoveAutoRoleAsync(guildId, roleId);
        }
    }
}
