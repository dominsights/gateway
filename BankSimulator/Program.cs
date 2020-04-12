using System;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Settings;

namespace BankSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            var stub = FluentMockServer.Start(new FluentMockServerSettings
            {
                Urls = new[] { "http://+:5001" },
                StartAdminInterface = true
            });

            stub.Given(
                Request.Create()
                .WithPath("/helloworld")
                .UsingGet())
                .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBodyAsJson(new { Message = "Hello world" }));

            Console.WriteLine("Press any key to stop the server...");
            Console.ReadLine();
            stub.Stop();
        }
    }
}
