using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using File = System.IO.File;

namespace TelegramBotDVFU.Models.Commands;

public class PinMap : Command
{
    public override string[] Names
    {
        get => new[] {@"P!N - карта"};
        set => throw new NotImplementedException();
    }

    public override int AdminsCommand => 0;
    public override void Execute(Message message)
    {
        var chatId = message.Chat.Id;
        using (var stream = File.OpenRead("../TelegramBotDVFU/Images/PinMap.jpg"))
        {
            InputOnlineFile inputOnlineFile = new InputOnlineFile(stream);
            // await botClient.SendPhotoAsync(chatId,
            //     inputOnlineFile);
        }
    }

    public override bool Contains(Message message)
    {
        if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
            return false;

        return message.Text != null && Names.Contains(message.Text);
    }
}