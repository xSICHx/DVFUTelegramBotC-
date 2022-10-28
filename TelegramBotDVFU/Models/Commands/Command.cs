using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBotDVFU.Models.Commands;

public abstract class Command
{
    public abstract string[] Names { get; }
    public abstract int AdminsCommand { get; }

    public abstract Task Execute(Message message, TelegramBotClient botClient);

    public abstract bool Contains(Message message);
}
