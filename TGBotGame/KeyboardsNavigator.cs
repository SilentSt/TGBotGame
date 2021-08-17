using BotDataSet;
using Telegram.Bot;
using Telegram.Bot.Types;

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
            RemoveFriends,
            InputPhone
        }
        public void PushToReasonPunishment(ITelegramBotClient botClient, Telegram.Bot.Types.User user, CallbackQuery callbackQuery)
        {
            botClient.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
            MessageSender.SendMessage(botClient, Keyboards.PrepareInlineKeyboard(KeyboardsTexts.resPun),
                Constants.REASON_PUNISHMENT_TEXT, user);
            Handlers.users[user.Id].curState = CurentState.ReasonPunishment;
        }

        public void PushToReason(ITelegramBotClient botClient, Telegram.Bot.Types.User user, CallbackQuery callbackQuery)
        {
            botClient.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
            MessageSender.SendMessage(botClient, Keyboards.PrepareInlineKeyboard(KeyboardsTexts.reasons), Constants.REASON_TEXT, user);
            Handlers.users[user.Id].curState = CurentState.Reason;
        }

        public void PushToPunishment(ITelegramBotClient botClient, Telegram.Bot.Types.User user, CallbackQuery callbackQuery)
        {
            botClient.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
            var balance = user.GetPoints();
            MessageSender.SendMessage(botClient, Keyboards.PrepareInlineKeyboard(KeyboardsTexts.removePun), Constants.PUNISHMENT_TEXT + balance, user);
            Handlers.users[user.Id].curState = CurentState.Punishment;
        }

        public void PushToFillBalance(ITelegramBotClient botClient, Telegram.Bot.Types.User user, CallbackQuery callbackQuery)
        {
            botClient.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
            MessageSender.SendMessage(botClient,Keyboards.PrepareInlineKeyboard(KeyboardsTexts.sums), Constants.FILL_CREDITS_TEXT, user);
            Handlers.users[user.Id].curState = CurentState.FillBalance;
        }

        public void PushToFriends(ITelegramBotClient botClient, Telegram.Bot.Types.User user, CallbackQuery callbackQuery)
        {
            botClient.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
            MessageSender.SendMessage(botClient, Keyboards.PrepareInlineKeyboard(KeyboardsTexts.friends), Constants.FRIENDS_TEXT, user);
            Handlers.users[user.Id].curState = CurentState.Friends;
        }

        public void PopToMenu(ITelegramBotClient botClient, Telegram.Bot.Types.User user, CallbackQuery callbackQuery)
        {
            botClient.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
            MessageSender.SendMessage(botClient, Keyboards.PrepareInlineKeyboard(KeyboardsTexts.menu), Constants.MENU_TEXT, user);
            Handlers.users[user.Id].curState = CurentState.Menu;
        }

    }
}