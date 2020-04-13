using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankSimulator.Hubs;
using BankSimulator.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BankSimulator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private IHubContext<PaymentHub> _hubContext;

        // GET: api/Payment/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Payment
        [HttpPost]
        public async Task Post([FromBody] Payment payment)
        {
            // Check card info
            // Process payment details
            // Send money to seller

            // Mock a approved response with the payment id
            var response = new PaymentResponse()
            {
                Id = payment.Id,
                Status = Status.APPROVED
            };

            // Calls the SignalR client to send response
            await _hubContext.Clients.All.SendAsync("PaymentResponse", response);
        }

        public PaymentController(IHubContext<PaymentHub> hubContext)
        {
            _hubContext = hubContext;
        }
    }
}
