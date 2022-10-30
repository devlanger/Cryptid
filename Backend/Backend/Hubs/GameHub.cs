using BackendShared;
using Microsoft.AspNetCore.SignalR;

namespace Backend.Hubs
{
    public class GameHub : Hub, ICryptidHubClientListener
    {
        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"Connected: {Context.ConnectionId}");
            Clients.Client(Context.ConnectionId).SendAsync("ReceiveGame", "127.0.0.1:1999");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"Disconnected: {Context.ConnectionId}");
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SearchForGame()
        {
            await Clients.All.SendAsync("ReceiveGame", "127.0.0.1:1999");
        }
    }
}