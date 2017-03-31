using Microsoft.AspNet.SignalR.Hubs;

namespace HelloSignalR.Services
{
    public interface INameService
    {
        string Name { get; }

        IHubConnectionContext<dynamic> Clients { get; }
    }
}