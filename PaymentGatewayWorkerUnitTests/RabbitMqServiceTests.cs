using PaymentGatewayWorker;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace PaymentGatewayWorkerUnitTests
{
    public class RabbitMqServiceTests
    {
        [Fact]
        public void ShouldTakeMessagesFromQueue()
        {
            //var rabbitMqService = new RabbitMqService();
            

            //var paymentInfo = rabbitMqService.RetrievePaymentInfoFromQueueAsync().Result;

            //ICollection<ValidationResult> validationResults = new List<ValidationResult>();
            //bool isPaymentValid = Validator.TryValidateObject(paymentInfo, new ValidationContext(paymentInfo), validationResults);
            //Assert.True(isPaymentValid);
        }
    }
}
