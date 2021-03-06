using BotDataSet;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace TGBotGame
{
    public static class MessageSender
    {
        public static void SendMessage(ITelegramBotClient botClient, string message, Telegram.Bot.Types.User user)
        {
            botClient.SendTextMessageAsync(user.Id,
                message
            );

        }

        public static void SendMessage(ITelegramBotClient botClient, ReplyKeyboardMarkup keyboard, string message, Telegram.Bot.Types.User user)
        {
            botClient.SendTextMessageAsync(user.Id,
                message,
                replyMarkup:keyboard
            );
        }
        
        public static void SendMessage(ITelegramBotClient botClient, InlineKeyboardMarkup keyboard, string message, Telegram.Bot.Types.User user)
        {
            botClient.SendTextMessageAsync(user.Id,
                message,
                replyMarkup:keyboard
            );
        }

        public static void SendMessage(ITelegramBotClient botClient, string message, long chatId)
        {
            botClient.SendTextMessageAsync(chatId,
                message
            );
        }

        public static void SendMessage(ITelegramBotClient botClient,
            InlineKeyboardMarkup prepareUnpunishmentAdminsKeyboard, string message, BotUser botUser)
        {
            botClient.SendTextMessageAsync(botUser.UserId,
                message,
                replyMarkup:prepareUnpunishmentAdminsKeyboard
            );
        }

        public static void SendMessage(ITelegramBotClient botClient, InlineKeyboardMarkup prepareUnpunishmentAdminsKeyboard, string adminsRequestRemovePunishment, long adminsGroupId)
        {
            botClient.SendTextMessageAsync(
                adminsGroupId,
                adminsRequestRemovePunishment,
                replyMarkup: prepareUnpunishmentAdminsKeyboard
            );
        }
    }
}