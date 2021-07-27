using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

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
                    //donate request
                    if (true)
                    {
                        MessageSender.SendMessage(botClient, Constants.SUCCESS_FILLING_BALANCE_TEXT, userId);
                        //write into DB
                    } //all ok
                    else
                    {
                        MessageSender.SendMessage(botClient, Constants.UNSUCCESS_FILING_BALANCE_TEXT, userId);
                    }

                    break;
                case Amount.Ten:
                    //donate request
                    if (true)
                    {
                        MessageSender.SendMessage(botClient, Constants.SUCCESS_FILLING_BALANCE_TEXT, userId);
                        //write into DB
                    } //all ok
                    else
                    {
                        MessageSender.SendMessage(botClient, Constants.UNSUCCESS_FILING_BALANCE_TEXT, userId);
                    }

                    break;
                case Amount.Twenty:
                    //donate request
                    if (true)
                    {
                        MessageSender.SendMessage(botClient, Constants.SUCCESS_FILLING_BALANCE_TEXT, userId);
                        //write into DB
                    }//all ok
                    else
                    {
                        MessageSender.SendMessage(botClient, Constants.UNSUCCESS_FILING_BALANCE_TEXT, userId);
                    }
                    break;
            }
        }

        public static async Task RemovePunishment(long? userId, Punishments punishments, ITelegramBotClient botClient)
        {
            switch (punishments)
            {
                case Punishments.Ban:
                    //get info about credits
                    if (true)
                    {
                        //unban
                        MessageSender.SendMessage(botClient, Constants.SUCCESS_REMOVED_PUNISHMENT_TEXT, userId);
                    } //if enough 
                    else
                    {
                        MessageSender.SendMessage(botClient, Constants.NOT_ENOUGH_CREDITS_TEXT, userId);
                    }

                    break;
                case Punishments.Mute:
                    //get info about credits
                    if (true)
                    {
                        //unban
                        MessageSender.SendMessage(botClient, Constants.SUCCESS_REMOVED_PUNISHMENT_TEXT, userId);
                    } //if enough 
                    else
                    {
                        MessageSender.SendMessage(botClient, Constants.NOT_ENOUGH_CREDITS_TEXT, userId);
                    }

                    break;
                case Punishments.Warn:
                    //get info about credits
                    if (true)
                    {
                        //unban
                        MessageSender.SendMessage(botClient, Constants.SUCCESS_REMOVED_PUNISHMENT_TEXT, userId);
                    } //if enough 
                    else
                    {
                        MessageSender.SendMessage(botClient, Constants.NOT_ENOUGH_CREDITS_TEXT, userId);
                    }

                    break;
            }
        }

        public static async Task GetFriendsList(long? userId, ITelegramBotClient botClient)
        {
            //download from DB friends list
            var friendsList = "";
            MessageSender.SendMessage(botClient,
                Constants.FRIEND_LIST_TEXT + '\n' + friendsList, userId);
        }

        public static async Task GetRemoveList(long? userId, ITelegramBotClient botClient)
        {
            //prepare inline keyboard with users friends
            
            //REMOVE AFTER FINISH
            var kbB = new InlineKeyboardButton();
            kbB.Text = "Somebody";
            kbB.CallbackData = "12345";
            //END
            MessageSender.SendMessage(botClient, new InlineKeyboardMarkup(kbB), Constants.FRIEND_LIST_TEXT, userId);
        }
        public static async Task RemoveFriend(long? userId, ITelegramBotClient botClient, long? whomDelete)
        {
            //remove friend from DB
            MessageSender.SendMessage(botClient, Constants.FRIEND_REMOVED_TEXT, userId);
        }

        public static async Task VokeToNextGame(long? userId, ITelegramBotClient botClient)
        {
            //write to DB to woke user for game
            MessageSender.SendMessage(botClient, Constants.VOKE_NEXT_GAME_TEXT, userId);
        }

        public static async Task GetReason(long? userId, Punishments punishments, ITelegramBotClient botClient)
        {
            switch (punishments)
            {
                case Punishments.Ban:
                    var ban = false;
                    //get ban reason from DB
                    if (ban) //if user has ban
                    {
                        
                    }
                    else
                    {
                        MessageSender.SendMessage(botClient, Constants.NO_BANS_TEXT, userId);
                        Handlers.users[userId].keyboardNavigator.PopToMenu(userId, botClient);
                    }

                    break;
                case Punishments.Mute:
                    var mute = false;
                    //get mute reason from DB
                    if (mute) //if user has mute
                    {
                    }
                    else
                    {
                        MessageSender.SendMessage(botClient, Constants.NO_MUTES_TEXT, userId);
                        Handlers.users[userId].keyboardNavigator.PopToMenu(userId, botClient);
                    }

                    break;
                case Punishments.Warn:
                    var warn = false;
                    //get warn reason from DB
                    if (warn) //if user has warn
                    {
                    }
                    else
                    {
                        MessageSender.SendMessage(botClient, Constants.NO_WARNS_TEXT, userId);
                        Handlers.users[userId].keyboardNavigator.PopToMenu(userId, botClient);
                    }

                    break;
            }
        }

        public static async Task GetRules(long? userId, ITelegramBotClient botClient)
        {
            MessageSender.SendMessage(botClient, Configuration.RulesLink, userId);
        }

        public static async Task GetRolesDescription(long? userId, ITelegramBotClient botClient)
        {
            MessageSender.SendMessage(botClient, Configuration.RolesDescLink, userId);
        }
    }
}