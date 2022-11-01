namespace TelegramBot.View;

public abstract class TrialConst
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Reward { get; set; }
    public int AdditionalReward { get; set; }
    

    public TrialConst(string name, string description, int reward, int additionalReward)
    {
        Name = name;
        Description = description;
        Reward = reward;
        AdditionalReward = additionalReward;
    }

    public abstract bool CheckCompleted(string telegramName);
    public abstract bool CheckIfAllCompleted(string telegramName);
}
