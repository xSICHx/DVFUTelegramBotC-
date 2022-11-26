using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBotDVFU.Models.Commands;

public abstract class Command
{
    public abstract string[] Names { get; set; }
    public abstract int AdminsCommand { get; }

    public abstract void Execute(Message message);

    public abstract bool Contains(Message message);
}
