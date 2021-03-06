using System;
using System.Linq;
using System.Threading.Tasks;
using BotDataSet;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TGBotGame
{
    public static class PrivateChatFunctions
    {
        public enum Punishments
        {
            Warn,
            Mute,
            Ban
        }


        public static async Task FillBalance(uint amount, ITelegramBotClient botClient, Telegram.Bot.Types.User user, CallbackQuery callbackQuery)
        {
            await botClient.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
            Handlers.users[user.Id].payment = await Assist.AddPayment(amount, user.Id);
            MessageSender.SendMessage(botClient,
                Keyboards.PreparePaymentKeyboardMarkup(
                    $"https://qiwi.com/payment/form/99?extra[%27accountType%27]=phone&extra[%27account%27]={Configuration.QiwiMobile}&amountInteger={amount}&amountFraction=0&extra[%27comment%27]={Handlers.users[user.Id].payment.RId}&blocked[0]=account&blocked[3]=comment&blocked[4]=amountInteger"),
                "Когда вы оплатите и платеж пройдет, я уведомлю вас. Внимание, зачисление средств возможно в течение 3 дней", user);
        }

        public static async Task RemovePunishment(Punishments punishments, ITelegramBotClient botClient,
            Telegram.Bot.Types.
                User user, CallbackQuery callbackQuery)
        {
            await botClient.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
            ActionResult result;
            switch (punishments)
            {
                case Punishments.Ban:
                    if (!user.GetUser().IsBanned)
                    {
                        MessageSender.SendMessage(botClient,Keyboards.PreparePopMenuKeyboard(user), Constants.NO_BANS_TEXT, user);
                        return;
                    }

                    result = await user.RemovePointsAsync(Constants.BAN_REMOVE_PRICE);
                    if (result is OkResult)
                    {
                        user.UnBan();
                        MessageSender.SendMessage(botClient, Keyboards.PrepareUnpunishmentAdminsKeyboard(user),
                            Constants.ADMINS_REQUEST_REMOVE_PUNISHMENT + "бана.\n" + Constants.USERNAME +
                            user.Username + '\n' +
                            Constants.USERID + user.Id, Configuration.AdminsGroupId);
                        MessageSender.SendMessage(botClient, Keyboards.PreparePopMenuKeyboard(user), Constants.SUCCESS_REMOVED_PUNISHMENT_TEXT, user);
                    }
                    else if (result is BadRequestResult)
                    {
                        MessageSender.SendMessage(botClient, Keyboards.PrepareUnpunishmentAdminsKeyboard(user), Constants.UNSUCCESS_REMOVING_PUNISHMENT, user);
                    }
                    else
                    {
                        MessageSender.SendMessage(botClient, Keyboards.PrepareUnpunishmentAdminsKeyboard(user), Constants.NOT_ENOUGH_CREDITS_TEXT, user);
                    }

                    break;
                case Punishments.Mute:
                    if (!user.GetUser().IsMuted)
                    {
                        MessageSender.SendMessage(botClient, Keyboards.PreparePopMenuKeyboard(user), Constants.NO_MUTES_TEXT, user);
                        return;
                    }

                    result = await user.RemovePointsAsync(Constants.MUTE_REMOVE_PRICE);
                    if (result is OkResult)
                    {
                        user.UnMute();
                        MessageSender.SendMessage(botClient, Keyboards.PrepareUnpunishmentAdminsKeyboard(user),
                            Constants.ADMINS_REQUEST_REMOVE_PUNISHMENT + "мута.\n" + Constants.USERNAME +
                            user.Username + '\n' +
                            Constants.USERID + user.Id, Configuration.AdminsGroupId);
                        MessageSender.SendMessage(botClient, Keyboards.PreparePopMenuKeyboard(user), Constants.SUCCESS_REMOVED_PUNISHMENT_TEXT, user);
                    }
                    else if (result is BadRequestResult)
                    {
                        MessageSender.SendMessage(botClient, Keyboards.PreparePopMenuKeyboard(user), Constants.UNSUCCESS_REMOVING_PUNISHMENT, user);
                    }
                    else
                    {
                        MessageSender.SendMessage(botClient, Keyboards.PreparePopMenuKeyboard(user), Constants.NOT_ENOUGH_CREDITS_TEXT, user);
                    }

                    break;
                case Punishments.Warn:
                    if (user.GetUserWarnCount() == 0)
                    {
                        MessageSender.SendMessage(botClient, Keyboards.PreparePopMenuKeyboard(user), Constants.NO_WARNS_TEXT, user);
                        return;
                    }

                    result = await user.RemovePointsAsync(Constants.WARN_REMOVE_PRICE);
                    if (result is OkResult)
                    {
                        await Assist.RemoveWarn(user.Id);
                        MessageSender.SendMessage(botClient, Keyboards.PrepareUnpunishmentAdminsKeyboard(user),
                            Constants.ADMINS_REQUEST_REMOVE_PUNISHMENT + "варна.\n" + Constants.USERNAME +
                            user.Username + '\n' +
                            Constants.USERID + user.Id, Configuration.AdminsGroupId);
                        MessageSender.SendMessage(botClient, Keyboards.PreparePopMenuKeyboard(user), Constants.SUCCESS_REMOVED_PUNISHMENT_TEXT, user);
                    }
                    else if (result is BadRequestResult)
                    {
                        MessageSender.SendMessage(botClient, Keyboards.PreparePopMenuKeyboard(user), Constants.UNSUCCESS_REMOVING_PUNISHMENT, user);
                    }
                    else
                    {
                        MessageSender.SendMessage(botClient, Keyboards.PreparePopMenuKeyboard(user), Constants.NOT_ENOUGH_CREDITS_TEXT, user);
                    }

                    break;
            }

    }

        public static async Task GetFriendsList(ITelegramBotClient botClient, Telegram.Bot.Types.User user,
            CallbackQuery callbackQuery)
        {
            await botClient.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
            var friendsList = user.GetFriendsList();
            if (friendsList.Count()<1)
            {
                MessageSender.SendMessage(botClient,Keyboards.PreparePopMenuKeyboard(user), Constants.FRIENDS_LIST_EMPTY, user);
                return;
            }
            string frList = "";
            foreach (var fr in friendsList)
            {
                frList += fr.UserName + '\n';
            }

            MessageSender.SendMessage(botClient,Keyboards.PreparePopMenuKeyboard(user),
                Constants.FRIEND_LIST_TEXT + '\n' + frList, user);
    }

        public static async Task GetRemoveList(ITelegramBotClient botClient, Telegram.Bot.Types.User user, CallbackQuery callbackQuery)
        {
            await botClient.DeleteMessageAsync(callbackQuery.Message.Chat, callbackQuery.Message.MessageId);
            var kb = Keyboards.PrepareRemoveFriendsList(user);
            if (kb==null)
            {
                MessageSender.SendMessage(botClient,Keyboards.PreparePopMenuKeyboard(user), Constants.FRIENDS_LIST_EMPTY, user);
                return;
            }
            Handlers.users[user.Id].curState = KeyboardsNavigator.CurentState.RemoveFriends;
            MessageSender.SendMessage(botClient, kb , Constants.REMOVE_FRIEND_TEXT,
                user);
        }

        public static async Task RemoveFriend(ITelegramBotClient botClient, long whomDelete,
            Telegram.Bot.Types.User user, CallbackQuery callbackQuery)
        {
            ActionResult result;
            result = await user.RemoveFriendAsync(whomDelete);
            if (result is OkResult)
            {
                MessageSender.SendMessage(botClient, Keyboards.PreparePopMenuKeyboard(user), Constants.FRIEND_REMOVED_TEXT, user);
                await botClient.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                await GetRemoveList(botClient, user, callbackQuery);
            }
            else if (result is Assist.AlreadyResult)
            {
                MessageSender.SendMessage(botClient, Keyboards.PreparePopMenuKeyboard(user), Constants.FRIEND_REMOVED_ALREADY, user);
            }
            else
            {
                MessageSender.SendMessage(botClient, Keyboards.PreparePopMenuKeyboard(user), Constants.FRIEND_REMOVED_EXCEPTION, user);
            }
        }

        public static async Task VokeToNextGame(ITelegramBotClient botClient, Telegram.Bot.Types.User user,
            CallbackQuery callbackQuery)
        {
            await botClient.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
            await user.AddNextGame();
            MessageSender.SendMessage(botClient, Keyboards.PreparePopMenuKeyboard(user), Constants.VOKE_NEXT_GAME_TEXT, user);
    }

        public static async Task GetReason(Punishments punishments, ITelegramBotClient botClient,
            Telegram.Bot.Types.User user, CallbackQuery callbackQuery)
        {
            await botClient.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
            switch (punishments)
            {
                case Punishments.Ban:
                    var ban = user.IsUserBanned();
                    if (ban)
                    {
                        string reason = "У тебя бан за " + user.GetUser().BanReason + " до " + user.GetUnBanDate();
                        MessageSender.SendMessage(botClient, Keyboards.PreparePopMenuKeyboard(user), reason, user);
                            }
                    else
                    {
                        MessageSender.SendMessage(botClient, Keyboards.PreparePopMenuKeyboard(user), Constants.NO_BANS_TEXT, user);
                            }

                    break;
                case Punishments.Mute:
                    var mute = user.IsUserMuted();
                    if (mute)
                    {
                        string reason = "У тебя мут за " + user.GetUser().MuteReason + " до " + user.GetUnMuteDate();
                        MessageSender.SendMessage(botClient, Keyboards.PreparePopMenuKeyboard(user), reason, user);
                            }
                    else
                    {
                        MessageSender.SendMessage(botClient, Keyboards.PreparePopMenuKeyboard(user), Constants.NO_MUTES_TEXT, user);
                            }

                    break;
                case Punishments.Warn:
                    var warn = user.GetUserWarnCount();
                    if (warn > 0)
                    {
                        string reason = "У тебя есть " + user.GetUserWarnCount() + " варн(а): \n" +
                                        String.Join("\n", user.GetUserWarnsReasons());
                        MessageSender.SendMessage(botClient, Keyboards.PreparePopMenuKeyboard(user), reason, user);
                            }
                    else
                    {
                        MessageSender.SendMessage(botClient, Keyboards.PreparePopMenuKeyboard(user), Constants.NO_WARNS_TEXT, user);
                            }

                    break;
            }
        }

        public static async Task GetRules(ITelegramBotClient botClient, Telegram.Bot.Types.User user,
            CallbackQuery callbackQuery)
        {
            await botClient.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
            MessageSender.SendMessage(botClient, Keyboards.PrepareLinkKeyboardMarkup(Configuration.RulesLink),
                "Нажмите на кнопку, чтобы перейти на канал с правилами", user);
        }

        public static async Task GetRolesDescription(ITelegramBotClient botClient, Telegram.Bot.Types.User user,
            CallbackQuery callbackQuery)
        {
            await botClient.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
            MessageSender.SendMessage(botClient, Keyboards.PrepareLinkKeyboardMarkup(Configuration.RolesDescLink),
                "Нажмите на кнопку, чтобы перейти на канал с описанием ролей", user);
        }
    }
}