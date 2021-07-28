using Microsoft.AspNetCore.Mvc;

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
        public class NotFoundResult : ActionResult
        {
            public string Content { get; } = "Not found";
        }
        public class AlreadyResult : ActionResult
        {
            public string Content { get; } = "Already";
        }
        public class NotEnoughResult : ActionResult
        {
            public string Content { get; } = "Not enough points";
        }
        public static BotUser GetUser(this User user)
        {
            using (BotDBContext cont = new BotDBContext())
            {
                if (cont.Users.Any(x => x.UserId == user.Id))
                {
                    return cont.Users.FirstOrDefault(x => x.UserId == user.Id);
                }
                else
                {
                    var NewUser = cont.Users.Add(new BotUser() { UserId = user.Id, UserName = user.Username });
                    cont.SaveChanges();
                    return NewUser.Entity;
                }
            }
        }
        private static BotUser GetUser(string UserName)
        {
            using (BotDBContext cont = new BotDBContext())
            {
                if (cont.Users.Any(x => x.UserName == UserName))
                {
                    return cont.Users.FirstOrDefault(x => x.UserName == UserName);
                }
                else
                {
                    throw new Exception("404");
                }
            }
        }
        private static BotUser GetUser(long userId)
        {
            using (BotDBContext cont = new BotDBContext())
            {
                if (cont.Users.Any(x => x.UserId == userId))
                {
                    return cont.Users.FirstOrDefault(x => x.UserId == userId);
                }
                else
                {
                    throw new Exception("404");
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
        public static async Task<ActionResult> AddPointsAsync(this User user, uint value)
        {
            var botUser = GetUser(user);
            botUser.Points += value;
            using(var cont =  new BotDBContext())
            {
                cont.Users.Update(botUser);
                cont.SaveChanges();
            }
            return new OkResult();
        }
        public static async Task<ActionResult> RemovePointsAsync(this User user, uint value)
        {
            if (value > 0)
            {
                var botUser = GetUser(user);
                if (botUser.Points < value)
                {
                    return new NotEnoughResult();
                }
                if (botUser.Points >= value)
                {
                    botUser.Points -= value;
                    using (var cont = new BotDBContext())
                    {
                        cont.Users.Update(botUser);
                        cont.SaveChanges();
                        return new OkResult();
                    }
                }
            }
            return new BadRequestResult();
        }

        public static long GetChatId(this User user)
        {
            return GetUser(user).UserId;
        }

        public static IEnumerable<BotUser> GetFriendsList(this User user)
        {
            var botuser = GetUser(user);
            IEnumerable<BotUser> result;
            using (BotDBContext cont = new BotDBContext())
            {
                result = cont.Friends.Where(x => x.User == botuser).Select(u => u.Friend).ToList();
                return result;
            }
        }

        public static async Task<ActionResult> AddFriend(this User user, string UserName)
        {
            try
            {
                var friend = GetUser(UserName);
                var BotUser = GetUser(user);
                using(var cont =  new BotDBContext())
                {
                    if(cont.Friends.Any(x => x.UserId == BotUser.UserId && x.FriendId == friend.UserId))
                    {
                        return new AlreadyResult();
                    }
                    cont.Friends.Add(new Friends() { UserId = BotUser.UserId, FriendId = friend.UserId });
                    cont.SaveChanges();
                    return new OkResult();
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "404")
                {
                    return new NotFoundResult();
                }
                else
                {
                    throw;
                }
            }
        }

        public static async Task<ActionResult> RemoveFriendAsync(this User user, long userId)
        {
            try
            {
                var friend = GetUser(userId);
                var BotUser = GetUser(user);
                using (var cont = new BotDBContext())
                {
                    if (cont.Friends.All(x => x.UserId != BotUser.UserId && x.FriendId != friend.UserId))
                    {
                        return new AlreadyResult();
                    }
                    var fr = cont.Friends.First(x => x.UserId == BotUser.UserId && x.FriendId == friend.UserId);
                    cont.Friends.Remove(fr);
                    cont.SaveChanges();
                    return new OkResult();
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "404")
                {
                    return new NotFoundResult();
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
