namespace TelegramBot.View;

public abstract class TrialConst
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string CompletedMessage { get; set; }
    public int Reward { get; set; }
    public int AdditionalReward { get; set; }
    

    public TrialConst(string name, string description, string completedMessage, int reward, int additionalReward)
    {
        Name = name;
        Description = description;
        CompletedMessage = completedMessage;
        Reward = reward;
        AdditionalReward = additionalReward;
    }

    public abstract bool CheckCompleted(string telegramName);
    public abstract bool CheckIfAllCompleted(string telegramName);
}
