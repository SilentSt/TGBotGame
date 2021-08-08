using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BotDataSet;
using QiwiApi;
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
            var qiwi = new Qiwi(Configuration.QiwiMobile, Configuration.QiwiToken);
            qiwi.StartHistoryPolling(cancellationToken: cts.Token);
            qiwi.OnIncomingPayment += QiwiOnOnIncomingPayment;
            //await Bot.SendTextMessageAsync(Configuration.GroupId, "/info RUBLbBudet");
            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();

            // Send cancellation request to stop bot
            cts.Cancel();
        }

        private static void QiwiOnOnIncomingPayment(object? sender, PaymentEventArgs e)
        {
            try
            {
                var user = Handlers.users.Values.First(x => x.payment.Sum == e.Payment.Sum && x.payment.RId.ToString() == e.Payment.Comment);
                user._user.AddPointsAsync(uint.Parse(e.Payment.Sum.ToString()));
                MessageSender.SendMessage(Bot, Constants.SUCCESS_FILLING_BALANCE_TEXT, user._user);
                user.payment.RemovePayment();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}