using MassTransit;
using Messages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Consumers
{
    public class NotifyConsumer : IConsumer<INotify>
    {
        private readonly Notifier _notifier;
        private readonly ILogger _logger;
        public NotifyConsumer(Notifier notifier, ILogger<NotifyConsumer> logger)
        {
            _notifier = notifier;
            _logger = logger;
        }
        public Task Consume(ConsumeContext<INotify> context)
        {
            _logger.Log(LogLevel.Information, $"[OrderId: {context.Message.OrderId}][ClientId: {context.Message.ClientId}][Message: {context.Message.Message}]");
            _notifier.Notify(context.Message.OrderId, context.Message.ClientId, context.Message.Message);
            return context.ConsumeCompleted;
        }
    }
}
