using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace TGBotGame
{
    public class Handlers
    {
        public static Dictionary<long? ,User> users = new Dictionary<long?, User>();
        public static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception,
            CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException =>
                    $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
            CancellationToken cancellationToken)
        {
            var handler = update.Type switch
            {
                // UpdateType.Unknown:
                // UpdateType.ChannelPost:
                // UpdateType.EditedChannelPost:
                // UpdateType.ShippingQuery:
                // UpdateType.PreCheckoutQuery:
                // UpdateType.Poll:
                UpdateType.Message => BotOnMessageReceived(botClient, update.Message),
                UpdateType.EditedMessage => BotOnMessageReceived(botClient, update.EditedMessage),
                UpdateType.CallbackQuery => BotOnCallbackQueryReceived(botClient, update.CallbackQuery),
                UpdateType.InlineQuery => BotOnInlineQueryReceived(botClient, update.InlineQuery),
                UpdateType.ChosenInlineResult => BotOnChosenInlineResultReceived(botClient, update.ChosenInlineResult),
                _ => UnknownUpdateHandlerAsync(botClient, update)
            };

            try
            {
                await handler;
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(botClient, exception, cancellationToken);
            }
        }

        private static async Task BotOnMessageReceived(ITelegramBotClient botClient, Message message)
        {
            if (message.Type != MessageType.Text)
                return;
            if (message.From.Id != message.Chat.Id)
            {
                var action = (message.Text.Split(' ').First()) switch
                {
                    "/next" => GroupFunctions.CreateRequestNextGame(botClient, message),
                    "/gift" => GroupFunctions.SendGiftToFriend(botClient, message),
                    "/friends" => GroupFunctions.ShowFriends(botClient, message),
                    "/friendsplay" => GroupFunctions.VokeFriendsPlay(botClient, message),
                    "/delfriends" => GroupFunctions.DeleteFriend(botClient, message),
                    "/help" => SendHelpMessage(botClient, message),
                    _ => Usage(botClient, message, Constants.GROUP_USAGE, new ReplyKeyboardMarkup())
                };
            }
            else
            {
                if (!users.ContainsKey(message.Chat.Id))
                {
                    users.Add(message.Chat.Id, new User(message.Chat.Id));
                }
                switch (message.Text)
                {
                    case "🗣 Позвать меня на следующую игру":
                        await PrivateChatFunctions.VokeToNextGame(message.Chat.Id, botClient);
                        break;
                    case "❓ Узнать причину и снять мут/варн/бан":
                        users[message.Chat.Id].keyboardNavigator.PushToReasonPunishment(message.Chat.Id, botClient);
                        break;
                    case "🤝 Кто у меня в друзьях?":
                        users[message.Chat.Id].keyboardNavigator.PushToFriends(message.Chat.Id, botClient);
                        break;
                    case "📕 Правила чата и игры":
                        await PrivateChatFunctions.GetRules(message.Chat.Id, botClient);
                        break;
                    case "🤵🏻 Описание ролей":
                        await PrivateChatFunctions.GetRolesDescription(message.Chat.Id, botClient);
                        break;
                    case "Мои друзья":
                        await PrivateChatFunctions.GetFriendsList(message.Chat.Id, botClient);
                        break;
                    case "Удалить из друзей":
                        await PrivateChatFunctions.GetRemoveList(message.Chat.Id, botClient);
                        break;
                    case "Причина варна":
                        await PrivateChatFunctions.GetReason(message.Chat.Id, PrivateChatFunctions.Punishments.Warn, botClient);
                        break;
                    case "Причина мута":
                        await PrivateChatFunctions.GetReason(message.Chat.Id, PrivateChatFunctions.Punishments.Mute, botClient);
                        break;
                    case "Причина бана":
                        await PrivateChatFunctions.GetReason(message.Chat.Id, PrivateChatFunctions.Punishments.Ban, botClient);
                        break;
                    case "Узнать причину":
                        users[message.Chat.Id].keyboardNavigator.PushToReason(message.Chat.Id, botClient);
                        break;
                    case "Снять наказание":
                        users[message.Chat.Id].keyboardNavigator.PushToPunishment(message.Chat.Id, botClient);
                        break;
                    case "Пополнить баланс":
                        users[message.Chat.Id].keyboardNavigator.PushToFillBalance(message.Chat.Id, botClient);
                        break;
                    case"5 кредитов":
                        await PrivateChatFunctions.FillBalance(message.Chat.Id, PrivateChatFunctions.Amount.Five, botClient);
                        break;
                    case"10 кредитов":
                        await PrivateChatFunctions.FillBalance(message.Chat.Id, PrivateChatFunctions.Amount.Ten, botClient);
                        break;
                    case"20 кредитов":
                        await PrivateChatFunctions.FillBalance(message.Chat.Id, PrivateChatFunctions.Amount.Twenty, botClient);
                        break;
                    case "Снять варн":
                        await PrivateChatFunctions.RemovePunishment(message.Chat.Id, PrivateChatFunctions.Punishments.Warn, botClient);
                        break;
                    case "Снять мут":
                        await PrivateChatFunctions.RemovePunishment(message.Chat.Id, PrivateChatFunctions.Punishments.Mute, botClient);
                        break;
                    case "Снять бан":
                        await PrivateChatFunctions.RemovePunishment(message.Chat.Id, PrivateChatFunctions.Punishments.Ban, botClient);
                        break;
                    default:
                        await Usage(botClient, message, Constants.USER_USAGE, Keyboards.PrepareMenuKeyboard());
                        break;
                }
            }

            static async Task<Message> Usage(ITelegramBotClient botClient, Message message, string mes, ReplyKeyboardMarkup keyboard)
            {
                return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                    text: mes,
                    replyMarkup: keyboard);
            }
        }

        // Process Inline Keyboard callback data
        private static async Task BotOnCallbackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            if (users.ContainsKey(callbackQuery.Message.Chat.Id)&&
                users[callbackQuery.Message.Chat.Id].curState == KeyboardsNavigator.CurentState.Friends
                )
            {
                long? whomDelete = long.Parse(callbackQuery.Data);
                //in inline keyboard you`ll see names of users, but in data will be there chat id`s
                await PrivateChatFunctions.RemoveFriend(callbackQuery.Message.Chat.Id, botClient, whomDelete);
            }
        }

        private static async Task BotOnInlineQueryReceived(ITelegramBotClient botClient, InlineQuery inlineQuery)
        {
            Console.WriteLine($"Received inline query from: {inlineQuery.From.Id}");

            InlineQueryResultBase[] results =
            {
                // displayed result
                new InlineQueryResultArticle(
                    id: "3",
                    title: "TgBots",
                    inputMessageContent: new InputTextMessageContent(
                        "hello"
                    )
                )
            };

            await botClient.AnswerInlineQueryAsync(
                inlineQueryId: inlineQuery.Id,
                results: results,
                isPersonal: true,
                cacheTime: 0);
        }

        private static async Task SendHelpMessage(ITelegramBotClient botClient, Message message)
        {
            Console.WriteLine($"Help");
        }

        private static Task BotOnChosenInlineResultReceived(ITelegramBotClient botClient,
            ChosenInlineResult chosenInlineResult)
        {
            Console.WriteLine($"Received inline result: {chosenInlineResult.ResultId}");
            return Task.CompletedTask;
        }

        private static Task UnknownUpdateHandlerAsync(ITelegramBotClient botClient, Update update)
        {
            Console.WriteLine($"Unknown update type: {update.Type}");
            return Task.CompletedTask;
        }
    }
}