using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Models.Consts;
using TelegramBotDVFU.Models;

namespace TelegramBot.Controllers;

public class MessageController : Controller
{
    private static async Task  HandleUpdateAsync()
    {
        var f = new StreamReader("logs.txt");
        while (!f.EndOfStream)
        {
            var s = await f.ReadLineAsync();
            if (s is "\n" or null)
            {
                continue;
            }

            var update = Newtonsoft.Json.JsonConvert.DeserializeObject<Update>(s);
            if (update == null)
                continue;
            if (update.Type is UpdateType.MyChatMember)
                continue;

            var trash = update.Message?.Chat.Username ?? update.CallbackQuery?.From.Username;
            if (trash is null) continue;
            if (update.Type is not (UpdateType.Message or UpdateType.CallbackQuery)) continue;
            
            var commands = Bot.Commands;
            var queries = Bot.Queries;
            try
            {
                switch (update.Type)
                {
                    case UpdateType.Message:
                        var message = update.Message;
                        await using (var dbUsr = new ApplicationUserContext())
                        {
                            
                            var user =  await dbUsr.Users.FindAsync(message.Chat.Username);
                            if (user == null)
                            {
                                if (message.Chat.Username != null)
                                {
                                    dbUsr.Users.Add(new Usr(message.Chat.Username, message.Chat.Id,
                                        "Главное меню"));
                                    user = await dbUsr.Users.FindAsync(message.Chat.Username);
                                }
                                else
                                {
                                    dbUsr.Users.Add(
                                        new Usr(message.Chat.Id.ToString(), message.Chat.Id, "Главное меню"));
                                    user = await dbUsr.Users.FindAsync(message.Chat.Id.ToString());
                                }

                                foreach (var nick in ConstAdmins.AdminsNickname)
                                {
                                    if (message.Chat.Username == nick)
                                        user.AdminFlag = 1;
                                }
                                MyProducers.Produce("trials-input", "people-counter","people-counter");
                                MyProducers.Produce("people-activity-input", user.Id, JsonConvert.SerializeObject(user));
                                await dbUsr.SaveChangesAsync();
                            }
                            
                            if (user.AdminFlag > 0)
                            {
                                await using var dbAdmin = new ApplicationAdminContext();
                                var admin = await dbAdmin.Admins.FindAsync(message.Chat.Username);
                                if (admin == null)
                                {
                                    dbAdmin.Admins.Add(new Admin(message.Chat.Username, message.Chat.Id));
                                    await dbAdmin.SaveChangesAsync();
                                }
                            }

                            var flagCommandCompleted = 0;
                            foreach (var command in commands)
                            {
                                if (!command.Contains(message)) continue;
                                if (user.AdminFlag == 0 && command.AdminsCommand == 1)
                                    break;
                                if (user.AdminFlag > 0 && command.AdminsCommand == 0)
                                {
                                    user.AdminFlag = 1;
                                    await dbUsr.SaveChangesAsync();
                                }

                                try
                                {
                                    await command.Execute(message);
                                    var regex = new Regex(@"@\w*");
                                    if (regex.IsMatch(message.Text))
                                        MyProducers.Produce("command-counter-input", "AdminNickname", "AdminNickname");
                                    else 
                                        MyProducers.Produce("command-counter-input", message.Text, message.Text);
                                    flagCommandCompleted = 1;
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                                await dbUsr.SaveChangesAsync();
                            }
                            if (flagCommandCompleted == 1)
                                break;
                            await dbUsr.SaveChangesAsync();
                        }
                        await TelegramBotDVFU.Models.Commands.Ok.Execute(update.Message);
                        MyProducers.Produce("command-counter-input", "Ok", "Ok");

                        break;


                    case UpdateType.CallbackQuery:
                        foreach (var query in queries)
                        {
                            if (!query.Contains(update)) continue;
                            try
                            {
                                await query.Execute(update);
                                MyProducers.Produce("command-counter-input", update.CallbackQuery.Data, update.CallbackQuery.Data);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }

                            break;
                        }

                        break;
                    case UpdateType.Unknown:
                    case UpdateType.InlineQuery:
                    case UpdateType.ChosenInlineResult:
                    case UpdateType.EditedMessage:
                    case UpdateType.ChannelPost:
                    case UpdateType.EditedChannelPost:
                    case UpdateType.ShippingQuery:
                    case UpdateType.PreCheckoutQuery:
                    case UpdateType.Poll:
                    case UpdateType.PollAnswer:
                    case UpdateType.MyChatMember:
                    case UpdateType.ChatMember:
                    case UpdateType.ChatJoinRequest:
                    default:
                        break;
                }
            }
            catch
            {
                // ignored
            }
            
        }
        Console.WriteLine("END");
        f.Close();
    }

    public static async Task StartBot()
    {
        MyProducers.StartProducers();
        // string userId, userSerial;
        //
        // await using (var dbUsr = new ApplicationUserContext())
        // {
        //     var user = await dbUsr.Users.FindAsync("sich_sweetheart");
        //     Console.WriteLine(user.Id);
        //     userId = user.Id;
        //     userSerial = JsonConvert.SerializeObject(user);
        //     await dbUsr.SaveChangesAsync();
        // }
        // MyProducers.Produce("command-counter-input", "asdf", "asdf");
        // MyProducers.Produce("people-activity-input", userId, userSerial);
        // Console.WriteLine(1);
        
        Bot.GetBotClient();
        await HandleUpdateAsync();
    }
}