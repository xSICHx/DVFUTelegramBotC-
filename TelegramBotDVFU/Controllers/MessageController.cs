using System.Text;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Models.Consts;
using TelegramBotDVFU.Models;

namespace TelegramBotDVFU.Controllers;

public class MessageController : Controller
{
    private static void  HandleUpdateAsync()
    {
        StreamReader f = new StreamReader("logs.txt");
        while (!f.EndOfStream)
        {
            var s = f.ReadLine();
            if (s is "\n" or null)
            {
                continue;
            }

            var update = Newtonsoft.Json.JsonConvert.DeserializeObject<Update>(s);
            if (update == null)
                continue;
            if (update.Type is UpdateType.MyChatMember)
                continue;
            // Console.WriteLine(s);

            var tresh = update.Message?.Chat.Username ?? update.CallbackQuery?.From.Username;
            if (tresh is not null)
            {
                if (update.Type is UpdateType.Message or UpdateType.CallbackQuery)
                {
                    var commands = Bot.Commands;
                    var queries = Bot.Queries;
                    try
                    {
                        switch (update.Type)
                        {
                            case UpdateType.Message:
                                var message = update.Message;
                                using (ApplicationUserContext dbUsr = new ApplicationUserContext())
                                {
                                    var user = dbUsr.Users.Find(message.Chat.Username ?? message.Chat.Id.ToString());
                                    if (user == null)
                                    {
                                        if (message.Chat.Username != null)
                                        {
                                            dbUsr.Users.Add(new Usr(message.Chat.Username, message.Chat.Id,
                                                "Главное меню"));
                                            user = dbUsr.Users.Find(message.Chat.Username);
                                        }
                                        else
                                        {
                                            dbUsr.Users.Add(
                                                new Usr(message.Chat.Id.ToString(), message.Chat.Id, "Главное меню"));
                                            user = dbUsr.Users.Find(message.Chat.Id.ToString());
                                        }

                                        foreach (var nick in ConstAdmins.AdminsNickname)
                                        {
                                            if (message.Chat.Username == nick)
                                                user.AdminFlag = 1;
                                        }

                                        dbUsr.SaveChanges();
                                    }
                                    
                                    if (user.AdminFlag > 0)
                                    { 
                                        using (ApplicationAdminContext dbAdmin = new ApplicationAdminContext())
                                        {
                                            var admin = dbAdmin.Admins.Find(message.Chat.Username);
                                            if (admin == null)
                                            {
                                                dbAdmin.Admins.Add(new Admin(message.Chat.Username, message.Chat.Id));
                                                dbAdmin.SaveChanges();
                                            }
                                        }
                                    }

                                    foreach (var command in commands)
                                    {
                                        if (!command.Contains(message)) continue;
                                        if (user.AdminFlag == 0 && command.AdminsCommand == 1)
                                            break;
                                        if (user.AdminFlag > 0 && command.AdminsCommand == 0)
                                        {
                                            user.AdminFlag = 1;
                                            dbUsr.SaveChanges();
                                        }

                                        try
                                        { 
                                            
                                            command.Execute(message);
                                            if (user.AdminFlag > 0)
                                                Console.WriteLine(user.AdminFlag);
                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine(e.Message);
                                        }

                                        break;
                                    }

                                    dbUsr.SaveChanges();
                                }

                                Models.Commands.Ok.Execute(update.Message);
                                break;


                            case UpdateType.CallbackQuery:
                                foreach (var query in queries)
                                {
                                    if (!query.Contains(update)) continue;
                                    try
                                    {
                                        query.Execute(update);
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(e.Message);
                                    }

                                    break;
                                }

                                break;
                            default:
                                break;
                        }
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
        }

        f.Close();
    }

    public static void StartBot()
    {
        Bot.GetBotClient();
        HandleUpdateAsync();
        Console.ReadLine();
    }
}