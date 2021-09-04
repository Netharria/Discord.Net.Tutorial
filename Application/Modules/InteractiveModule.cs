using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Modules
{
    public class InteractiveModule : InteractiveBase
    {
        private readonly CommandService _service;

        public InteractiveModule(CommandService service)
        {
            _service = service;
        }

        // DeleteAfterAsync will send a message and asynchronously delete it after the timeout has popped
        // This method will not block.
        [Command("delete")]
        public async Task<RuntimeResult> Test_DeleteAfterAsync()
        {
            await ReplyAndDeleteAsync("this message will delete in 10 seconds", timeout: TimeSpan.FromSeconds(10));
            return Ok();
        }

        // NextMessageAsync will wait for the next message to come in over the gateway, given certain criteria
        // By default, this will be limited to messages from the source user in the source channel
        // This method will block the gateway, so it should be ran in async mode.
        [Command("next", RunMode = RunMode.Async)]
        public async Task Test_NextMessageAsync()
        {
            await ReplyAsync("What is 2+2?");
            var response = await NextMessageAsync();
            if (response != null)
            {
                if (response.Content.Equals("4", StringComparison.CurrentCulture))
                {
                    await ReplyAsync($"You replied: {response.Content}");
                }
                else
                {
                    await ReplyAsync($"Wrong! The answer was 4");
                }
            }
            else
                await ReplyAsync("You did not reply before the timeout");
        }

        // PagedReplyAsync will send a paginated message to the channel1
        // You can customize the paginator by creating a PaginatedMessage object
        // You can customize the criteria for the paginator as well, which defaults to restricting to the source user
        // This method will not block.
        [Command("paginator")]
        [Summary("This will create a paginator.")]
        public async Task Test_Paginator()
        {
            var pages = new[] { "Page 1", "Page 2", "Page 3", "aaaaaa", "Page 5" };
            PaginatedMessage paginatedMessage = new PaginatedMessage()
            {
                Color = new Color(33, 176, 252),
                Pages = pages,
                Options = new PaginatedAppearanceOptions
                {
                    Info = new Emoji("⚽"),
                    InformationText = "This is a test.",
                },
                Title = "My Nice interactive paginator."
            };
            await PagedReplyAsync(paginatedMessage);
        }

        [Command("help")]
        public async Task Help()
        {
            var listPages = new List<string>();

            foreach(var module in _service.Modules)
            {
                var page = $"**{module.Name}**\n\n";
                page = string.Join(' ', module.Commands.Select(x => $"`!{x.Name}` - {x.Summary ?? "No Description Provided"}\n").ToArray());
                listPages.Add(page);
            }

            await PagedReplyAsync(listPages);
        }
    }
}
