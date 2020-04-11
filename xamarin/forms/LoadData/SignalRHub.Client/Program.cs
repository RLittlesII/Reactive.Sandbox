using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace SignalRHub.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var connection = new HubConnectionBuilder().WithUrl("https://localhost:5001/coffee").Build();
            connection.On<string, string>("ReceivedMessage", (user, message) =>
            {
                Console.WriteLine($"{user}: {message}");
            });

            await connection.StartAsync();

            for (int i = 0; i < 100; i++)
            {
                await connection.InvokeAsync("SendMessage", "rlittlesii", "message" + i.ToString());
            }
        }
    }
}