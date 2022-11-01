using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotDVFU.View;

namespace TelegramBotDVFU.Models.Commands;

public class StartCommand : Command
{
    public override string[] Names
    {
        get => new[] {@"/start"};
        set => throw new NotImplementedException();
    }

    public override int AdminsCommand => 0;

    public override bool Contains(Message message)
    {
        if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
            return false;

        return message.Text != null && Names.Contains(message.Text);
    }

    public override async Task Execute(Message message, TelegramBotClient botClient)
    {
        var chatId = message.Chat.Id;
        var (name, buttons, parent) = Menu.GetMenu(message);
        await botClient.SendTextMessageAsync(chatId, "Добро пожаловать на борт, " + message.Chat.FirstName + "!",
            replyMarkup: buttons);
    }
}