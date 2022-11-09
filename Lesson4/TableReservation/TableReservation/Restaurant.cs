using Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableReservation
{
    public class Restaurant
    {

        private readonly List<Table> _tables = new();

        private readonly Producer _producer =
            new("BookingNotification", "localhost");
        public Restaurant()
        {
            for(ushort i = 0; i <=10; i++)
            {
                _tables.Add(new Table(i));
            }
            
        }

        public void BookFreeTable()
        {
            PostMessage.Send("Добрый день! Подождите секунду я подберу столик и подтвержу вашу бронь, оставайтесь на линии", ConsoleColor.Yellow);
            var table = _tables.FirstOrDefault(t => t.State == State.Free);
            Thread.Sleep(5000);
            table?.SetState(State.Blocked);
            PostMessage.SendCondition(table, "К сожалению, сейчас все столики заняты", $"Готово! Ваш столик номер {table.Id}", ConsoleColor.Green);

        }

        /*
        public Task BookFreeTableAsync(int countOfPersons)
        {
            PostMessage.Send("Добрый день! Подождите секунду я подберу столик и подтвержу вашу бронь, вам придет уведомление", ConsoleColor.DarkYellow);
            Task.Run(async () =>
            {
                var table = _tables.FirstOrDefault(t => t.SeatsCount > countOfPersons && t.State == State.Free);
                await Task.Delay(1000*5);
                table?.SetState(State.Blocked);

                _producer.Send(table is null
                   ? $"Сообщение SMS: К сожалению, сейчас все столики заняты"
                   : $"Сообщение SMS: Готово! Ваш столик номер {table.Id}");
            });
            return Task.CompletedTask;
        }
        */

        public async Task<bool?> BookFreeTableAsync(int countOfPersons)
        {
            Console.WriteLine("Спасибо за Ваше обращение, я подберу столик и подтвержу вашу бронь," +
                              "Вам придет уведомление");

            var table = _tables.FirstOrDefault(t => t.SeatsCount > countOfPersons
                                                        && t.State == State.Free);
            await Task.Delay(1000 * 5); //у нас нерасторопные менеджеры, 5 секунд они находятся в поисках стола
            return table?.SetState(State.Blocked);
        }

        public void ReturTable(int countforPersons)
        {
            PostMessage.Send("Снимаю вашу бронь", ConsoleColor.Yellow);
            var table = new TableView();
            foreach (var t in _tables)
            {
                if (t.Id == countforPersons && t.State == State.Blocked)
                {
                    t.SetState(State.Free);
                    table.State = t.State;
                    table.Id = t.Id;
                }
            }
            PostMessage.SendCondition(table, "УВЕДОМЛЕНИЕ:  Упс, произошла ошибка такого столика нет", $"УВЕДОМЛЕНИЕ:Готово! Бронь со столика {table.Id} снята", ConsoleColor.Green);
        }

        public Task ReturnReservationTableAsync(int countOfPersons)
        {
            PostMessage.Send("Снимаю вашу бронь", ConsoleColor.DarkYellow);
            Task.Run(async () =>
            {
                var table = new TableView();
                foreach (var t in _tables)
                {
                    if (t.Id == countOfPersons && t.State == State.Blocked)
                    {
                        t.SetState(State.Free);
                        table.State = t.State;
                        table.Id = t.Id;
                    }
                }

                _producer.Send(table is null
                   ? $"Сообщение SMS: Упс, произошла ошибка такого столика нет"
                   : $"Сообщение SMS: Готово! Бронь со столика {table.Id} снята");
            });
            return Task.CompletedTask;  
        }

        public void GetAllTables()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Статус столиков");
            foreach (var t in _tables)
            {
                Thread.Sleep(1000);
                Console.WriteLine($"{ t.Id} - { t.State}");
            }
        }

        public  List<Table> GetTables()
        {
            List<Table> tables = new List<Table>();
            foreach (var t in _tables)
                tables.Add(t);
            return tables;
        }
    }
}
