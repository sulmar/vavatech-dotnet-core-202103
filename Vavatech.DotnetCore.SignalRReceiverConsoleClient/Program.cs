using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace Vavatech.DotnetCore.SignalRReceiverConsoleClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("Hello Signal-R Receiver!");

            const string url = "http://localhost:5000/signalr/messages";

            // dotnet add package Microsoft.AspNetCore.SignalR.Client
            HubConnection connection = new HubConnectionBuilder()
                .WithUrl(url)
                .WithAutomaticReconnect(new[] { TimeSpan.Zero, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(30) })                
                .Build();

            connection.Reconnecting += error =>
            {
                if (connection.State == HubConnectionState.Reconnecting)
                {
                    Console.WriteLine("Próba połączenia...");
                }

                return Task.CompletedTask;
            };

            connection.Reconnected += error =>
            {
                if (connection.State == HubConnectionState.Connected)
                {
                    Console.WriteLine("Connected again.");
                }

                return Task.CompletedTask;
            };



            connection.Closed += Connection_Closed;
            connection.Closed += ByeBye;

            connection.On<string>("YouHaveGotMessage", message => Console.WriteLine($"Received {message}"));

            Console.WriteLine("Connecting...");
            await connection.StartAsync();
            Console.WriteLine("Connected.");

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();

            Console.ResetColor();
        }

        private static Task Connection_Closed(Exception arg)
        {
            throw new NotImplementedException();
        }

        private static Task ByeBye(Exception arg)
        {
            throw new NotImplementedException();
        }
    }
}
