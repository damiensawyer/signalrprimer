using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using SignalRWithIOC.Hubs;

namespace HelloSignalR.Services
{
    public class NameService : INameService
    {
        private int i = 0;
        public NameService(IHubContext hubContext)
        {
             
            Task.Run(async () =>
            {
                while (true)
                {
                    hubContext.Clients.All.ServicePulse($"From Name Service {this.i++}");
                    await Task.Delay(2000);
                }
            });
        }

        public string Name { get; } = "Damien";
    }
}