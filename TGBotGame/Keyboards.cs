using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace TGBotGame
{
    public static class Keyboards
    {
        public static ReplyKeyboardMarkup PrepareMenuKeyboard()
        {
            var rpm = new ReplyKeyboardMarkup();
            rpm.Keyboard = new KeyboardButton[][]
            {
                new KeyboardButton[]
                {
                    new KeyboardButton("🗣 Позвать меня на следующую игру")
                },
                new KeyboardButton[]
                {
                new KeyboardButton("❓ Узнать причину и снять мут/варн/бан")
                },
                new KeyboardButton[]
                {
                    new KeyboardButton("🤝 Кто у меня в друзьях?")
                },
                new KeyboardButton[]
                {
                    new KeyboardButton("📕 Правила чата и игры"),
                    new KeyboardButton("🤵🏻 Описание ролей")
                }

            };
            return rpm;
        }

        public static ReplyKeyboardMarkup PrepareFriendsKeyboard()
        {
            var rpm = new ReplyKeyboardMarkup();
            rpm.Keyboard = new KeyboardButton[][]
            {
                new KeyboardButton[]
                {
                    new KeyboardButton("Мои друзья"),
                    new KeyboardButton("Удалить из друзей")
                }
            };
            return rpm;
        }

        public static ReplyKeyboardMarkup PrepareReasonKeyboard()
        {
            var rpm = new ReplyKeyboardMarkup();
            rpm.Keyboard = new KeyboardButton[][]
            {
                new KeyboardButton[]
                {
                    new KeyboardButton("Причина варна"),
                    new KeyboardButton("Причина мута"),
                    new KeyboardButton("Причина бана")
                }
            };
            return rpm;
        }
        
        public static ReplyKeyboardMarkup PrepareReasonPunishmentKeyboard()
        {
            var rpm = new ReplyKeyboardMarkup();
            rpm.Keyboard = new KeyboardButton[][]
            {
                new KeyboardButton[]
                {
                    new KeyboardButton("Узнать причину"),
                    new KeyboardButton("Снять наказание")
                }
            };
            return rpm;
        }
        
        public static ReplyKeyboardMarkup PrepareRemovePunishmentKeyboard()
        {
            var rpm = new ReplyKeyboardMarkup();
            rpm.Keyboard = new KeyboardButton[][]
            {
                new KeyboardButton[]
                {
                    new KeyboardButton("Снять варн"),
                    new KeyboardButton("Снять мут"),
                    new KeyboardButton("Снять бан")
                },
                new KeyboardButton[]
                {
                    new KeyboardButton("Пополнить баланс")
                }
            };
            return rpm;
        }
        
        public static ReplyKeyboardMarkup PrepareFillBalanceKeyboard()
        {
            var rpm = new ReplyKeyboardMarkup();
            rpm.Keyboard = new KeyboardButton[][]
            {
                new KeyboardButton[]
                {
                    new KeyboardButton("5 кредитов"),
                    new KeyboardButton("10 кредитов"),
                    new KeyboardButton("20 кредитов")
                }
            };
            return rpm;
        }
    }
}