using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Telegram.Bot.Types;
namespace BotDataSet
{
    public static class Assist
    {
        public static BotUser GetUser(this User user)
        {
            using (BotDBContext cont = new BotDBContext())
            {
                if (cont.Users.Any(x => x.Id == user.Id && x.UserName == user.Username))
                {
                    return cont.Users.FirstOrDefault(x => x.UserName == user.Username);
                }
                else
                {
                    var NewUser = cont.Users.Add(new BotUser() { Id = user.Id, UserName = user.Username });
                    cont.SaveChanges();
                    return NewUser.Entity;
                }
            }
        }
        public static bool IsUserMuted(this User user)
        {
            return GetUser(user).IsMuted;
        }

        public static bool IsUserBanned(this User user)
        {
            return GetUser(user).IsBanned;
        }

        public static byte GetUserWarnCount(this User user)
        {
            return GetUser(user).WarnCount;
        }

        public static uint GetPoints(this User user)
        {
            return GetUser(user).Points;
        }

        public static long GetChatId(this User user)
        {
            return GetUser(user).Id;
        }
    }
}
