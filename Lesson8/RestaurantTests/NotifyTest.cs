using Kitchen;
using Lesson4Saga.Consumers;
using MassTransit;
using MassTransit.Testing;
using Messages;
using Microsoft.Extensions.DependencyInjection;
using Notification;
using Notification.Consumers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantTests
{
    [TestFixture]
    public class NotifyTest
    {
        private ServiceProvider _provider;
        private ITestHarness _harness;

        [OneTimeSetUp]
        public async Task Init()
        {
            _provider = new ServiceCollection()
                .AddMassTransitTestHarness(cfg =>
                {
                    cfg.AddConsumer<NotifyConsumer>();
                })
                .AddLogging()
                .AddSingleton<Notifier>()
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
            await _harness.Bus.Publish(
                (INotify)new Notify(
                    orderIdd, Guid.NewGuid(),"some message"
                   ));
            Assert.That(await _harness.Consumed.Any<INotify>());
        }

    }
}
