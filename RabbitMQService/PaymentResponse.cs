using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQService
{
    public class PaymentResponse
    {
        public Guid PaymentId { get; set; }
        public string Status { get; set; }

        public PaymentResponse(Guid paymentId, string status)
        {
            PaymentId = paymentId;
            Status = status;
        }
        public PaymentResponse()
        {

        }

        public override bool Equals(object obj)
        {
            return obj is PaymentResponse other &&
                   PaymentId.Equals(other.PaymentId) &&
                   Status == other.Status;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PaymentId, Status);
        }
    }
}
