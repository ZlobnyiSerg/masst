using System;

namespace MT.Messages
{
    public class OrderPlacedMessage
    {
        public Guid Id { get; set; }

        public OrderPlacedMessage(Guid id)
        {
            Id = id;
        }
    }
}