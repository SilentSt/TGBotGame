using System.Globalization;
using System.Linq;
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
            string whom;
            if (message.ReplyToMessage!=null)
            {
                whom = message.ReplyToMessage.From.Username;
            }
            else
            {
                whom = message.Text.Split(' ')[1];
            }
            var result = await message.From.RemoveFriendAsync(whom);
            if (result is OkResult)
            {
                MessageSender.SendMessage(botClient,Keyboards.PrepareUnpunishmentAdminsKeyboard(message.From), Constants.FRIEND_REMOVED_TEXT, message.From);
            }
            else if (result is Assist.AlreadyResult)
            {
                MessageSender.SendMessage(botClient,Keyboards.PrepareUnpunishmentAdminsKeyboard(message.From), Constants.FRIEND_REMOVED_ALREADY, message.From);
            }
            else
            {
                MessageSender.SendMessage(botClient,Keyboards.PrepareUnpunishmentAdminsKeyboard(message.From), Constants.FRIEND_REMOVED_EXCEPTION, message.From);
            }
        }

        public static async Task VokeFriendsPlay(ITelegramBotClient botClient, Message message)
        {
            foreach (var botUser in message.From.GetFriendsList())
            {
                MessageSender.SendMessage(botClient,Keyboards.PrepareUnpunishmentAdminsKeyboard(message.From), Constants.VOKE_FRIENDS_PLAY_TEXT + message.From.Username, botUser);
            }
        }

        public static async Task AddFriend(ITelegramBotClient botClient, Message message)
        {
            string whom;
            if (message.ReplyToMessage!=null)
            {
                whom = message.ReplyToMessage.From.Username;
            }
            else
            {
                whom = message.Text.Split(' ')[1];
            }
            if (string.IsNullOrEmpty(whom) || string.IsNullOrWhiteSpace(whom))
            {
                return;
            }

            var result = await message.From.AddFriend(whom);
            if (result is OkResult || result is Assist.AlreadyResult)
            {
                MessageSender.SendMessage(botClient,Keyboards.PrepareUnpunishmentAdminsKeyboard(message.From), Constants.SUCCESS_ADD_FRIEND_TEXT, message.From);
            }
            else
            {
                MessageSender.SendMessage(botClient,Keyboards.PrepareUnpunishmentAdminsKeyboard(message.From), Constants.USER_NOT_FOUND, message.From);
            }
        }

        public static async Task SendGiftToFriend(ITelegramBotClient botClient, Message message)
        {
            string whom, count = null;
            if (message.ReplyToMessage!=null)
            {
                whom = message.ReplyToMessage.From.Username;
                count = message.Text.Split(' ')[1];
            }
            else
            {
                whom = message.Text.Split(' ')[1];
                count = message.Text.Split(' ')[2];
            }
            
            uint val;
            if (uint.TryParse(count, out val))
            {
                var result = await message.From.GiftPointsAsync(whom, val);
                if (result is OkResult)
                {
                    MessageSender.SendMessage(botClient,Keyboards.PrepareUnpunishmentAdminsKeyboard(message.From), Constants.GIFT_SENT_TEXT, message.From);
                }
                else if (result is Assist.NotEnoughResult)
                {
                    MessageSender.SendMessage(botClient,Keyboards.PrepareUnpunishmentAdminsKeyboard(message.From), Constants.NOT_ENOUGH_CREDITS_TEXT, message.From);
                }
                else if (result is NotFoundResult)
                {
                    MessageSender.SendMessage(botClient,Keyboards.PrepareUnpunishmentAdminsKeyboard(message.From), Constants.USER_NOT_FOUND, message.From);
                }
            }
            else
            {
                MessageSender.SendMessage(botClient, Keyboards.PrepareUnpunishmentAdminsKeyboard(message.From),Constants.ERROR_IN_SUM, message.From);
            }
        }

        public static async Task CreateRequestNextGame(ITelegramBotClient botClient, Message message)
        {
            var result = await Assist.AddNextGame(message.From);
            if (result is OkResult)
            {
                MessageSender.SendMessage(botClient,Keyboards.PrepareUnpunishmentAdminsKeyboard(message.From), Constants.VOKE_NEXT_GAME_TEXT, message.From);
            }
        }

        public static async Task SendInvitesToUsers(ITelegramBotClient botClient, Message message)
        {
            var us = await botClient.GetChatMemberAsync(message.Chat.Id, message.From.Id);
            if (us.Status == ChatMemberStatus.Administrator || us.Status == ChatMemberStatus.Creator)
            {
                var users = Assist.GetNextGameUsers();
                foreach (var user in users)
                {
                    MessageSender.SendMessage(botClient,Keyboards.PrepareUnpunishmentAdminsKeyboard(message.From), Constants.VOKE_PLAYER_PLAY, user);
                }

                await users.RemoveNextGame();
            }
        }
    }
}