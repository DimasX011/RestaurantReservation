using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages
{
    public interface IKitchenReady
    {
        public Guid OrderId { get; }
    }

    public class KitchenReady : IKitchenReady
    {
        public KitchenReady(Guid orderId, bool ready)
        {
            OrderId = orderId;
        }

        public Guid OrderId { get; }
    }
}
