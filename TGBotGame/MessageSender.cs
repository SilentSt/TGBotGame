using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace TGBotGame
{
    public static class MessageSender
    {
        public static void SendMessage(ITelegramBotClient botClient, string message, long? chatId)
        {
            botClient.SendTextMessageAsync(chatId,
                message
            );

        }

        public static void SendMessage(ITelegramBotClient botClient, ReplyKeyboardMarkup keyboard, string message, long? chatId)
        {
            botClient.SendTextMessageAsync(chatId,
                message,
                replyMarkup:keyboard
            );
        }
        
        public static void SendMessage(ITelegramBotClient botClient, InlineKeyboardMarkup keyboard, string message, long? chatId)
        {
            botClient.SendTextMessageAsync(chatId,
                message,
                replyMarkup:keyboard
            );
        }
    }
}