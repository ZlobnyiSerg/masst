using System;
using MassTransit;
using MT.Messages;

namespace MT.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            var bus = ServiceBusFactory.New(sbc =>
            {
                sbc.UseMsmq(mc =>
                {
                    mc.VerifyMsmqConfiguration();
                    mc.UseSubscriptionService("msmq://localhost/mt.control");
                });
                sbc.VerifyMsDtcConfiguration();                
                sbc.ReceiveFrom("msmq://localhost/mt.cli");
                sbc.UseControlBus();

                sbc.Subscribe(subs =>
                {
                    subs.Handler<OrderPlacedMessage>((ctx, msg) =>
                    {
                        Console.WriteLine("Confirmed order {0}", msg.Id);                        
                    }).Permanent();
                });
            });
            while (true)
            {
                Console.WriteLine("Press enter to order");
                Console.ReadLine();
                bus.GetEndpoint(new Uri("msmq://localhost/mt.serv")).Send(new PlaceOrderMessage(Guid.NewGuid(), "Some product"));
            }
        }
    }
}
