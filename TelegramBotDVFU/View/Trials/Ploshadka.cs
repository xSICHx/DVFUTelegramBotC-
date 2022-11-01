using TelegramBot.Models.Consts;
using TelegramBotDVFU.Models;

namespace TelegramBot.View;

public class Ploshadka: TrialConst
{
    public Ploshadka(string name, string description, int reward, int additional) : base(name, description, reward, additional) { }

    public override bool CheckCompleted(string telegramName)
    {
        using var db = new ApplicationUserContext();
        var user = db.Users.Find(telegramName);
        return user.TrialsDict[Name] != 0;
    }

    public override bool CheckIfAllCompleted(string telegramName)
    {
        using var db = new ApplicationUserContext();
        var user = db.Users.Find(telegramName);
        foreach (var trial in ConstTrials.TrialsList)
        {
            if (trial.GetType() != typeof(Ploshadka))
                break;
            if (user.TrialsDict[trial.Name] == 0)
                return false;
        }

        return true;
    }
}