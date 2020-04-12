﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PaymentGatewayWorker.EventSourcing
{
    public class LoggedEvent
    {
        public Guid Id { get; set; }
        [Required]
        public string Action { get; set; }
        [Required]
        public Guid AggregateId { get; set; }
        [Required]
        public string Data { get; set; }
        [Required]
        public DateTime TimeStamp { get; set; }
    }
}
