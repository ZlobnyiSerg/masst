using System;

namespace MT.Messages
{
    public class PlaceOrderMessage
    {
        public Guid Id { get; set; }

        public string ProductName { get; set; }

        public PlaceOrderMessage(Guid id, string productName)
        {
            Id = id;
            ProductName = productName;
        }
    }
}