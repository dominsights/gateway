This project simulates a payment gateway which interacts with a bank api and returns the payment details to the client.

**How to setup:**

  You will need to install docker and visual studio on your local machine:

  - Execute Docker as Linux containers;
  - Go to the root folder of the cloned the repository and open the solution file in Visual Studio (PaymentGateway.sln);
  - Right-click on the docker-compose project and set is as Startup Project;
  - Run the project with F5;
  - The application should start running and be available to debug and etc;

**Testing:**

  I added a postman collection with some requests you can use to play using Postman:
  
    - Payment Gateway.postman_collection.json

  Registration:

    - First execute Register request;
    - Now execute the Login request and copy the token, i.e, "token-here";

  Payments:

    - With the token copied in last step, go to "Get payment" request;
    - In the Authorization tab, select the Type "Bearer Token" and copy the token to the Token input;
    - Now you can send the payment that is in the Body tab;
    - Remember payments must be unique, you'll need to change the details, e.g, value, card number;
    - Now repeat the token copy step in the "Get payment" tab;
    - Copy the payment id returned in the "Send payment" to the end of the url;
    - Now execute and see the payment result;

  Debbuging:

    A good start point is to place breakpoints in the Handle methods of the following classes:

    - AddNewPaymentCommandHandler;
    - PaymentAcceptedEventHandler;
    - PaymentCreatedEventHandler;
    - UpdatePaymentStatusWithBankResponseCommandHandler;

    They are located in the PaymentGatewayWorker > CQRS > CommandStack > Handlers folder;

**Architecture diagram:**

Here we have an API as entry point responsible for user authentication and to queue payment requests to a RabbitMQ queue. On the other side we have workers listening to messages and running the time consuming tasks (payment processing).

The workers use event sourcing to update the payment data as inserts in the event store. They also communicate with the bank api and later schedule the payment final state to a response queue.

The API finally listens to the response queue and deliver the payment status to clients using SignalR.

![Architecture diagram](https://github.com/domicioam/gateway/blob/master/Architecture.png)

**Improvements:** 

  - Open only one connection for sending messages with RabbitMq;
  - Add authentication to SignalR;
