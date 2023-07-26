using HelloSignalR.Clients;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace HelloSignalR.Hubs
{
    public class HelloHub : Hub<IHelloClient>
    {
        const string ItemsKey_Score = "score";
        const string ItemsKey_TeamName = "teamName";

        public int Score
        {
            get
            {
                if (!Context.Items.TryGetValue(ItemsKey_Score, out var score))
                {
                    score = 0;
                    Context.Items[ItemsKey_Score] = score;
                }
                return (int)score;
            }
            set
            {
                Context.Items[ItemsKey_Score] = value;
            }
        }

        public override Task OnConnectedAsync()
        {
            Clients.All.ClientConnected(Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Clients.All.ClientDisconnected(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SetTeam(string teamName)
        {
            if (Context.Items.TryGetValue(ItemsKey_TeamName, out var oldTeamName))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, (string)oldTeamName);
                Context.Items.Remove(ItemsKey_TeamName);
            }

            if (!string.IsNullOrWhiteSpace(teamName))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, teamName);
                Context.Items[ItemsKey_TeamName] = teamName;
            }
        }

        public async Task SyncTextBox(string text, string receiver)
        {
            var clients = string.IsNullOrWhiteSpace(receiver) ?
                Clients.Others : Clients.OthersInGroup(receiver);
            await clients.UpdateText(
                $"ConnectionId: [{Context.ConnectionId}]\n" +
                $"Score: {Score}\n" +
                $"Message: {text}");
        }

        public async Task ScoreUp()
        {
            Score += 100;
            await Clients.Caller.UpdateScore(Score);
        }

        public async Task Abort()
        {
            Context.Abort();
            await Clients.Others.UpdateText(
                $"ConnectionId: [{Context.ConnectionId}] was ejected");
        }
    }
}