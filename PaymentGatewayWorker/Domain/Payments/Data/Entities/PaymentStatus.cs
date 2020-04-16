using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGatewayWorker.Domain.Payments.Data.Entities
{
    enum PaymentStatus
    {
        CREATED,
        PROCESSING,
        APPROVED,
        DENIED
    }
}
