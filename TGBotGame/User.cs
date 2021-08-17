using System;
using BotDataSet;

namespace TGBotGame
{
    public class User
    {
        public Payment payment; 
        public long? chatId = 0;
        public uint donateAmount;
        public Telegram.Bot.Types.User _user;
        public KeyboardsNavigator keyboardNavigator;
        public KeyboardsNavigator.CurentState curState = KeyboardsNavigator.CurentState.Menu;

        public User(long? chatId, Telegram.Bot.Types.User user)
        {
            this.chatId = chatId;
            keyboardNavigator = new KeyboardsNavigator();
            _user = user;
            curState = KeyboardsNavigator.CurentState.Menu;
        }
    }
}