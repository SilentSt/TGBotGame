using System;

namespace TGBotGame
{
    public class User
    {
        public long? chatId;
        public KeyboardsNavigator keyboardNavigator;
        public KeyboardsNavigator.CurentState curState = KeyboardsNavigator.CurentState.Menu;

        public User(long? chatId)
        {
            this.chatId = chatId;
            keyboardNavigator = new KeyboardsNavigator();
        }
    }
}