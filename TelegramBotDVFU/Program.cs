
using TelegramBot.Controllers;

namespace TelegramBotDVFU;

static class Program
{
    static async Task Main(string[] args)
    {
        // MyProducers.StartProducers();
        // Thread.Sleep(5000);
        // MyProducers.Produce("command-counter-input", "asdf", "asdf");
        await MessageController.StartBot();
    }
}