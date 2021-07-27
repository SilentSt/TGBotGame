using System;

namespace TGBotGame
{
    public class User
    {
        public string chatId;
        public KeyboardsNavigator.CurentState curState = KeyboardsNavigator.CurentState.Menu;

        public User(long chatId)
        {
            this.chatId = this.chatId;
        }
    }
}