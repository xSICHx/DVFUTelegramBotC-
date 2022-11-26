using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotDVFU.View;

namespace TelegramBotDVFU.Models.Commands;

public class ChangeAdminMenu : Command
{
    public override string[] Names
    {
        get => new[] {"Админские штучки", "Площадка"};
        set => throw new NotImplementedException();
    }

    public override int AdminsCommand => 1;
    public override void Execute(Message message)
    {
        var chatId = message.Chat.Id;
        var (_, buttons, _) = Menu.GetMenu(message);
        // await botClient.SendTextMessageAsync(chatId, "Что дальше?",
        //     replyMarkup: buttons);
    }

    public override bool Contains(Message message)
    {
        if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
            return false;

        return message.Text != null && Names.Contains(message.Text);
    }
}