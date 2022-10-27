using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Restaurant.Notification;
using System.Diagnostics;
using System.Diagnostics.Metrics;


namespace TableReservation
{
    internal class Program
    {
        private static ReservationCancel _reservation = new();
        private static Restaurant _rest = new();



        static void Main(string[] args)
        {
            #region MassTransitCommunication
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            CreateHostBuilder(args).Build().Run();
            #endregion
            #region easycommunication
            /*
            Console.OutputEncoding = System.Text.Encoding.UTF8;
           var reservation = new ReservationCancel();
           var rest = new Restaurant();
            _reservation = reservation;
            _rest = rest;
            TimerCallback tm = new TimerCallback(Count);
            Timer timer = new Timer(tm, null, 0, 60000);
            while (true)
            {
                PostMessage.Send("Привет! Желаете забронировать столик?\n1 - мы уведомим вас по смс (асинхронно)" + "\n2 - подождите на линии, мы Вас оповестим (синхронно)\nОжидание ответа ...",ConsoleColor.Blue);
                if(int.TryParse(Console.ReadLine(), out var choise ) && choise is not (1 or 2 or 3))
                {
                    PostMessage.Send("Введите пожалуйста 1 или 2\nОжидание ответа ...",ConsoleColor.White);
                    continue;
                }
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                if (choise == 1)
                {
                    rest.BookFreeTableAsync(1);
                }
                else if(choise == 2)
                {
                    rest.BookFreeTable();
                }
                else if(choise == 3)
                {
                    rest.GetAllTables();
                }
                PostMessage.Send("Спасибо за ваше обращение!", ConsoleColor.White);
                stopWatch.Stop();
                var ts = stopWatch.Elapsed;
                Console.WriteLine($"{ts.Seconds:00}:{ts.Milliseconds:00}");
                Thread.Sleep(5500);
                PostMessage.Send("Желаете снять бронь? 1 - Да, 2 - нет\nОжидание ответа ...", ConsoleColor.DarkRed);
                int answer = Convert.ToInt32(Console.ReadLine());
                int choisevar = 0;
               if(answer == 1)
                {
                    PostMessage.Send("Как нам уведомить вас?\n1 - мы уведомим вас по смс (асинхронно)\n2 - подождите на линии, мы Вас оповестим (синхронно)\nОжидание ответа ...", ConsoleColor.DarkRed); 
                    do
                    {
                        choisevar = Convert.ToInt32(Console.ReadLine());
                        if (choisevar is not (1 or 2))
                        {

                            PostMessage.Send("Введите пожалуйста 1 или 2\nОжидание ответа ...", ConsoleColor.White);
                        }
                    } while (choisevar is not (1 or 2));
                    PostMessage.Send("Укажите номер столика\nОжидание ответа ...", ConsoleColor.DarkRed);
                    if (int.TryParse(Console.ReadLine(), out var choisetable) && choisetable is not (0 - 10))
                    {
                        PostMessage.Send("Введите пожалуйста 1 или 2\nОжидание ответа ...", ConsoleColor.White);
                    }
                    if (choisevar == 1)
                    {
                        rest.ReturnReservationTableAsync(choisetable);
                    }
                    else if (choisevar == 2)
                    {
                        rest.ReturTable(choisetable);
                    }
                }
                else
                {
                    continue;
                }
            }
            */
            #endregion

        }


        public static void Count(object rest)
        {
            var timego = new Stopwatch();
            timego.Start();
            Task.Run(async () =>
            {
                await _reservation.CheckReservation(_rest);
            });
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               .ConfigureServices((hostContext, services) =>
               {
                   services.AddMassTransit(x =>
                   {
                       x.UsingRabbitMq((context, cfg) =>
                       {
                           cfg.ConfigureEndpoints(context);
                       });
                       
                   });
                   services.AddMassTransitHostedService(true);

                   services.AddTransient<Restaurant>();

                   services.AddHostedService<Worker>();
               });

    }
}