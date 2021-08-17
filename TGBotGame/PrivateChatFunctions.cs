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
        

        public static async Task FillBalance(uint amount, ITelegramBotClient botClient, Telegram.Bot.Types.User user)
        {
            Handlers.users[user.Id].payment = await Assist.AddPayment(amount, user.Id);
            MessageSender.SendMessage(botClient, "Заявка на оплату создана, переведите "+amount+" рублей на киви кошелек "+Configuration.QiwiMobile+
                                                 "(номер телефона). Обязательно напишите этот код в комментарий - "+Handlers.users[user.Id].payment.RId+
                                                 ". Когда вы оплатите и платеж пройдет, я уведомлю вас.", user);
        }

    /*public static async Task PrepareForPayment(uint amount, ITelegramBotClient botClient, Telegram.Bot.Types.User user)
    {
        Handlers.users[user.Id].donateAmount = amount;
        MessageSender.SendMessage(botClient, Constants.REQUEST_PHONE, user);
        Handlers.users[user.Id].curState = KeyboardsNavigator.CurentState.FillBalance;
    }*/

    public static async Task RemovePunishment(Punishments punishments, ITelegramBotClient botClient, Telegram.Bot.Types.
    User user)
    {
    ActionResult result;
        switch (punishments)
    {
        case Punishments.Ban:
        if (!user.GetUser().IsBanned)
        {
            MessageSender.SendMessage(botClient, Constants.NO_BANS_TEXT, user);
            return;
        }

        result = await user.RemovePointsAsync(Constants.BAN_REMOVE_PRICE);
        if (result is OkResult)
        {
            user.UnBan();
            MessageSender.SendMessage(botClient, Keyboards.PrepareUnpunishmentAdminsKeyboard(user),
                Constants.ADMINS_REQUEST_REMOVE_PUNISHMENT + "бана.\n" + Constants.USERNAME + user.Username + '\n' +
                Constants.USERID + user.Id, Configuration.AdminsGroupId);
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
        if (!user.GetUser().IsMuted)
        {
            MessageSender.SendMessage(botClient, Constants.NO_MUTES_TEXT, user);
            return;
        }

        result = await user.RemovePointsAsync(Constants.MUTE_REMOVE_PRICE);
        if (result is OkResult)
        {
            user.UnMute();
            MessageSender.SendMessage(botClient, Keyboards.PrepareUnpunishmentAdminsKeyboard(user),
                Constants.ADMINS_REQUEST_REMOVE_PUNISHMENT + "мута.\n" + Constants.USERNAME + user.Username + '\n' +
                Constants.USERID + user.Id, Configuration.AdminsGroupId);
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
        if (user.GetUserWarnCount() == 0)
        {
            MessageSender.SendMessage(botClient, Constants.NO_WARNS_TEXT, user);
            return;
        }

        result = await user.RemovePointsAsync(Constants.WARN_REMOVE_PRICE);
        if (result is OkResult)
        {
            await Assist.RemoveWarn(user.Id);
            MessageSender.SendMessage(botClient, Keyboards.PrepareUnpunishmentAdminsKeyboard(user),
                Constants.ADMINS_REQUEST_REMOVE_PUNISHMENT + "варна.\n" + Constants.USERNAME + user.Username + '\n' +
                Constants.USERID + user.Id, Configuration.AdminsGroupId);
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
    await user.AddNextGame();
    MessageSender.SendMessage(botClient, Constants.VOKE_NEXT_GAME_TEXT, user);
    Handlers.users[user.Id].keyboardNavigator.PopToMenu(botClient, user);
}

public static async Task GetReason(Punishments punishments, ITelegramBotClient botClient, Telegram.Bot.Types.User user)
{
    switch (punishments)
    {
        case Punishments.Ban:
            var ban = user.IsUserBanned();
            if (ban)
            {
                string reason = "У тебя бан за " + user.GetUser().BanReason + " до " + user.GetUnBanDate();
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
                string reason = "У тебя мут за " + user.GetUser().MuteReason + " до " + user.GetUnMuteDate();
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
            if (warn > 0)
            {
                string reason = "У тебя есть " + user.GetUserWarnCount() + " варн(а): \n" +
                                String.Join("\n", user.GetUserWarnsReasons());
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
    MessageSender.SendMessage(botClient, Keyboards.PrepareLinkKeyboardMarkup(Configuration.RulesLink),"Нажмите на кнопку, чтобы перейти на канал с правилами", user);
}

public static async Task GetRolesDescription(ITelegramBotClient botClient, Telegram.Bot.Types.User user)
{
    MessageSender.SendMessage(botClient,Keyboards.PrepareLinkKeyboardMarkup(Configuration.RolesDescLink), "Нажмите на кнопку, чтобы перейти на канал с описанием ролей", user);
}
}
}