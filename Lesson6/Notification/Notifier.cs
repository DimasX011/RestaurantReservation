﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification
{
    public class Notifier
    {
        public void Notify(Guid orderId, Guid clientId, string message)
        {
            Console.WriteLine($"[OrderID: {orderId}] Уважаемый клиент {clientId}! {message}");
        }
    }
}
