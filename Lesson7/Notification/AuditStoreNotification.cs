using MassTransit.Audit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Notification
{
    public class AuditStoreNotification : IMessageAuditStore
    {
        private readonly ILogger _logger;

        public AuditStoreNotification(ILogger<AuditStoreNotification> logger)
        {
            _logger = logger;
        }

        public Task StoreMessage<T>(T message, MessageAuditMetadata metadata) where T : class
        {
            _logger.Log(LogLevel.Information,
                JsonSerializer.Serialize(metadata) + "\n" + JsonSerializer.Serialize(message));
            return Task.CompletedTask;
        }
    }
}
