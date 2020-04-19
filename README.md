This project simulates a payment gateway which interacts with a bank api and returns the payment details to the client.

How to setup:

You will need to install the following applications:

- Postgres;
- MongoDb;
- RabbitMq;
- Dotnet core 3;

Projects setup (to run in localhost):

- Set all start up configuration to IISExpress (otherwise you'll have to configure the url for the services inside the respectives appSettings.json files;
- Set multiple projects to startup (PaymentGateway, PaymentGatewayWorker and BankSimulator);
- Go to Tools -> Options -> Debugging - Check the box "Automatically close console when debugging" (if you don't like having to close the console window everytime);

Configuration for PaymentGateway:

- Configure mongodb url / port in PaymentGateway/appSettings.json;
- Configure postgres connection string in PaymentGateway/appSettings.json;
- Add rabbitMq settings (if not using the defaults);

Configuration for PaymentGatewayWorker: 

- Configure mongodb url / port in PaymentGatewayWorker/appSettings.json;
- Configure postgres connection string in PaymentGatewayWorker/appSettings.json;
- Add rabbitMq settings (if not using the defaults);
- Configure the url for the Bank Simulator SignalR Hub (optional if you are using the IISExpress configuration);

Improvements: 

- Open only one connection for sending messages with RabbitMq;
