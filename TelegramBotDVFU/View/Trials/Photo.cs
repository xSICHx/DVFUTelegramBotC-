using TelegramBotDVFU.Models;

namespace TelegramBot.View;

public class Photo: TrialConst
{
    public Photo(string name, string description, string completedMessage, int reward, int additional) :
        base(name, description, completedMessage, reward, additional) { }

    public override bool CheckCompleted(string telegramName)
    {
        using var db = new ApplicationUserContext();
        var user = db.Users.Find(telegramName);
        return user.TrialsDict[Name] == 7;
    }
    

    public override bool CheckIfAllCompleted(string telegramName)
    {
        return CheckCompleted(telegramName);
    }
}