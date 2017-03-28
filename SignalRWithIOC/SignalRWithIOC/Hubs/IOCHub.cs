using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using HelloSignalR.Services;
using Microsoft.AspNet.SignalR;

namespace HelloSignalR.Hubs
{
    public class IOCHub : Hub
    {
        private readonly NameService _nameService;
        private int i  = 0;

        public IOCHub(NameService nameService)
        {
            _nameService = nameService;

            Task.Run(async () =>
            {
                while (true)
                {
                    this.Pulse();
                    await Task.Delay(2000);
                }
            });
        }

        public string GetName()
        {
            return this._nameService.Name;
        }

        public void Pulse()
        {
            i++;
            Clients.All.Pulse(i);
        }

        public override Task OnConnected()
        {
            Debug.WriteLine("conected");
            return base.OnConnected();
        }
    }
}