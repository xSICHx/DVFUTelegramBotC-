using TelegramBotDVFU.Models;

namespace TelegramBot.View;

public class ContestWinner: TrialConst
{
    public ContestWinner(string name, string description, int reward, int additionalReward) : base(name, description, reward, additionalReward) { }

    public override bool CheckCompleted(string telegramName)
    {
        using var db = new ApplicationUserContext();
        var user = db.Users.Find(telegramName);
        return user.TrialsDict[Name] != 0;
    }

    public override bool CheckIfAllCompleted(string telegramName)
    {
        return CheckCompleted(telegramName);
    }
}