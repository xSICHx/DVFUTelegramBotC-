using System.Runtime.InteropServices;
using System.Text;
using Telegram.Bot.Types;

namespace TelegramBotDVFU.Models.Commands;

public static class HandleCallbackQuery
{
    public static string Handle(Update update)
    {
        var callbackQuery = update.CallbackQuery;
        //all cbData looks like type_data
        var cbData = callbackQuery.Data.Split(new []{'_'});
        var (type, data) = (cbData[0], cbData[1]);
        var messageText = callbackQuery.Message.Text;
        switch (type)
        {
            case "solve":
                switch (data)
                {  
                }

                break;
        }

        return messageText;
    }
}