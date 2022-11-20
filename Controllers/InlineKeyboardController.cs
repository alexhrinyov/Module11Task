using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace AlexHrinyovBot2.Controllers
{
    public class InlineKeyboardController
    {
        private readonly ITelegramBotClient _telegramClient;


        public InlineKeyboardController(ITelegramBotClient telegramBotClient)
        {
            _telegramClient = telegramBotClient;

        }
        public async Task Handle(CallbackQuery? callbackQuery, CancellationToken ct)
        {
            if (callbackQuery?.Data == null)
                return;

            // Обновление пользовательской сессии новыми данными
            Session.OperationType = callbackQuery.Data;

            // Генерим информационное сообщение
            string OperationText = callbackQuery.Data switch
            {
                "cn" => " Calculate text",
                "sn" => " Sum numbers",
                _ => string.Empty
            };

            // Отправляем в ответ уведомление о выборе
            await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id,
                $"<b>You've choose - {OperationText}.{Environment.NewLine}</b>" +
                $"{Environment.NewLine}You can change it in Main Menu.", cancellationToken: ct, parseMode: ParseMode.Html);
        }
    }
}
