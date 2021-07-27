using System.Threading.Tasks;
using Telegram.Bot;

namespace TGBotGame
{
    public static class PrivateChatFunctions
    {
        public enum Punishments
        {
            Warn,
            Mute,
            Ban
        }

        public enum Amount
        {
            Five,
            Ten,
            Twenty
        }
        public static async Task FillBalance(long? userId, Amount amount, ITelegramBotClient botClient)
        {
            switch (amount)
            {
                case Amount.Five:

                    break;
                case Amount.Ten:

                    break;
                case Amount.Twenty:

                    break;
            }
        }

        public static async Task RemovePunishment(long? userId, Punishments punishments, ITelegramBotClient botClient)
        {
            switch (punishments)
            {
                case Punishments.Ban:

                    break;
                case Punishments.Mute:

                    break;
                case Punishments.Warn:

                    break;
            }
            
        }

        public static async Task GetFriendsList(long? userId, ITelegramBotClient botClient)
        {
            
        }

        public static async Task RemoveFriend(long? userId, ITelegramBotClient botClient)
        {
            
        }

        public static async Task VokeToNextGame(long? userId, ITelegramBotClient botClient)
        {
            
        }

        public static async Task GetReason(long? userId, Punishments punishments, ITelegramBotClient botClient)
        {
            switch (punishments)
            {
                case Punishments.Ban:

                    break;
                case Punishments.Mute:

                    break;
                case Punishments.Warn:

                    break;
            }
        }

        public static async Task GetRules(long? userId, ITelegramBotClient botClient)
        {
            
        }

        public static async Task GetRolesDescription(long? userId, ITelegramBotClient botClient)
        {
            
        }
        
    }
}