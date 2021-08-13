using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BotDataSet;
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
                },
                new KeyboardButton[]
                {
                    new KeyboardButton("Меню")
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
                },
                new KeyboardButton[]
                {
                    new KeyboardButton("Меню")
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
                },
                new KeyboardButton[]
                {
                    new KeyboardButton("Меню")
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
                },
                new KeyboardButton[]
                {
                    new KeyboardButton("Меню")
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
                },
                new KeyboardButton[]
                {
                    new KeyboardButton("Меню")
                }
            };
            return rpm;
        }

        public static InlineKeyboardMarkup PrepareRemoveFriendsList(Telegram.Bot.Types.User user)
        {
            var friendsList = Assist.GetFriendsList(user);
            List<InlineKeyboardButton> users = new List<InlineKeyboardButton>();
            foreach (var friend in friendsList)
            {
                var btn = new InlineKeyboardButton();
                btn.Text = friend.UserName;
                btn.CallbackData = friend.UserId.ToString();
                users.Add(btn);
            }

            return new InlineKeyboardMarkup(users);

        }

        public static InlineKeyboardMarkup PrepareUnpunishmentAdminsKeyboard(Telegram.Bot.Types.User user)
        {
            var btn = new InlineKeyboardButton();
            btn.Text = "Готово!(сообщение удалится)";
            btn.CallbackData = "remove_message";
            return new InlineKeyboardMarkup(btn);
        }

        public static InlineKeyboardMarkup PrepareLinkKeyboardMarkup(string link)
        {
            var btn = new InlineKeyboardButton();
            btn.Text = "Канал с информацией";
            btn.Url = link;
            return new InlineKeyboardMarkup(btn);
        }
    }
}