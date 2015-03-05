using System;
using MassTransit.Saga;
using MassTransit.Services.Subscriptions.Server;
using MT.Messages;

namespace MassTransit
{
    class Program
    {
        static void Main(string[] args)
        {
            var subscriptionBus = ServiceBusFactory.New(sbc =>
            {
                sbc.UseMsmq();
                sbc.SetConcurrentConsumerLimit(1);
                sbc.ReceiveFrom("msmq://localhost/mt.control");
            });

            var subscriptionSagas = new InMemorySagaRepository<SubscriptionSaga>();
            var subscriptionClientSagas = new InMemorySagaRepository<SubscriptionClientSaga>();
            var subscriptionService = new SubscriptionService(subscriptionBus, new , subscriptionClientSagas);

            subscriptionService.Start();

            var bus = ServiceBusFactory.New(sbc =>
            {
                sbc.UseMsmq(mc =>
                {
                    mc.VerifyMsmqConfiguration();
                    mc.UseSubscriptionService("msmq://localhost/mt.control");
                });
                sbc.VerifyMsDtcConfiguration();                
                sbc.UseControlBus();
                sbc.ReceiveFrom("msmq://localhost/mt.serv");

                sbc.Subscribe(subs =>
                {
                    subs.Handler<PlaceOrderMessage>((ctx, msg) =>
                    {
                        Console.WriteLine("Received order {0} for {1}", msg.Id, msg.ProductName);
                        ctx.Respond(new OrderPlacedMessage(msg.Id));
                    });
                });
            });


            Console.WriteLine("Waiting for messages");
            Console.ReadLine();
        }
    }
}
