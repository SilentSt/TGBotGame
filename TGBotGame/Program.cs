using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using QiwiApi.Events;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;

namespace TGBotGame
{
    public static class Program
    {
        private static TelegramBotClient? Bot;

        public static async Task Main()
        {
            Bot = new TelegramBotClient("1435986427:AAFJXkzS1HuxjE1H-4qULAtKDt2KXFGhYjw");

            var me = await Bot.GetMeAsync();
            Console.Title = me.Username;

            using var cts = new CancellationTokenSource();

            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            Bot.StartReceiving(new DefaultUpdateHandler(Handlers.HandleUpdateAsync, Handlers.HandleErrorAsync),
                cts.Token);
            //await Bot.SendTextMessageAsync(Configuration.GroupId, "/info RUBLbBudet");
            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();

            // Send cancellation request to stop bot
            cts.Cancel();
        }
    }
}