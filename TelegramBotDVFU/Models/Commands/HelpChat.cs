using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotDVFU.View;

namespace TelegramBotDVFU.Models.Commands;

public class HelpChat : Command
{
    public override string[] Names
    {
        get => new[] {"Чат P!N'a"};
        set => throw new NotImplementedException();
    }

    public override int AdminsCommand => 0;
    public override void Execute(Message message)
    {
        var chatId = message.Chat.Id;
        // await botClient.SendTextMessageAsync(chatId,
        //     "Задавай вопросы и общайся в этом чате\n"+@"https://t.me/+J21bmIAXFXlmNDMy");
    }

    public override bool Contains(Message message)
    {
        if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
            return false;

        return message.Text != null && Names.Contains(message.Text);
    }
}