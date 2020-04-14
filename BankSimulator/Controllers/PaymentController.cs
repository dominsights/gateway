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
        public async Task<Guid> Post([FromBody] Payment payment)
        {
            // Check card info
            // Process payment details
            // Send money to seller

            // Mock a approved response with the payment id
            var id = Guid.NewGuid(); // payment identifier on bank side
            
            var response = new PaymentResponse()
            {
                Id = id,
                Status = Status.APPROVED
            };

            // Calls the SignalR client to send response
            Task.Run(async () =>
            {
                // Delay execution for 3 seconds
                await Task.Delay(3 * 1000);
            _hubContext.Clients.All.SendAsync("PaymentResponse", response);
            });

            return id;
        }

        public PaymentController(IHubContext<PaymentHub> hubContext)
        {
            _hubContext = hubContext;
        }
    }
}
