using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotDVFU.View;

namespace TelegramBotDVFU.Models.Commands;

public class PinChannel : Command
{
    public override string[] Names
    {
        get => new[] {@"Канал P!N'a"};
        set => throw new NotImplementedException();
    }

    public override int AdminsCommand => 0;
    public override async Task Execute(Message message, TelegramBotClient botClient)
    {
        var chatId = message.Chat.Id;
        await botClient.SendTextMessageAsync(chatId,
            @"Новости, изменения, бэкстейджи в нашем канале(https://t.me/pintournament)");
    }

    public override bool Contains(Message message)
    {
        if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
            return false;

        return message.Text != null && Names.Contains(message.Text);
    }
}