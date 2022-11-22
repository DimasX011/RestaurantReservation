using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages
{
    public interface IGuestArrival
    {
        public GuestStatus status { get; }

        public Guid OrderId { get; }
    }

    public class GuestArrival : IGuestArrival
    {
        public GuestArrival(GuestStatus status, Guid guid)
        {
            Task.Delay(10000).Wait();
            this.status = status;
            OrderId = guid;
        }

       

        public GuestStatus status { get; }
        public Guid OrderId { get; }
    }


}
