using Automatonymous;
using Lesson4Saga.Consumers;
using MassTransit;
using Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson4Saga
{
    public sealed class RestaurantBookingSaga : MassTransitStateMachine<RestaurantBooking>
    {
        public RestaurantBookingSaga()
        {
            InstanceState(x => x.CurrentState);

            Event(() => BookingRequested,
                x =>
                    x.CorrelateById(context => context.Message.OrderId)
                        .SelectId(context => context.Message.OrderId));

            Event(() => GuestArrivals,
                x =>
                     x.CorrelateById(context => context.Message.OrderId).SelectId(context => context.Message.OrderId));    

            Event(() => TableBooked,
                x =>
                    x.CorrelateById(context => context.Message.OrderId));

            Event(() => KitchenReady,
                x =>
                    x.CorrelateById(context => context.Message.OrderId));

            CompositeEvent(() => BookingApproved,
                x => x.ReadyEventStatus, KitchenReady, TableBooked);

            Event(() => BookingRequestFault,
                x =>
                    x.CorrelateById(m => m.Message.Message.OrderId));

            Schedule(() => BookingExpired,
                x => x.ExpirationId, x =>
                {
                    x.Delay = TimeSpan.FromSeconds(5);
                    x.Received = e => e.CorrelateById(context => context.Message.OrderId);
                });

            Initially(
                When(BookingRequested)
                    .Then(context =>
                    {
                        context.Instance.CorrelationId = context.Data.OrderId;
                        context.Instance.OrderId = context.Data.OrderId;
                        context.Instance.ClientId = context.Data.ClientId;
                        Console.WriteLine("Saga: " + context.Data.CreationDate);
                    })
                    .Schedule(BookingExpired,
                        context => new BookingExpire(context.Instance),
                        context => TimeSpan.FromSeconds(Random.Shared.Next(7,15))
                        )
                    
                    .TransitionTo(AwaitingBookingApproved)
            );

            During(AwaitingBookingApproved,
                When(BookingApproved)
                    .Unschedule(BookingExpired)
                    .Publish(context =>
                        (INotify)new Notify(context.Instance.OrderId,
                            context.Instance.ClientId,
                            $"\nОжидание клиента {context.Instance.ClientId} ... \nСтол успешно забронирован"))
                    .Finalize(),
                When(BookingRequestFault)
                    .Then(context => Console.WriteLine($"Ошибочка вышла!"))
                    .Publish(context => (INotify)new Notify(context.Instance.OrderId,
                        context.Instance.ClientId,
                        $"Приносим извинения, стол забронировать не получилось."))
                    .Publish(context => (IBookingCancellation)
                        new BookingCancellation(context.Data.Message.OrderId))
                    .Finalize(),
                 When(BookingApproved).Unschedule(BookingExpired)
                    .Then(context => Console.WriteLine($"Клиент {context.Instance.ClientId} прибыл!"))
                    .Finalize()
            );

            SetCompletedWhenFinalized();
        }
        public State AwaitingBookingApproved { get; private set; }
        public Event<IBookingRequest> BookingRequested { get; private set; }
        public Event<ITableBooked> TableBooked { get; private set; }
        public Event<IKitchenReady> KitchenReady { get; private set; }

        public Event<Fault<IBookingRequest>> BookingRequestFault { get; private set; }

        public Schedule<RestaurantBooking, IBookingExpire> BookingExpired { get; private set; }
        public Event BookingApproved { get; private set; }

        public Event<IGuestArrival> GuestArrivals { get; private set; } 

    }
}
