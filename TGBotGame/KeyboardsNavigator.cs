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
            Friends
        }
        public void PushToReasonPunishment(long? userId, ITelegramBotClient botClient)
        {
            MessageSender.SendMessage(botClient, Keyboards.PrepareReasonPunishmentKeyboard(),
                Constants.REASON_PUNISHMENT_TEXT, userId);
            Handlers.users[userId].curState = CurentState.ReasonPunishment;
        }

        public void PushToReason(long? userId, ITelegramBotClient botClient)
        {
            MessageSender.SendMessage(botClient, Keyboards.PrepareReasonKeyboard(), Constants.REASON_TEXT, userId);
            Handlers.users[userId].curState = CurentState.Reason;
        }

        public void PushToPunishment(long? userId, ITelegramBotClient botClient)
        {
            MessageSender.SendMessage(botClient, Keyboards.PrepareRemovePunishmentKeyboard(), Constants.PUNISHMENT_TEXT, userId);
            Handlers.users[userId].curState = CurentState.Punishment;
        }

        public void PushToFillBalance(long? userId, ITelegramBotClient botClient)
        {
            MessageSender.SendMessage(botClient, Keyboards.PrepareFillBalanceKeyboard(), Constants.FILL_CREDITS_TEXT, userId);
            Handlers.users[userId].curState = CurentState.FillBalance;
        }

        public void PushToFriends(long? userId, ITelegramBotClient botClient)
        {
            MessageSender.SendMessage(botClient, Keyboards.PrepareFriendsKeyboard(), Constants.FRIENDS_TEXT, userId);
            Handlers.users[userId].curState = CurentState.Friends;
        }

        public void PopToMenu(long? userId, ITelegramBotClient botClient)
        {
            MessageSender.SendMessage(botClient, Keyboards.PrepareMenuKeyboard(), Constants.MENU_TEXT, userId);
            Handlers.users[userId].curState = CurentState.Menu;
        }
        
    }
}