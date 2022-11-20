namespace TelegramBotDVFU.Models.Consts;

public static class ConstReplyes
{
    private static readonly Random Rnd = new();
    private static string[] Replyes { get; } = {"Хорошо, чумба", "Так точно, кэп", "Как скажешь, мейт"};

    public static string GetRandomReply()
    {
        return Replyes[Rnd.Next(Replyes.Length)];
    }
}