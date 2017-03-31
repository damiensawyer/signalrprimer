using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using SignalRWithIOC.Hubs;

namespace HelloSignalR.Services
{
    public class NameService : INameService
    {
        private int i = 0;
        private IHubContext _context;

        public NameService(HubConfiguration hubConfiguration)
        {
            this._context = hubConfiguration.Resolver.Resolve<IConnectionManager>().GetHubContext<IOCHub>(); 
            
            Task.Run(async () =>
            {
                // This isn't that neat. I tried (for ages) to get this going http://stackoverflow.com/questions/29783898/owin-signalr-autofac/29793864#29793864 but couldn't
                // To be honest though, I was 16 hours awake tired.... probably simple.
                while (true)
                {
                    this.Clients.All.ServicePulse($"From Name Service {this.i++}");
                    await Task.Delay(2000);
                }
            });
        }

        public IHubConnectionContext<dynamic> Clients
        {
            get { return _context.Clients; }
        }

        public string Name { get; } = "Damien";
    }
}