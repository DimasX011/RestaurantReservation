using MassTransit;
using Messages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitchen.Consumers
{
    public class KitchenBookingRequestFaultConsumer : IConsumer<Fault<IBookingRequest>>
    {

        public Task Consume(ConsumeContext<Fault<IBookingRequest>> context)
        {
           
            Console.WriteLine($"[OrderId {context.Message.Message.OrderId}] прибыл");
            return Task.CompletedTask;
        }
    }
}
