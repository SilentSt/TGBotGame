using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BotDataSet;
using Telegram.Bot.Types.ReplyMarkups;

namespace TGBotGame
{
    public static class Keyboards
    {
        public static InlineKeyboardMarkup PrepareInlineKeyboard(List<string> texts)
        {
            var rpm = new List<IEnumerable<InlineKeyboardButton>>();
            if (!texts.Contains("🗣 Позвать меня на следующую игру"))
            {
                foreach (var text in texts)
                {
                    var btn = new InlineKeyboardButton();
                    btn.Text = text;
                    btn.CallbackData = text;
                    rpm.Add(new List<InlineKeyboardButton>(){btn});
                }
            }
            else
            {
                for (int i = 0; i < texts.Count; i++)
                {
                    var btn = new InlineKeyboardButton();
                    btn.Text = texts[i];
                    btn.CallbackData = KeyboardsTexts.menuData[i];
                    rpm.Add(new List<InlineKeyboardButton>(){btn});
                }
            }

            return new InlineKeyboardMarkup(rpm);
        }

        public static InlineKeyboardMarkup PrepareRemoveFriendsList(Telegram.Bot.Types.User user)
        {
            var friendsList = Assist.GetFriendsList(user);
            if (friendsList.Count()<1)
            {
                return null;
            }
            var users = new List<IEnumerable<InlineKeyboardButton>>();
            var bt = new InlineKeyboardButton();
            bt.Text = "Меню";
            bt.CallbackData = "Меню";
            users.Add(new List<InlineKeyboardButton>(){bt});
            foreach (var friend in friendsList)
            {
                var btn = new InlineKeyboardButton();
                btn.Text = friend.UserName;
                btn.CallbackData = friend.UserId.ToString();
                users.Add(new List<InlineKeyboardButton>(){btn});
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
        
        public static InlineKeyboardMarkup PreparePopMenuKeyboard(Telegram.Bot.Types.User user)
        {
            var btn = new InlineKeyboardButton();
            btn.Text = "Меню";
            btn.CallbackData = "Меню";
            return new InlineKeyboardMarkup(btn);
        }

        public static InlineKeyboardMarkup PrepareLinkKeyboardMarkup(string link)
        {
            var btn = new InlineKeyboardButton();
            btn.Text = "Канал с информацией";
            btn.Url = link;
            var btn1 = new InlineKeyboardButton();
            btn1.Text = "Меню";
            btn1.CallbackData = "Меню";
            return new InlineKeyboardMarkup(new List<IEnumerable<InlineKeyboardButton>>()
                {new List<InlineKeyboardButton>(){btn}, new List<InlineKeyboardButton>(){btn1}});
        }

        public static InlineKeyboardMarkup PreparePaymentKeyboardMarkup(string link)
        {
            var btn = new InlineKeyboardButton();
            btn.Text = "Оплатить";
            btn.Url = link;
            var btn1 = new InlineKeyboardButton();
            btn1.Text = "Меню";
            btn1.CallbackData = "Меню";
            return new InlineKeyboardMarkup(new List<IEnumerable<InlineKeyboardButton>>()
                {new List<InlineKeyboardButton>(){btn}, new List<InlineKeyboardButton>(){btn1}});
        }
    }
}