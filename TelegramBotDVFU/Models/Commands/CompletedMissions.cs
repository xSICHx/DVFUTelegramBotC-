
using Telegram.Bot.Types;
using TelegramBot.Models.Consts;

namespace TelegramBotDVFU.Models.Commands;

public class CompletedMissions : Command
{
    public override string[] Names
    {
        get => new[] {@"Завершённые миссии"};
        set => throw new NotImplementedException();
    }

    public override int AdminsCommand => 0;
    public override async Task Execute(Message message)
    {
        var chatId = message.Chat.Id;
        using var db = new ApplicationUserContext();
        var user =  db.Users.Find(message.Chat.Username);
        var flagAllCompleted = true;
        foreach (var trial in ConstTrials.TrialsList)
        {
            if (!user.TrialsDict.ContainsKey(trial.Name)) continue;
            if (!trial.CheckCompleted(message.Chat.Username)) continue;
            flagAllCompleted = false;
            // await botClient.SendTextMessageAsync(chatId, trial.CompletedMessage);
        }
            
        if (flagAllCompleted)
        {
            // await botClient.SendTextMessageAsync(chatId, "Вы ещё не выполнили ни одной миссии");
        }
        db.SaveChanges();
    }

    public override bool Contains(Message message)
    {
        if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
            return false;

        return message.Text != null && Names.Contains(message.Text);
    }
}