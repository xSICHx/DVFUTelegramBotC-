﻿using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Models.Consts;

namespace TelegramBotDVFU.Models.Commands;

public class CurrentMissions : Command
{
    public override string[] Names
    {
        get => new[] {@"Текущие миссии"};
        set => throw new NotImplementedException();
    }

    public override int AdminsCommand => 0;
    public override async Task Execute(Message message, TelegramBotClient botClient)
    {
        var chatId = message.Chat.Id;
        await using var db = new ApplicationUserContext();
        var user = await db.Users.FindAsync(message.Chat.Username);
        var flagAllCompleted = true;
        foreach (var trial in ConstTrials.TrialsList)
        {
            if (!user.TrialsDict.ContainsKey(trial.Name)) continue;
            if (trial.CheckCompleted(message.Chat.Username)) continue;
            flagAllCompleted = false;
            await botClient.SendTextMessageAsync(chatId, trial.Description);
        }
            
        if (flagAllCompleted)
        {
            await botClient.SendTextMessageAsync(chatId, "Вы выполнили все миссии. Вы молодец!");
        }
        await db.SaveChangesAsync();
    }

    public override bool Contains(Message message)
    {
        if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
            return false;

        return message.Text != null && Names.Contains(message.Text);
    }
}