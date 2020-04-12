using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGatewayWorker.Domain.Payments.Data.Entities
{
    enum PaymentStatus
    {
        CREATED,
        SENT,
        APPROVED,
        DENIED
    }
}
