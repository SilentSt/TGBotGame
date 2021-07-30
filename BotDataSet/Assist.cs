﻿using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Telegram.Bot.Types;
namespace BotDataSet
{
    public static class Assist
    {
        public static Regex PhoneValidation = new Regex(@"^(\+7|7|8)?[\s\-]?\(?[489][0-9]{2}\)?[\s\-]?[0-9]{3}[\s\-]?[0-9]{2}[\s\-]?[0-9]{2}$");
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
        public static BotUser GetUser(long userId)
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
        public static void Mute(this User user, DateTime? unMuteDate = null)
        {
            var botUser = GetUser(user);
            botUser.IsMuted = true;
            if (unMuteDate != null)
            {
                botUser.UnMutedDate = (DateTime)unMuteDate;
            }
            using (var cont = new BotDBContext())
            {
                cont.Users.Update(botUser);
                cont.SaveChanges();
            }
        }
        public static void Mute(this BotUser botUser, DateTime? unMuteDate = null)
        {
            botUser.IsMuted = true;
            if (unMuteDate != null)
            {
                botUser.UnMutedDate = (DateTime)unMuteDate;
            }
            using (var cont = new BotDBContext())
            {
                cont.Users.Update(botUser);
                cont.SaveChanges();
            }
        }
        public static void Mute(long id, DateTime? unMuteDate = null)
        {
            var botUser = GetUser(id);
            botUser.Mute(unMuteDate);
        }
        public static bool IsUserMuted(this User user)
        {
            return GetUser(user).IsMuted;
        }
        public static DateTime? GetUnMuteDate(this User user)
        {
            return GetUser(user).UnMutedDate;
        }
        public static void Ban(this User user, DateTime? unBanDate = null)
        {
            var botUser = GetUser(user);
            botUser.IsBanned = true;
            if (unBanDate != null)
            {
                botUser.UnBanDate = (DateTime)unBanDate;
            }
            using (var cont = new BotDBContext())
            {
                cont.Users.Update(botUser);
                cont.SaveChanges();
            }
        }
        public static void Ban(this BotUser botUser, DateTime? unBanDate = null)
        {
            botUser.IsBanned = true;
            if (unBanDate != null)
            {
                botUser.UnBanDate = (DateTime)unBanDate;
            }
            using (var cont = new BotDBContext())
            {
                cont.Users.Update(botUser);
                cont.SaveChanges();
            }
        }
        public static void Ban(long id, DateTime? unBanDate = null)
        {
            var botUser = GetUser(id);
            botUser.Ban(unBanDate);
        }
        public static bool IsUserBanned(this User user)
        {
            return GetUser(user).IsBanned;
        }
        public static DateTime? GetUnBanDate(this User user)
        {
            return GetUser(user).UnBanDate;
        }
        public static int GetUserWarnCount(this User user)
        {
            return GetUser(user).Warns.Count;
        }
        public static List<string> GetUserWarnsReasons(this User user)
        {
            return GetUser(user).Warns.Select(x => x.Reason).ToList();
        }
        public static async Task<ActionResult> AddWarn(this User user, string reason)
        {
            if (!string.IsNullOrWhiteSpace(reason))
            {
                var botuser = GetUser(user);
                if (botuser.Warns.Count == 3)
                {
                    return new AlreadyResult();
                }
                using (var cont = new BotDBContext())
                {
                    await cont.Warns.AddAsync(new Warn()
                    {
                        UserId = user.Id,
                        Reason = reason
                    });
                    await cont.SaveChangesAsync();
                    return new OkResult();
                }
            }
            else
            {
                return new BadRequestResult();
            }
        }
        public static async Task<ActionResult> RemoveWarn(this User user)
        {
            var botUser = GetUser(user);
            if (botUser.Warns.Count < 1)
            {
                return new AlreadyResult();
            }
            using (var cont = new BotDBContext())
            {
                var warn = cont.Warns.OrderBy(x => x.Id).First();
                cont.Warns.Remove(warn);
                await cont.SaveChangesAsync();
                return new OkResult();
            }
        }
        public static uint GetPoints(this User user)
        {
            return GetUser(user).Points;
        }
        public static async Task<ActionResult> AddPointsAsync(this User user, uint value)
        {
            var botUser = GetUser(user);
            botUser.Points += value;
            using (var cont = new BotDBContext())
            {
                cont.Users.Update(botUser);
                await cont.SaveChangesAsync();
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
                        await cont.SaveChangesAsync();
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
                using (var cont = new BotDBContext())
                {
                    if (cont.Friends.Any(x => x.UserId == BotUser.UserId && x.FriendId == friend.UserId))
                    {
                        return new AlreadyResult();
                    }
                    await cont.Friends.AddAsync(new Friends() { UserId = BotUser.UserId, FriendId = friend.UserId });
                    await cont.SaveChangesAsync();
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
        public static async Task<ActionResult> AddFriend(this User user, User Friend)
        {
            var friend = GetUser(Friend);
            var BotUser = GetUser(user);
            using (var cont = new BotDBContext())
            {
                if (cont.Friends.Any(x => x.UserId == BotUser.UserId && x.FriendId == friend.UserId))
                {
                    return new AlreadyResult();
                }
                await cont.Friends.AddAsync(new Friends() { UserId = BotUser.UserId, FriendId = friend.UserId });
                await cont.SaveChangesAsync();
                return new OkResult();
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
                    await cont.SaveChangesAsync();
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
        public static async Task<Payment> AddPayment(string phone, uint sum)
        {
            if (sum % 50 != 0) throw new Exception("Invalid sum");
            if (!PhoneValidation.IsMatch(phone)) throw new Exception("Invalid number");
            var rId = (new Random()).Next(1000000, 9999999);
            using (var cont = new BotDBContext())
            {
                while (cont.Payments.Any(x => x.RId == rId))
                {
                    rId = (new Random()).Next(1000000, 9999999);
                }
                if (cont.Payments.All(x => x.RId != rId))
                {
                    var paym = new Payment() { RId = (uint)rId };
                    await cont.Payments.AddAsync(paym);
                    await cont.SaveChangesAsync();
                    return paym;
                }
            }
            throw new Exception("Something went wrong");
        }
        public static async Task<ActionResult> RemovePayment(this Payment payment)
        {
            using (var cont = new BotDBContext())
            {
                if (cont.Payments.Any(x => x.Equals(payment)))
                {
                    cont.Payments.Remove(payment);
                    await cont.SaveChangesAsync();
                    return new OkResult();
                }
                else
                {
                    return new NotFoundResult();
                }
            }
        }
        public static void Save(this BotUser user)
        {
            using(var cont  = new BotDBContext())
            {
                cont.Users.Update(user);
                cont.SaveChanges();
            }
        }
    }
}
