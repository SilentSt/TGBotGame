using System;
using System.Threading.Tasks;
using BotDataSet;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
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

        public enum Amount
        {
            Five,
            Ten,
            Twenty
        }

        public static async Task FillBalance(Amount amount, ITelegramBotClient botClient, Telegram.Bot.Types.User user)
        {
            ActionResult result;
            switch (amount)
            {
                case Amount.Five:
                    //donate request
                    if (true)
                    {
                        result = await user.AddPointsAsync(5);
                        if (result is OkResult)
                        {
                            MessageSender.SendMessage(botClient, Constants.SUCCESS_FILLING_BALANCE_TEXT, user);
                        }
                        else
                        {
                            MessageSender.SendMessage(botClient, Constants.UNSUCCESS_FILING_BALANCE_TEXT, user);
                        }
                        
                    }
                    else
                    {
                        MessageSender.SendMessage(botClient, Constants.UNSUCCESS_FILING_BALANCE_TEXT, user);
                    }

                    break;
                case Amount.Ten:
                    //donate request
                    if (true)
                    {
                        result = await user.AddPointsAsync(10);
                        if (result is OkResult)
                        {
                            MessageSender.SendMessage(botClient, Constants.SUCCESS_FILLING_BALANCE_TEXT, user);
                        }
                        else
                        {
                            MessageSender.SendMessage(botClient, Constants.UNSUCCESS_FILING_BALANCE_TEXT, user);
                        }
                        
                    }
                    else
                    {
                        MessageSender.SendMessage(botClient, Constants.UNSUCCESS_FILING_BALANCE_TEXT, user);
                    }
                    break;
                case Amount.Twenty:
                    //donate request
                    if (true)
                    {
                        result = await user.AddPointsAsync(20);
                        if (result is OkResult)
                        {
                            MessageSender.SendMessage(botClient, Constants.SUCCESS_FILLING_BALANCE_TEXT, user);
                        }
                        else
                        {
                            MessageSender.SendMessage(botClient, Constants.UNSUCCESS_FILING_BALANCE_TEXT, user);
                        }
                        
                    }
                    else
                    {
                        MessageSender.SendMessage(botClient, Constants.UNSUCCESS_FILING_BALANCE_TEXT, user);
                    }
                    break;
            }
            Handlers.users[user.Id].keyboardNavigator.PopToMenu(botClient, user);
        }

        public static async Task RemovePunishment(Punishments punishments, ITelegramBotClient botClient, Telegram.Bot.Types.User user)
        {
            ActionResult result;
            switch (punishments)
            {
                case Punishments.Ban:
                    result = await user.RemovePointsAsync(Constants.BAN_REMOVE_PRICE);
                    if (result is OkResult)
                    {
                        //unban
                        MessageSender.SendMessage(botClient, Constants.SUCCESS_REMOVED_PUNISHMENT_TEXT, user);
                    }
                    else if (result is BadRequestResult)
                    {
                        MessageSender.SendMessage(botClient, Constants.UNSUCCESS_REMOVING_PUNISHMENT, user);
                    }
                    else
                    {
                        MessageSender.SendMessage(botClient, Constants.NOT_ENOUGH_CREDITS_TEXT, user);
                    }

                    break;
                case Punishments.Mute:
                    result = await user.RemovePointsAsync(Constants.MUTE_REMOVE_PRICE);
                    if (result is OkResult)
                    {
                        //unban
                        MessageSender.SendMessage(botClient, Constants.SUCCESS_REMOVED_PUNISHMENT_TEXT, user);
                    }
                    else if (result is BadRequestResult)
                    {
                        MessageSender.SendMessage(botClient, Constants.UNSUCCESS_REMOVING_PUNISHMENT, user);
                    }
                    else
                    {
                        MessageSender.SendMessage(botClient, Constants.NOT_ENOUGH_CREDITS_TEXT, user);
                    }

                    break;
                case Punishments.Warn:
                    result = await user.RemovePointsAsync(Constants.WARN_REMOVE_PRICE);
                    if (result is OkResult)
                    {
                        //unban
                        MessageSender.SendMessage(botClient, Constants.SUCCESS_REMOVED_PUNISHMENT_TEXT, user);
                    }
                    else if (result is BadRequestResult)
                    {
                        MessageSender.SendMessage(botClient, Constants.UNSUCCESS_REMOVING_PUNISHMENT, user);
                    }
                    else
                    {
                        MessageSender.SendMessage(botClient, Constants.NOT_ENOUGH_CREDITS_TEXT, user);
                    }

                    break;
            }
            Handlers.users[user.Id].keyboardNavigator.PopToMenu(botClient, user);
        }

        public static async Task GetFriendsList(ITelegramBotClient botClient, Telegram.Bot.Types.User user)
        {
            var friendsList = user.GetFriendsList();
            string frList = "";
            foreach (var fr in friendsList)
            {
                frList += fr.UserName + '\n';
            }
            MessageSender.SendMessage(botClient,
                Constants.FRIEND_LIST_TEXT + '\n' + frList, user);
            Handlers.users[user.Id].keyboardNavigator.PopToMenu(botClient, user);
        }

        public static async Task GetRemoveList(ITelegramBotClient botClient, Telegram.Bot.Types.User user)
        {
            MessageSender.SendMessage(botClient, Keyboards.PrepareRemoveFriendsList(user), Constants.FRIEND_LIST_TEXT, user);
        }
        public static async Task RemoveFriend(ITelegramBotClient botClient, long whomDelete, Telegram.Bot.Types.User user)
        {
            ActionResult result;
            result = await user.RemoveFriendAsync(whomDelete);
            if (result is OkResult)
            {
                MessageSender.SendMessage(botClient, Constants.FRIEND_REMOVED_TEXT, user);
            }
            else if (result is Assist.AlreadyResult)
            {
                MessageSender.SendMessage(botClient, Constants.FRIEND_REMOVED_ALREADY, user);
            }
            else
            {
                MessageSender.SendMessage(botClient, Constants.FRIEND_REMOVED_EXCEPTION, user);
            }
        }

        public static async Task VokeToNextGame(ITelegramBotClient botClient, Telegram.Bot.Types.User user)
        {
            //write to DB to woke user for game
            // Я НЕ НАШЕЛ МЕТОДА ДЛЯ ЭТОГО
            MessageSender.SendMessage(botClient, Constants.VOKE_NEXT_GAME_TEXT, user);
            Handlers.users[user.Id].keyboardNavigator.PopToMenu(botClient, user);
        }

        public static async Task GetReason(Punishments punishments, ITelegramBotClient botClient, Telegram.Bot.Types.User user)
        {
            switch (punishments)
            {
                case Punishments.Ban:
                    var ban = user.IsUserBanned();
                    //ДОБАВЬ ПОЛЯ ВРЕМЯ БАНА, К МУТУ И ВАРНУ ОТНОСИТСЯ ТОЖЕ САМОЕ, У НИХ ТАКОЕ ЕСТЬ((((
                    if (ban)
                    {
                        string reason = "У тебя бан за "+user.GetUser().BanReason+"до %время%";
                        MessageSender.SendMessage(botClient, reason, user);
                        Handlers.users[user.Id].keyboardNavigator.PopToMenu(botClient, user);
                    }
                    else
                    {
                        MessageSender.SendMessage(botClient, Constants.NO_BANS_TEXT, user);
                        Handlers.users[user.Id].keyboardNavigator.PopToMenu(botClient, user);
                    }

                    break;
                case Punishments.Mute:
                    var mute = user.IsUserMuted();
                    if (mute)
                    {
                        string reason = "У тебя мут за "+user.GetUser().MuteReason+"до %время%";
                        MessageSender.SendMessage(botClient, reason, user);
                        Handlers.users[user.Id].keyboardNavigator.PopToMenu(botClient, user);
                    }
                    else
                    {
                        MessageSender.SendMessage(botClient, Constants.NO_MUTES_TEXT, user);
                        Handlers.users[user.Id].keyboardNavigator.PopToMenu(botClient, user);
                    }

                    break;
                case Punishments.Warn:
                    var warn = user.GetUserWarnCount();
                    if (warn>0)
                    {
                        string reason = "У тебя есть "+user.GetUserWarnCount()+" варнов: \n"+String.Join("\n",user.GetUserWarnsReasons());
                        MessageSender.SendMessage(botClient, reason, user);
                        Handlers.users[user.Id].keyboardNavigator.PopToMenu(botClient, user);
                    }
                    else
                    {
                        MessageSender.SendMessage(botClient, Constants.NO_WARNS_TEXT, user);
                        Handlers.users[user.Id].keyboardNavigator.PopToMenu(botClient, user);
                    }

                    break;
            }
        }

        public static async Task GetRules(ITelegramBotClient botClient, Telegram.Bot.Types.User user)
        {
            MessageSender.SendMessage(botClient, Configuration.RulesLink, user);
        }

        public static async Task GetRolesDescription(ITelegramBotClient botClient, Telegram.Bot.Types.User user)
        {
            MessageSender.SendMessage(botClient, Configuration.RolesDescLink, user);
        }
    }
}