using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotDVFU.View;

namespace TelegramBotDVFU.Models.Commands;

public class HelpDeveloper : Command
{
    public override string[] Names
    {
        get => new[] {"Разработчик бота"};
        set => throw new NotImplementedException();
    }

    public override int AdminsCommand => 0;
    public override async Task Execute(Message message)
    {
        var chatId = message.Chat.Id;
        var markup = new InlineKeyboardMarkup(new []
        {
            new InlineKeyboardButton("Напишите, если есть вопросы"){Url = "https://t.me/sich_sweetheart"}
        });
        // await botClient.SendTextMessageAsync(chatId, "Разработчик бота: xSICHx");
        // await botClient.SendStickerAsync(chatId,
        //     "CAACAgIAAxkBAAEEbuxiUx3_A8H7VSIZUjmS1--jZQG8rAACGG4AAp7OCwABkdqo7KpGhagjBA",
        //     replyMarkup: markup);
    }

    public override bool Contains(Message message)
    {
        if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
            return false;

        return message.Text != null && Names.Contains(message.Text);
    }
}