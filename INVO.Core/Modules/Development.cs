using System.Linq;
using Discord.Commands;
using System.Threading.Tasks;
using INVO.Core.Common;
using Discord;
using System.Collections.Generic;


namespace INVO.Core.Modules
{
    [Name("Development")]
    public class Development : ModuleBase<SocketCommandContext>
    {
        [Command("say"), Alias("s")]
        [Remarks("Speak as core")]
        public async Task Say([Remainder] string text)
        {
            await ReplyAsync(Functions.BotTextFormat(text));
        }

        [Command("clear"), Alias("clear", "purge", "delete")]
        [Summary("Clears messages.")]
        public async Task Clean(
            [Summary("The optional number of messages to delete; defaults to 10")] int count = 10,
            [Summary("The type of messages to delete - Self, Bot, or All")] DeleteType deleteType = DeleteType.Self,
            [Summary("The strategy to delete messages - BulkDelete or Manual")] DeleteStrategy deleteStrategy =
                DeleteStrategy.BulkDelete)
        {
            int index = 0;
            var deleteMessages = new List<IMessage>(count);
            var messages = Context.Channel.GetMessagesAsync();
            await messages.ForEachAsync(async m =>
            {
                IEnumerable<IMessage> delete = null;
                switch (deleteType)
                {
                    case DeleteType.Self:
                        delete = m.Where(msg => msg.Author.Id == Context.Client.CurrentUser.Id);
                        break;
                    case DeleteType.Bot:
                        delete = m.Where(msg => msg.Author.IsBot);
                        break;
                    case DeleteType.All:
                        delete = m;
                        break;
                }

                foreach (var msg in delete.OrderByDescending(msg => msg.Timestamp))
                {
                    if (index >= count)
                    {
                        await EndClean(deleteMessages, deleteStrategy);
                        return;
                    }
                    deleteMessages.Add(msg);
                    index++;
                }
            });
        }

        internal async Task EndClean(IEnumerable<IMessage> messages, DeleteStrategy strategy)
        {
            switch (strategy)
            {
                case DeleteStrategy.BulkDelete:
                    await Context.Channel.DeleteMessagesAsync(messages);
                    break;
                case DeleteStrategy.Manual:
                    foreach (var msg in messages.Cast<IUserMessage>())
                    {
                        await msg.DeleteAsync();
                    }
                    break;
            }
        }
    }
    public enum DeleteType
    {
        Self = 0,
        Bot = 1,
        All = 2
    }
    public enum DeleteStrategy
    {
        BulkDelete = 0,
        Manual = 1,

    }
}