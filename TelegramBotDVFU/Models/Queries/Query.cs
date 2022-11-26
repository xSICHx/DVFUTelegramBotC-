using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBotDVFU.Models.Queries;

public abstract class Query
{
    public abstract string[] Names { get; }

    public abstract void Execute(Update message);

    public abstract bool Contains(Update message);
}