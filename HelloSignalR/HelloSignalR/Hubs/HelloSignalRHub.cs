using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace HelloSignalR.Hubs
{
    public class HelloSignalRHub : Hub
    {
        private readonly Guid _key;
        private readonly CancellationTokenSource _cts;

        public HelloSignalRHub()
        {
            this._key = Guid.NewGuid();
            this._cts = new CancellationTokenSource();
        }

        public void SendPulse(int counter)
        {
            Clients.All.Pulse(counter, this._key);
        }
        public void Stop()
        {
            this._cts.Cancel(true);
        }

        public override Task OnConnected()
        {
            // set up infinate loop that can be cancelled
            Task.Factory.StartNew(async () =>
            {
                var i = 0; // I think that each client that connects gets their own instance of this hub... therefore this value isn't shared. Need to use a common service
                while (true)
                {
                    this.SendPulse(i++);
                    await Task.Delay(2000);
                }
            }, this._cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            this._cts.Cancel();
            return base.OnDisconnected(stopCalled);
        }
    }
}