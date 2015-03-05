using System;
using Magnum.Pipeline;
using MT.Messages;

namespace MassTransit
{
    public class PlaceOrderConsumer : IConsumer<PlaceOrderMessage>
    {
        private readonly IServiceBus _bus;

        public PlaceOrderConsumer(IServiceBus bus)
        {
            _bus = bus;
        }

        public void Consume(PlaceOrderMessage message)
        {
            Console.WriteLine("Received order {0} for {1}", message.Id, message.ProductName);
            _bus.Publish(new OrderPlacedMessage(message.Id));
        }
    }
}