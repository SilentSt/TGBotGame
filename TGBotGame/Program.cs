using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BotDataSet;
using QiwiApi;
using QiwiApi.Events;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using System.Timers;

namespace TGBotGame
{
    public static class Program
    {
        private static TelegramBotClient? Bot;

        public static async Task Main()
        {
            Bot = new TelegramBotClient(Configuration.BotToken);
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
            // Create a timer with a two second interval.
            var aTimer = new System.Timers.Timer(60000);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += ATimerOnElapsed;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
            Console.ReadLine();

            // Send cancellation request to stop bot
            cts.Cancel();
        }

        private static void ATimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            Assist.GetAllUsers().ForEach(user =>
            {
                if (user.IsMuted&&user.UnMutedDate<DateTime.Now)
                {
                    user.UnMute();
                }

                if (user.IsBanned&&user.UnBanDate<DateTime.Now)
                {
                    user.UnBan();
                }
            } );
        }


        private static async void QiwiOnOnIncomingPayment(object? sender, PaymentEventArgs e)
        {
            try
            {
                var payList = await Assist.GetListPaymentsAsync();
                var payment = payList.First(x => x.Sum == e.Payment.Sum && x.RId.ToString() == e.Payment.Comment);
                await Assist.AddPointsAsync(payment.UserId, payment.Sum / 10);
                MessageSender.SendMessage(Bot, Constants.SUCCESS_FILLING_BALANCE_TEXT, payment.UserId);
                await payment.RemovePayment();
            }
            catch (Exception exception)
            {
                //Console.WriteLine(exception.Message);
            }
        }
    }
}