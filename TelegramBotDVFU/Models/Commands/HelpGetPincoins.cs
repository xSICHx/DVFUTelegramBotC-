using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotDVFU.View;
using File = System.IO.File;

namespace TelegramBotDVFU.Models.Commands;

public class HelpGetPincoins : Command
{
    public override string[] Names
    {
        get => new[] {@"Как купить мерч?"};
        set => throw new NotImplementedException();
    }

    public override int AdminsCommand => 0;
    public override async Task Execute(Message message, TelegramBotClient botClient)
    {
        var chatId = message.Chat.Id;
        
        await botClient.SendTextMessageAsync(chatId, "QR-код твоего тг— твой лучший друг." +
                                                     " После выполнения миссии покажи его организатору," +
                                                     " чтобы получить валюту, а также, чтобы обменять виртуальную покупку в магазине на реальную");
        using (var stream = File.OpenRead("../TelegramBotDVFU/Images/qrIOS.jpg"))
        {
            InputOnlineFile inputOnlineFile = new InputOnlineFile(stream);
            await botClient.SendPhotoAsync(chatId,
                inputOnlineFile, "Где найти QR на IOS");
        }
        using (var stream = File.OpenRead("../TelegramBotDVFU/Images/qrAndroid.jpg"))
        {
            InputOnlineFile inputOnlineFile = new InputOnlineFile(stream);
            await botClient.SendPhotoAsync(chatId,
                inputOnlineFile, "Где найти QR на Android");
        }
    }

    public override bool Contains(Message message)
    {
        if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
            return false;

        return message.Text != null && Names.Contains(message.Text);
    }
}