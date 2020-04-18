using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Payments.Models
{
    public enum PaymentStatus
    {
        CREATED,
        PROCESSING,
        APPROVED,
        DENIED
    }
}
