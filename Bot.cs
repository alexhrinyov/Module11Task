using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using AlexHrinyovBot2.Services;
using AlexHrinyovBot2.Controllers;

namespace AlexHrinyovBot2
{
    public class Bot : BackgroundService
    {
        private ITelegramBotClient _telegramClient;
        public TextMessageController _textMessageController;
        public InlineKeyboardController _inlineKeyboardController;
        //public MessageHandler _messageHandler = new MessageHandler(Session.OperationType);
        public MessageHandler _messageHandler;
        public Bot(ITelegramBotClient telegramClient, TextMessageController textMessageController, InlineKeyboardController inlineKeyboardController, MessageHandler messageHandler)
        {
            _textMessageController = textMessageController;
            _telegramClient = telegramClient;
            _inlineKeyboardController = inlineKeyboardController;
            _messageHandler = messageHandler;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _telegramClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                new ReceiverOptions() { AllowedUpdates = { } }, // Здесь выбираем, какие обновления хотим получать. В данном случае разрешены все
                cancellationToken: stoppingToken);

            Console.WriteLine("Бот запущен");

        }


        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {

            //  Обрабатываем нажатия на кнопки  из Telegram Bot API: https://core.telegram.org/bots/api#callbackquery
            if (update.Type == UpdateType.CallbackQuery)
            {
                await _inlineKeyboardController.Handle(update.CallbackQuery, cancellationToken);
                return;
            }

            if (update.Type == UpdateType.Message)
            {
                if (Session.OperationType == null)
                {
                    await _textMessageController.Handle(update.Message, cancellationToken);
                    return;
                }
                else
                {
                    if (update.Message.Text == "/start")
                    {
                        await _textMessageController.Handle(update.Message, cancellationToken);
                        return;
                    }
                    else
                    {
                        string message = _messageHandler.Handle(Session.OperationType, update.Message.Text);
                        await _telegramClient.SendTextMessageAsync(update.Message.Chat.Id, message, cancellationToken: cancellationToken);
                    }
                    
                }

            }


        }


        Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Задаем сообщение об ошибке в зависимости от того, какая именно ошибка произошла
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            // Выводим в консоль информацию об ошибке
            Console.WriteLine(errorMessage);

            // Задержка перед повторным подключением
            Console.WriteLine("Ожидаем 10 секунд перед повторным подключением.");
            Thread.Sleep(10000);

            return Task.CompletedTask;
        }


    }
}
