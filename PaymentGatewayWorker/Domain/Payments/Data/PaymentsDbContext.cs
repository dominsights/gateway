using Microsoft.EntityFrameworkCore;
using PaymentGatewayWorker.Domain.Payments.Data.Entities;
using PaymentGatewayWorker.EventSourcing;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGatewayWorker.Domain.Payments.Data
{
    class PaymentsDbContext : DbContext
    {
        public DbSet<Entities.Payment> Payments { get; set; }
        public DbSet<LoggedEvent> LoggedEvents { get; set; }
        public DbSet<BankResponse> BankResponses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Entities.BankResponse>()
                .HasOne(p => p.Payment)
                .WithOne(b => b.BankResponse)
                .IsRequired();
        }

        public PaymentsDbContext(DbContextOptions<PaymentsDbContext> options) : base(options)
        {
        }
    }
}
