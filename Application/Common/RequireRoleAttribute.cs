using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Common
{
    public class RequireRoleAttribute : PreconditionAttribute
    {
        private readonly string _name;

        public RequireRoleAttribute(string name) => _name = name;

        public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            if (context.User is SocketGuildUser gUser)
            {
                var user = await context.Guild.GetUserAsync(context.User.Id) as SocketGuildUser;
                if (gUser.Roles.Any(r => r.Name == _name))
                    return await Task.FromResult(PreconditionResult.FromSuccess());
                else return await Task.FromResult(PreconditionResult.FromError($"You must have a role names {_name} to run this command."));
            }
            else
                return await Task.FromResult(PreconditionResult.FromError($"You must be in a guild to run this command."));
        }
    }
}
