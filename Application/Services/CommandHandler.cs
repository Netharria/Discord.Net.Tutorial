using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Domain.Contracts.Persistance;
using Discord;
using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Application.Utilities;
using Application.Common;
using System.Collections.Generic;

namespace Application.Services
{
    public class CommandHandler : InitializedService
    {
        private readonly IServiceProvider _provider;
        private readonly DiscordSocketClient _client;
        private readonly CommandService _service;
        private readonly IConfiguration _config;
        private readonly IServerRepository _servers;
        private readonly BotService _botService;
        private string previousMessage;
        private readonly Images _images;
        public static List<Mute> Mutes = new List<Mute>();

        public CommandHandler(IServiceProvider provider, DiscordSocketClient client, CommandService service, IConfiguration config, IServerRepository servers, BotService botService, Images images)
        {
            _provider = provider;
            _client = client;
            _service = service;
            _config = config;
            _servers = servers;
            _images = images;
            _botService = botService;
        }

        public override async Task InitializeAsync(CancellationToken cancellationToken)
        {
            _client.MessageReceived += OnMessageReceived;
            _client.UserJoined += OnUserJoined;
            _client.ReactionAdded += OnReactionAdded;
            _client.JoinedGuild += OnJoinedGuild;
            _client.LeftGuild += LeftGuild;
            var newTask = new Task(async () => await MuteHandler());
            newTask.Start();
            _service.CommandExecuted += OnCommandExecuted;
            await _service.AddModulesAsync(Assembly.GetExecutingAssembly(), _provider);
        }

        private async Task LeftGuild(SocketGuild arg)
        {
            await _client.SetGameAsync($"in {_client.Guilds.Count}", null, ActivityType.Playing);
        }

        private async Task OnJoinedGuild(SocketGuild arg)
        {
            await _client.SetGameAsync($"in {_client.Guilds.Count}", null, ActivityType.Playing);
        }

        private async Task OnReactionAdded(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            if (arg3.MessageId != 883157940276691016) return;

            if (arg3.Emote.Name != "✅") return;

            var channel = await (arg2 as ITextChannel).Guild.GetTextChannelAsync(639594402414854145);

            await channel.SendMessageAsync($"{arg3.User.Value.Mention} replied with the ✅");
        }

        private async Task MuteHandler()
        {
            List<Mute> Remove = new List<Mute>();
            foreach(var mute in Mutes)
            {
                if (DateTime.Now < mute.End) continue;

                var guild = _client.GetGuild(mute.Guild.Id);

                if (mute.Guild.GetRole(mute.Role.Id) is null)
                {
                    Remove.Add(mute);
                    continue;
                }

                var role = guild.GetRole(mute.Role.Id);

                if(guild.GetUser(mute.User.Id) is null)
                {
                    Remove.Add(mute);
                    continue;
                }

                var user = guild.GetUser(mute.User.Id);

                if(role.Position > guild.CurrentUser.Hierarchy)
                {
                    Remove.Add(mute);
                    continue;
                }

                await user.RemoveRoleAsync(mute.Role);
                Remove.Add(mute);
            }

            Mutes = Mutes.Except(Remove).ToList();

            await Task.Delay(1 * 60 * 1000);

            await MuteHandler();
        }

        private async Task OnUserJoined(SocketGuildUser arg) =>
            new Task(async () => await HandleUserJoined(arg)).Start();

        private async Task HandleUserJoined(SocketGuildUser arg)
        {
            var roles = await _botService.GetAutoRolesAsync(arg.Guild);
            if (roles.Count > 0)
                await arg.AddRolesAsync(roles);

            var channelId = await _servers.GetWelcomeAsync(arg.Guild.Id);
            if (channelId == default)
                return;

            var channel = arg.Guild.GetTextChannel(channelId);
            if(channel is null)
            {
                await _servers.ClearWelcomeAsync(arg.Guild.Id);
                return;
            }

            var background = await _servers.GetBackgroundAsync(arg.Guild.Id);

            string path = await _images.CreateImageAsync(arg, background);

            await channel.SendFileAsync(filePath: path, text: null) ;
            System.IO.File.Delete(path);

        }

        private async Task OnMessageReceived(SocketMessage arg)
        {
            if (!(arg is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;

            string[] filters = new string[] { "discord.gg", "apple", "macbook" };

            if (message.Content.Split(" ").Intersect(filters).Any())
            {
                if (!(message.Channel as SocketGuildChannel).Guild.GetUser(message.Author.Id).GuildPermissions.Administrator)
                {
                    await message.DeleteAsync();
                    await message.Channel.SendMessageAsync($"{message.Author.Mention} You sent a blacklisted word!");
                    return;
                }
            }

            if (message.Content.Equals(previousMessage, StringComparison.CurrentCultureIgnoreCase))
            {
                await message.DeleteAsync();
                await message.Channel.SendMessageAsync($"{message.Author.Mention} Please do not spam that message.");
                return;
            }
            previousMessage = message.Content;
            var argPos = 0;
            var prefix = await _servers.GetGuildPrefix((message.Channel as SocketGuildChannel).Guild.Id) ?? _config["prefix"];
            if (!message.HasStringPrefix(prefix, ref argPos) && !message.HasMentionPrefix(_client.CurrentUser, ref argPos)) return;
            var context = new SocketCommandContext(_client, message);
            await _service.ExecuteAsync(context, argPos, _provider);
        }

        private async Task OnCommandExecuted(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if (command.IsSpecified && !result.IsSuccess) await (context.Channel as ISocketMessageChannel).SendErrorAsync("Error", result.ErrorReason);
        }
    }
}