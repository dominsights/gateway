using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway.Payments.Models;
using PaymentGateway.Payments.Services;

namespace PaymentGateway.Payments.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private PaymentService _paymentAppService;
        private ILogger<PaymentController> _logger;
        private IMapper _mapper;

        public PaymentController(PaymentService paymentAppService, ILogger<PaymentController> logger, IMapper mapper)
        {
            _paymentAppService = paymentAppService;
            _logger = logger;
            _mapper = mapper;
        }

        // GET: api/Payment
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Payment/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {



            return "value";
        }

        // POST: api/Payment
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PaymentInput input)
        {
            var dto = _mapper.Map<PaymentDto>(input);
            dto.UserId = new Guid(User.Identity.Name);

            try
            {
                var paymentId = await _paymentAppService.ProcessPaymentAsync(dto);
                dto.Id = paymentId;
                _logger.LogInformation($"Payment {paymentId} succesfully created for user {User.Identity.Name}");
                return Accepted(dto);
            } catch(Exception e)
            {
                _logger.LogError(e, $"Error while trying to issue payment for user {User.Identity.Name}", dto);
                return BadRequest(dto);
            }
        }
    }
}
