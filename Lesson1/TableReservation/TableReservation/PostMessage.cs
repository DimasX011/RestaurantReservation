using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableReservation
{
    public static class PostMessage
    {
        public static Task SendAsync(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Task.Run(async () =>
            {
                await Task.Delay(1000 * 5);
                Console.WriteLine(message);

            });
            return Task.CompletedTask;
        }

        public static void Send(string message, ConsoleColor color)
        {
             Console.ForegroundColor = color;
            Console.WriteLine(message);
        }

        public static void SendCondition(Table table, string messagetrue, string messagefalse, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            if (table == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(messagetrue);
            }
            else
            {
                Console.WriteLine(messagefalse);
            }
        }

        public static Task SendConditionAsync(Table table, string messagetrue, string messagefalse, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Task.Run(async () =>
            {
                await Task.Delay(1000 * 5);
                if (table == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(messagetrue);
                }
                else
                {
                    Console.WriteLine(messagefalse);
                }

            });
            return Task.CompletedTask;
        }

        public static void SendCondition(TableView table, string messagetrue, string messagefalse, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            if (table == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(messagetrue);
            }
            else
            {
                Console.WriteLine(messagefalse);
            }
        }

        public static Task SendConditionAsync(TableView table, string messagetrue, string messagefalse, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Task.Run(async () =>
            {
                await Task.Delay(1000 * 5);
                if (table == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(messagetrue);
                }
                else
                {
                    Console.WriteLine(messagefalse);
                }

            });
            return Task.CompletedTask;
        }



    }
}
