using Microsoft.EntityFrameworkCore;
using PaymentGatewayWorker.EventSourcing;
using System;
using System.Collections.Generic;
using System.Text;

using Entities = PaymentGatewayWorker.Domain.Payments.Data.Entities;

namespace PaymentGatewayWorker.Domain.Payments.Data
{
    class PaymentsDbContext : DbContext
    {
        public DbSet<Entities.Payment> Payments { get; set; }
        public DbSet<LoggedEvent> LoggedEvents { get; set; }

        public PaymentsDbContext(DbContextOptions<PaymentsDbContext> options) : base(options)
        {
        }
    }
}
