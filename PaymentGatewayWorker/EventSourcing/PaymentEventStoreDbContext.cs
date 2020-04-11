using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGatewayWorker.EventSourcing
{
    class PaymentEventStoreDbContext : DbContext
    {
        public PaymentEventStoreDbContext(DbContextOptions<PaymentEventStoreDbContext> options) : base(options)
        {
        }

        public DbSet<LoggedEvent> LoggedEvents { get; set; }
    }
}
