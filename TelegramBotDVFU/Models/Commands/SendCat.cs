using Telegram.Bot;
using Telegram.Bot.Types;
using HtmlAgilityPack;

namespace TelegramBotDVFU.Models.Commands;

public class SendCat : Command
{
    public override string[] Names
    {
        get => new[] {"Прислать котика"};
        set => throw new NotImplementedException();
    }

    public override int AdminsCommand => 0;
    public override async Task Execute(Message message, TelegramBotClient botClient)
    {
        var chatId = message.Chat.Id;
        try
        {
            const string url = "https://mimimi.ru/random/";
            var webpage = new HtmlWeb();
            var doc = webpage.Load(url);
            var img = doc.DocumentNode.SelectSingleNode("/html/body/div[5]/div[1]/div/div[1]/a/img").Attributes["src"]
                .Value;
            if (img != null) await botClient.SendPhotoAsync(chatId, caption: "Мур", photo: img);
        }
        catch
        {
            await botClient.SendTextMessageAsync(chatId, "Что-то пошло не так(");
        }
    }

    public override bool Contains(Message message)
    {
        if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
            return false;

        return message.Text != null && Names.Contains(message.Text);
    }
}