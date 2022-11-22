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
                })
                .AddLogging()
                .AddSingleton<Manager>()
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
        
        [Test]
        public async Task Any_kitchen_request_consumed()
        { 
            var orderIdd = Guid.NewGuid();
            await _harness.Bus.Publish((IBookingRequest)new BookingRequest(orderIdd, Guid.NewGuid(), null, DateTime.Now));
            Assert.That(await _harness.Consumed.Any<IBookingRequest>());
        }    
    }
}
