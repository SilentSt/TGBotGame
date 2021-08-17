using System.Collections.Generic;

namespace TGBotGame
{
    public static class KeyboardsTexts
    {
        public static List<string> menu = new List<string>()
                {
                    "🗣 Позвать меня на следующую игру",
                    "❓ Узнать причину и снять мут/варн/бан",
                    "🤝 Кто у меня в друзьях?",
                    "📕 Правила чата и игры",
                    "🤵🏻 Описание ролей"
                };
        public static List<string> menuData = new List<string>()
        {
            "next",
            "reason",
            "friends",
            "rules",
            "desc"
        };
        public static List<string> friends = new List<string>()
            {
                "Мои друзья",
                "Удалить из друзей",
                "Меню"
            };
        public static List<string> reasons = new List<string>()
        {
            "Причина варна",
                "Причина мута",
            "Причина бана"
        };
        public static List<string> resPun = new List<string>()
        {
            "Узнать причину",
            "Снять наказание",
            "Меню"
        };
        public static List<string> removePun = new List<string>()
        {
            "Снять варн",
            "Снять мут",
            "Снять бан",
            "Пополнить баланс",
            "Меню"
        };
        public static List<string> sums = new List<string>()
        {
            "5 кредитов",
            "10 кредитов",
            "20 кредитов",
            "Меню"
        };
            
    }
}