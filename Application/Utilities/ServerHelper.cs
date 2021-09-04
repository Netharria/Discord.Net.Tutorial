using Application.Common;
using Discord;
using Domain.Contracts.Persistance;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utilities
{
    public partial class BotService
    {
        private readonly IServerRepository _serverRepository;

        public BotService(IServerRepository serverRepository, IAutoRoleRepository autoRoleRepository, IRankRepository rankRepository)
        {
            _serverRepository = serverRepository;
            _autoRoleRepository = autoRoleRepository;
            _rankRepository = rankRepository;
        }

        public async Task SendLogAsync(IGuild guild, string title, string description)
        {
            var channelId = await _serverRepository.GetLogsAsync(guild.Id);
            if (channelId == 0)
                return;

            var fetchedChannel = await guild.GetTextChannelAsync(channelId);
            if(fetchedChannel is null)
            {
                await _serverRepository.ClearLogsAsync(guild.Id);
                return;
            }

            await fetchedChannel.SendLogAsync(title, description);
        }
    }
}
