using Bogus;
using Grpc.Net.Client;
using System;
using System.Threading;
using Tracking.API;

namespace Vavatech.DotnetCore.GrpcConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello gRPC Client!");

            const string url = "https://localhost:5001";

            var locations = new Faker<AddLocationRequest>()
                .RuleFor(p => p.DeviceId, f => f.Internet.Mac())
                .RuleFor(p => p.Latitude, f => f.Random.Float(-90f, 90f))
                .RuleFor(p => p.Longitude, f => f.Random.Float(-180f, 180f))
                .RuleFor(p => p.Speed, f => f.Random.Int(0, 140))
                .RuleFor(p=>p.Direction, f=>f.Random.Float())
                .GenerateForever();

            var channel = GrpcChannel.ForAddress(url);
            var client = new Tracking.API.TrackingService.TrackingServiceClient(channel);

            foreach (var location  in locations)
            {
                client.AddLocation(location);

                Console.WriteLine($"Sent {location.DeviceId} {location.Latitude}:{location.Longitude} {location.Speed} {location.Direction}");

                Thread.Sleep(TimeSpan.FromSeconds(0.01));
            }

          

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();

        }
    }
}
