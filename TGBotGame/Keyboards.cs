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
    }
}