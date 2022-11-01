using TelegramBotDVFU.Models;

namespace TelegramBot.View;

public class AskAQuestion: TrialConst
{
    public AskAQuestion(string name, string description, int reward, int additional) : base(name, description, reward, additional) { }

    public override bool CheckCompleted(string telegramName)
    {
        using var db = new ApplicationUserContext();
        var user = db.Users.Find(telegramName);
        return user.TrialsDict[Name] == 5;
    }
    

    public override bool CheckIfAllCompleted(string telegramName)
    {
        return CheckCompleted(telegramName);
    }
}