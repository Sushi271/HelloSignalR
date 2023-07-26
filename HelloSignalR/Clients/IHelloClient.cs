using System.Threading.Tasks;

namespace HelloSignalR.Clients
{
    public interface IHelloClient
    {
        Task UpdateText(string text);
        Task UpdateScore(int score);
        Task ClientConnected(string client);
        Task ClientDisconnected(string client);
    }
}
