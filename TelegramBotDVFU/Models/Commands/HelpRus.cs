using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBotDVFU.Models.Commands;

public class HelpRus : Command
{
    public override string[] Names
    {
        get => new[] {"Помощь"};
        set => throw new NotImplementedException();
    }

    public override int AdminsCommand => 0;
    public override Task Execute(Message message, TelegramBotClient botClient)
    {
        var help = new Help();
        return help.Execute(message, botClient);
    }

    public override bool Contains(Message message)
    {
        if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
            return false;

        return message.Text != null && Names.Contains(message.Text);
    }
}