using System;
using System.Text;
using System.Threading.Tasks;
using AlexHrinyovBot2.Controllers;
using AlexHrinyovBot2.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;

namespace AlexHrinyovBot2
{
    public class Program
    {

        public static async Task Main()
        {
            Console.OutputEncoding = Encoding.Unicode;

            // Объект, отвечающий за постоянный жизненный цикл приложения
            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) => ConfigureServices(services)) // Задаем конфигурацию
                .UseConsoleLifetime() // Позволяет поддерживать приложение активным в консоли
                .Build(); // Собираем

            Console.WriteLine("Сервис запущен");
            // Запускаем сервис
            await host.RunAsync();
            Console.WriteLine("Сервис остановлен");

        }

        static void ConfigureServices(IServiceCollection services)
        {
            // Регистрируем объект TelegramBotClient c токеном подключения
            services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient("5797711147:AAFiZxgUmDJ03ToquAML-yGrTcUaohGStrE"));
            // Регистрируем постоянно активный сервис бота
            services.AddHostedService<Bot>();
            //контроллер текстовых сообщений
            services.AddTransient<TextMessageController>();
            //контроллер нажатий на кнопки
            services.AddTransient<InlineKeyboardController>();
            // обработчик сообщений(расчет суммы или количества символов)
            services.AddTransient<MessageHandler>();

        }
    }
}
