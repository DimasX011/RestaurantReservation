using Kitchen.Consumers;
using MassTransit;
using MassTransit.Audit;
using Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Kitchen
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMassTransit(x =>
                    {
                        services.AddSingleton<IMessageAuditStore, AuditStoreKitchen>();
                        services.AddSingleton<IKitchenReady, KitchenReady>();

                        var serviceProvider = services.BuildServiceProvider();
                        var auditStore = serviceProvider.GetService<IMessageAuditStore>();

                        x.AddConsumer<KitchenBookingRequestedConsumer>();
                       // x.AddConsumer<KitchenBookingRequestFaultConsumer>();
                        x.AddDelayedMessageScheduler();

                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.UseDelayedMessageScheduler();
                            cfg.UseInMemoryOutbox();
                            cfg.ConfigureEndpoints(context);

                            cfg.ConnectSendAuditObservers(auditStore);
                            cfg.ConnectConsumeAuditObserver(auditStore);
                        });
                    });
                    services.AddSingleton<Manager>();
                    services.Configure<MassTransitHostOptions>(options =>
                    {
                        options.WaitUntilStarted = true;
                        options.StartTimeout = TimeSpan.FromSeconds(30);
                        options.StopTimeout = TimeSpan.FromMinutes(1);
                    });
                });
        }
    }
}