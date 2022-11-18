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
    public class KitchenBookingRequestedConsumer : IConsumer<IBookingRequest>
    {
        private readonly Manager _manager;
        private readonly IBus _bus;
        private readonly ILogger _logger;


        public KitchenBookingRequestedConsumer(Manager manager, IBus bus, ILogger<KitchenBookingRequestedConsumer> logger)
        {
            _manager = manager;
            _bus = bus;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IBookingRequest> context)
        {
            _logger.Log(LogLevel.Information, $"[OrderId: {context.Message.OrderId}]");
            await Task.Delay(500000);
            if (_manager.CheckKitchenReady(context.Message.OrderId, context.Message.PreOrder))
                await _bus.Publish<IKitchenReady>(new KitchenReady(context.Message.OrderId, true));
         
        }
    }
}
