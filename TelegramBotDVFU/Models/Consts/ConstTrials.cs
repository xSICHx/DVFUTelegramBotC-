using TelegramBot.View;

namespace TelegramBot.Models.Consts;

public static class ConstTrials
{
    public static List<TrialConst> TrialsList => new()
    {
        // new Ploshadka("VR", "VR зона", 100, 50),
        // new Ploshadka("PS", "PS зона", 100, 50),
        // new Ploshadka("просмотр", "просмотр зона", 100, 50),
        // new Ploshadka("лекторий", "зона лекторий", 100, 50),
        // new Ploshadka("фото", "фото зона", 100, 50),
        // new Ploshadka("СО", "хуй пойми, что за зона", 100, 50),
        new AskAQuestion("Задать вопрос", "За хорошие вопросы вы получаете валюту", 20, 20),
        // new ContestWinner("Победа в конкурсе", "победа в конкурсе", 300, 0)
    };

    public static TrialConst Find(string name)
    {
        foreach (var trial in TrialsList)
        {
            if (trial.Name == name)
                return trial;
        }

        return TrialsList[0];
    }
}