using MassTransit;
using MassTransit.RabbitMqTransport;
using Messages;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson4Saga
{
    public class Worker : BackgroundService
    {
        private readonly IBus _bus;


        public Worker(IBus bus)
        {
            _bus = bus;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
                Console.WriteLine("Привет! Желаете забронировать столик?");
                var bGuid = Guid.NewGuid();
                var dateTime = DateTime.Now;
                _ = _bus.GetRabbitMqHostTopology();
                await _bus.Publish(new BookingRequest(bGuid, Guid.NewGuid(), null, dateTime),
                    stoppingToken);

            }
        }
    }
}
