using System.Threading.Tasks;
using BotDataSet;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TGBotGame
{
    public static class GroupFunctions
    {
        public static async Task DeleteFriend(ITelegramBotClient botClient, Message message)
        {
            var whom = message.Text.Split(' ')[1];
            var result = await message.From.RemoveFriendAsync(whom);
            if (result is OkResult)
            {
                MessageSender.SendMessage(botClient, Constants.FRIEND_REMOVED_TEXT, message.From);
            }
            else if (result is Assist.AlreadyResult)
            {
                MessageSender.SendMessage(botClient, Constants.FRIEND_REMOVED_ALREADY, message.From);
            }
            else
            {
                MessageSender.SendMessage(botClient, Constants.FRIEND_REMOVED_EXCEPTION, message.From);
            }
        }

        public static async Task VokeFriendsPlay(ITelegramBotClient botClient, Message message)
        {
            foreach (var botUser in message.From.GetFriendsList())
            {
                MessageSender.SendMessage(botClient, Constants.VOKE_FRIENDS_PLAY_TEXT + message.From.Username, botUser);
            }
        }

        public static async Task AddFriend(ITelegramBotClient botClient, Message message)
        {
            var whom = message.Text.Split(' ')[1];
            if (string.IsNullOrEmpty(whom)||string.IsNullOrWhiteSpace(whom))
            {
                return;
            }
            var result = await message.From.AddFriend(whom);
            if (result is OkResult || result is Assist.AlreadyResult)
            {
                MessageSender.SendMessage(botClient, Constants.SUCCESS_ADD_FRIEND_TEXT, message.From);
            }
        }

        public static async Task SendGiftToFriend(ITelegramBotClient botClient, Message message)
        {
            var whom = message.Text.Split(' ')[1];
            var count = message.Text.Split(' ')[2];
            uint val;
            if (uint.TryParse(count, out val))
            {
                var result = await message.From.RemovePointsAsync(val);
                if (result is OkResult)
                {
                    
                }
                if (result is OkResult || result is Assist.AlreadyResult)
                {
                    MessageSender.SendMessage(botClient, Constants.SUCCESS_ADD_FRIEND_TEXT, message.From);
                }
            }
        }

        public static async Task CreateRequestNextGame(ITelegramBotClient botClient, Message message)
        {
            var result = await Assist.AddNextGame(message.From);
            if (result is OkResult)
            {
                MessageSender.SendMessage(botClient, Constants.VOKE_NEXT_GAME_TEXT, message.From);
            }
        }

        public static async Task SendInvitesToUsers(ITelegramBotClient botClient, Message message)
        {
            var us = await botClient.GetChatMemberAsync(message.Chat.Id, message.From.Id);
            if (us.Status != ChatMemberStatus.Administrator|| us.Status != ChatMemberStatus.Creator)
            {
                return;
            }
            var users = Assist.GetNextGameUsers();
            foreach (var user in users)
            {
                MessageSender.SendMessage(botClient, Constants.VOKE_PLAYER_PLAY, user);
            }

            await users.RemoveNextGame();
        }
    }
}