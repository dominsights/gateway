using AutoFixture;
using CQRS;
using MediatR;
using Moq;
using PaymentGatewayWorker.CQRS;
using PaymentGatewayWorker.CQRS.CommandStack.Events;
using PaymentGatewayWorker.CQRS.CommandStack.Handlers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PaymentGatewayWorkerUnitTests
{
    public class AddNewPaymentHandlerTests
    {
        [Fact]
        public void ShouldCreateNewPayment()
        {
            var repository = new Mock<IRepository>();
            repository.Setup(r => r.CreateFromRequestAsync(It.IsAny<object>()))
                .Returns(Task.FromResult(new PaymentGatewayWorker.Domain.Payments.CommandResponse(true)));

            var mediator = new Mock<IMediator>();
            var handler = new AddNewPaymentCommandHandler(repository.Object, mediator.Object);

            var fixture = new Fixture();

            var command = fixture.Build<AddNewPaymentCommand>()
                            .Create();

            handler.Handle(command, new CancellationToken()).Wait();

            repository.Verify(r => r.CreateFromRequestAsync(It.IsAny<object>()), Times.Once);
            mediator.Verify(m => m.Publish(It.IsAny<PaymentCreatedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void ShouldNotCreateNewPaymentWhenValidationFails()
        {
            var repository = new Mock<IRepository>();
            repository.Setup(r => r.CreateFromRequestAsync(It.IsAny<object>()))
                .Returns(Task.FromResult(new PaymentGatewayWorker.Domain.Payments.CommandResponse()));

            var mediator = new Mock<IMediator>();
            var handler = new AddNewPaymentCommandHandler(repository.Object, mediator.Object);

            var fixture = new Fixture();

            var command = fixture.Build<AddNewPaymentCommand>()
                            .Create();

            handler.Handle(command, new CancellationToken()).Wait();

            repository.Verify(r => r.CreateFromRequestAsync(It.IsAny<object>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<AddNewPaymentErrorEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
