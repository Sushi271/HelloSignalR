using HelloSignalR.Clients;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace HelloSignalR.Hubs
{
    public class HelloHub : Hub<IHelloClient>
    {
        public async Task SyncTextBox(string text)
        {
            Context.Items.TryGetValue("previousText", out var previousText);
            await Clients.Others.UpdateText(text + $", prev: {previousText ?? "NULL"}");
            Context.Items["previousText"] = text;
        }
    }
}