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
        public static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
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
            Console.WriteLine($"Receive message type: {message.Type}");
            if (message.Type != MessageType.Text)
                return;

            var action = (message.Text.Split(' ').First()) switch
            {
                "/next" => CreateRequestNextGame(botClient, message),//SendInlineKeyboard(botClient, message),
                "/gift" => SendGiftToFriend(botClient, message),
                "/friends" => ShowFriends(botClient, message),
                "/friendsplay" => VokeFriendsPlay(botClient, message),
                "/delfriends" => DeleteFriend(botClient, message),
                "/help" => SendHelpMessage(botClient, message),
                _ => Usage(botClient, message)
            };
            static async Task<Message> Usage(ITelegramBotClient botClient, Message message)
            {
                const string usage = "Команды бота:\n" +
                                     "/next – позвать на следующую игру\n" +
                                     "/gift – передать кредиты (количество)\n" +
                                     "/friends – добавить в друзья\n" +
                                     "/friendsplay – позвать друзей\n" +
                                     "/delfriends – удалить из друзей\n" +
                                     "/help - помощь";
                //ReplyKeyboardMarkup keyboard = Keyboards.PrepareMenuKeyboard();
                //keyboard.Keyboard = Keyboards.PrepareMenuKeyboard();
                return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                            text: usage,
                                                            replyMarkup:Keyboards.PrepareMenuKeyboard());
            }
        }

        private static async Task DeleteFriend(ITelegramBotClient botClient, Message message)
        {
            
        }

        private static async Task VokeFriendsPlay(ITelegramBotClient botClient, Message message)
        {
            
        }

        private static async Task ShowFriends(ITelegramBotClient botClient, Message message)
        {
            
        }

        private static async Task SendGiftToFriend(ITelegramBotClient botClient, Message message)
        {
            
        }

        private static async Task CreateRequestNextGame(ITelegramBotClient botClient, Message message)
        {
            
        }

        // Process Inline Keyboard callback data
        private static async Task BotOnCallbackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            await botClient.AnswerCallbackQueryAsync(
                callbackQueryId: callbackQuery.Id,
                text: $"Received {callbackQuery.Data}");

            await botClient.SendTextMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                text: $"Received {callbackQuery.Data}");
        }

        private static async Task BotOnInlineQueryReceived(ITelegramBotClient botClient, InlineQuery inlineQuery)
        {
            Console.WriteLine($"Received inline query from: {inlineQuery.From.Id}");

            InlineQueryResultBase[] results = {
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

        private static Task BotOnChosenInlineResultReceived(ITelegramBotClient botClient, ChosenInlineResult chosenInlineResult)
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