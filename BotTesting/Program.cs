using System;
using System.Linq;
using System.Threading.Tasks;
using BotDataSet;
using QiwiApi;
using QiwiApi.Events;
using Telegram.Bot.Types;
namespace BotTesting
{
    class Program
    {
        static async Task Main(string[] args)
        {
            /*using (var cont = new BotDBContext())
            {
                cont.Users.Add(new BotUser() { UserId = 123, UserName = "testusername" });
                cont.Users.Add(new BotUser() { UserId = 124, UserName = "testfriendusername" });
                cont.Users.Add(new BotUser() { UserId = 125, UserName = "testfrdusername" });
                cont.Users.Add(new BotUser() { UserId = 126, UserName = "testfrendurname" });
                cont.SaveChanges();
                
                cont.Friends.Add(new Friends() { UserId = 123, FriendId = 124 }) ;
                cont.Friends.Add(new Friends() { UserId = 123, FriendId = 125 });
                cont.Friends.Add(new Friends() { UserId = 123, FriendId = 126 });
                cont.SaveChanges();
            }
            var user = new User() { Id = 123, Username = "testusername" };
            var u = user.GetUser();
            Console.WriteLine(u.UserId);
            user.GetFriendsList().ToList().ForEach(x => Console.WriteLine(x.UserName));
            */
            var qiwi = new QiwiApi.Qiwi("79185617179","572d68e7f687c369faa28bf9b5636251");
            qiwi.OnIncomingPayment += Qiwi_OnIncomingPayment;
            qiwi.StartHistoryPolling(TimeSpan.FromMinutes(5));
            
            Console.ReadKey();
            //Console.WriteLine(hs.Payments.FirstOrDefault().Sum);
        }

        private static void Qiwi_OnIncomingPayment(object sender, PaymentEventArgs e)
        {
            Console.WriteLine(e.Payment.Account);
        }
    }
}
