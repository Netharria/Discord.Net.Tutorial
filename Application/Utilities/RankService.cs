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
    public partial class BotService
    {
        private readonly IRankRepository _rankRepository;



        public async Task<List<IRole>> GetRanksAsync(IGuild guild)
        {
            var roles = new List<IRole>();
            var invalidRanks = new List<Rank>();
            var ranks = await _rankRepository.GetRanksAsync(guild.Id);

            foreach (var rank in ranks)
            {
                var role = guild.Roles.FirstOrDefault(x => x.Id == rank.RoleId);
                if (role == null)
                {
                    invalidRanks.Add(rank);
                }
                else
                {
                    var currentUser = await guild.GetCurrentUserAsync();
                    var hierarchy = (currentUser as SocketGuildUser).Hierarchy;
                    if (role.Position > hierarchy)
                        invalidRanks.Add(rank);
                    else
                        roles.Add(role);
                }
            }
            if (invalidRanks.Count > 0)
                await _rankRepository.ClearRanksAsync(invalidRanks);

            return roles;
        }

        public async Task AddRankAsync(ulong guildId, ulong roleId)
        {
            await _rankRepository.AddRankAsync(guildId, roleId);
        }

        public async Task RemoveRankAsync(ulong guildId, ulong roleId)
        {
            await _rankRepository.RemoveRankAsync(guildId, roleId);
        }
    }
}
