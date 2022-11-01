using Microsoft.AspNetCore.Components.Forms;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotDVFU.Models.Consts;
using TelegramBotDVFU.View.Products;
using File = System.IO.File;


namespace TelegramBotDVFU.Models.Commands;

public class ShowProduct : Command
{
    //todo разобраться с этой хуетой
    // public sealed override string[] Names => new string[ConstAssortiment.Products.Length];
    public override string[] Names
    { get; set; }
    public override int AdminsCommand => 0;
    
    public ShowProduct()
    {
        List<string> lstNames = new List<string>();
        using (ApplicationProductContext dbProduct = new ApplicationProductContext())
        {
            foreach (var product in dbProduct.Products)
            {
                lstNames.Add(product.Name);
                Console.WriteLine(product.Name);
            }

            Names = lstNames.ToArray();
        }
    }
    public override async Task Execute(Message message, TelegramBotClient botClient)
    {
        var chatId = message.Chat.Id;
        Product product = new("Пиво", "Вкусное", 200, 10);
        await using (ApplicationProductContext dbProduct = new ApplicationProductContext())
        {
            foreach (var prdct in dbProduct.Products)
            {
                if (prdct.Name == message.Text)
                {
                    product = prdct;
                    break;
                }
            }
        }

        var img = @"../TelegramBotDVFU/Images/" + product.Name + @".jpg";
        try
        {
            using (var stream = File.OpenRead(img))
            {
                InputOnlineFile inputOnlineFile = new InputOnlineFile(stream);
                await botClient.SendPhotoAsync(chatId, caption: product.Name,
                    photo: inputOnlineFile);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        
        InlineKeyboardMarkup buttons = new(new[]
        {
            new InlineKeyboardButton("Купить " + product.Name){CallbackData = "Купить " + product.Name},
            new InlineKeyboardButton("Вернуть " + product.Name){CallbackData = "Вернуть " + product.Name}
        });
        await botClient.SendTextMessageAsync(chatId,
            "Описание айтема: " + product.Description + "\nОсталось: " + product.Amount +
            "\nСтоимость: " + product.Cost, replyMarkup: buttons);
    }
    

    public override bool Contains(Message message)
    {
        if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
            return false;
        var adminNotGiveGiftFlag = false;
        using (ApplicationUserContext db = new ApplicationUserContext())
        {
            var user =  db.Users.Find(message.Chat.Username);
            if (user.AdminFlag <= 1)
                adminNotGiveGiftFlag = true;
            db.SaveChanges();
        }

        return message.Text != null && Names.Contains(message.Text) && adminNotGiveGiftFlag;
    }
}