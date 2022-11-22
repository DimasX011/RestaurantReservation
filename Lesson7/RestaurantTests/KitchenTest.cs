using Lesson4Saga.Consumers;
using MassTransit.Testing;
using MassTransit;
using Messages.InMemoryDb;
using Messages;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kitchen.Consumers;
using Kitchen;

namespace RestaurantTests
{
    [TestFixture]
    public class KitchenTest
    {
        private ServiceProvider _provider;
        private ITestHarness _harness;

        [OneTimeSetUp]
        public async Task Init()
        {
            _provider = new ServiceCollection()
                .AddMassTransitTestHarness(cfg =>
                {
                    cfg.AddConsumer<KitchenBookingRequestedConsumer>();
                    cfg.AddConsumer<KitchenBookingRequestFaultConsumer>();
                })
                .BuildServiceProvider(true);
            _harness = _provider.GetTestHarness();
            await _harness.Start();
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            await _harness.OutputTimeline(TestContext.Out, options => options.Now().IncludeAddress());
            await _provider.DisposeAsync();
        }
        /*
        [Test]
        public async Task Any_booking_request_consumed()
        {
            var orderIdd = Guid.NewGuid();
            await _harness.Bus.Publish(
                (IKitchenReady)new KitchenReady(
                    orderIdd,true
                   ));
            Assert.That(await _harness.Consumed.Any<IKitchenReady>());
        }

        [Test]
        public async Task Booking_request_consumer_published_table_booked_message()
        {
            var consumer = _harness.GetConsumerHarness<KitchenBookingRequestedConsumer>();
            var orderId = NewId.NextGuid();
            var bus = _harness.Bus;
            await bus.Publish((IKitchenReady)
                new KitchenReady(orderId,false));
            Assert.That(consumer.Consumed.Select<IKitchenReady>()
                .Any(x => x.Context.Message.OrderId == orderId), Is.True);
        }
        */
    }
}
