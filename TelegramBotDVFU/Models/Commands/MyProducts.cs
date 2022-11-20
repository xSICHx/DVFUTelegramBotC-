using Telegram.Bot;
using Telegram.Bot.Types;


namespace TelegramBotDVFU.Models.Commands;

public class MyProducts : Command
{
    public override string[] Names
    {
        get => new[] {"Мои покупки"};
        set => throw new NotImplementedException();
    }

    public override int AdminsCommand => 0;
    public override async Task Execute(Message message, TelegramBotClient botClient)
    {
        var chatId = message.Chat.Id;
        await using (ApplicationUserContext db = new ApplicationUserContext())
        {
            var user = await db.Users.FindAsync(message.Chat.Username);
            string textReply = "Ага, свои P!N-коины ты потратил на следующее:";
            foreach (var product in user.ProductsPurchaced)
            {
                textReply += "\n" + product.Key + " " + product.Value + " шт.";
            }

            textReply += "\nПодойдите к организатору, чтобы обменять виртуальную покупку на реальную";
            await botClient.SendTextMessageAsync(chatId, textReply);
            await db.SaveChangesAsync();
        }
        
    }

    public override bool Contains(Message message)
    {
        if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
            return false;

        return message.Text != null && Names.Contains(message.Text);
    }
}