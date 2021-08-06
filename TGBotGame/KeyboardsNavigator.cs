using BotDataSet;
using Telegram.Bot;

namespace TGBotGame
{
    public class KeyboardsNavigator
    {
        public enum CurentState
        {
            Menu,
            ReasonPunishment,
            Reason,
            Punishment,
            FillBalance,
            Friends,
            InputPhone
        }
        public void PushToReasonPunishment(ITelegramBotClient botClient, Telegram.Bot.Types.User user)
        {
            MessageSender.SendMessage(botClient, Keyboards.PrepareReasonPunishmentKeyboard(),
                Constants.REASON_PUNISHMENT_TEXT, user);
            Handlers.users[user.Id].curState = CurentState.ReasonPunishment;
        }

        public void PushToReason(ITelegramBotClient botClient, Telegram.Bot.Types.User user)
        {
            MessageSender.SendMessage(botClient, Keyboards.PrepareReasonKeyboard(), Constants.REASON_TEXT, user);
            Handlers.users[user.Id].curState = CurentState.Reason;
        }

        public void PushToPunishment(ITelegramBotClient botClient, Telegram.Bot.Types.User user)
        {
            var balance = user.GetPoints();
            MessageSender.SendMessage(botClient, Keyboards.PrepareRemovePunishmentKeyboard(), Constants.PUNISHMENT_TEXT + balance, user);
            Handlers.users[user.Id].curState = CurentState.Punishment;
        }

        public void PushToFillBalance(ITelegramBotClient botClient, Telegram.Bot.Types.User user)
        {
            MessageSender.SendMessage(botClient, Keyboards.PrepareFillBalanceKeyboard(), Constants.FILL_CREDITS_TEXT, user);
            Handlers.users[user.Id].curState = CurentState.FillBalance;
        }

        public void PushToFriends(ITelegramBotClient botClient, Telegram.Bot.Types.User user)
        {
            MessageSender.SendMessage(botClient, Keyboards.PrepareFriendsKeyboard(), Constants.FRIENDS_TEXT, user);
            Handlers.users[user.Id].curState = CurentState.Friends;
        }

        public void PopToMenu(ITelegramBotClient botClient, Telegram.Bot.Types.User user)
        {
            MessageSender.SendMessage(botClient, Keyboards.PrepareMenuKeyboard(), Constants.MENU_TEXT, user);
            Handlers.users[user.Id].curState = CurentState.Menu;
        }

        public void PushToInputPhone(ITelegramBotClient botClient, Telegram.Bot.Types.User user)
        {
            MessageSender.SendMessage(botClient, Constants.REQUEST_PHONE, user);
            Handlers.users[user.Id].curState = CurentState.InputPhone;
        }
        
    }
}