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

            // Newer way to set up infinte loop. Can be cancelled.

            this._cts = new CancellationTokenSource(); // call .cancel() to kill
            Task.Run(async () =>
            {
                var i = 0;
                var cancelled = false;
                try
                {
                    while (!cancelled)
                    {
                        _cts.Token.ThrowIfCancellationRequested();
                        this.SendPulse(i += 10);
                        await Task.Delay(500, _cts.Token); // not executed by the thread pool
                    }
                }
                catch (TaskCanceledException)
                {
                    cancelled = true;
                }
            });

            //// Other way to set up infinate loop.
            //Task.Factory.StartNew(async () =>
            //{
            //    var i = 0; // I think that each client that connects gets their own instance of this hub... therefore this value isn't shared. Need to use a common service
            //    while (true)
            //    {
            //        this.SendPulse(i++);
            //        await Task.Delay(2000);
            //    }
            //}, TaskCreationOptions.LongRunning);
        }

        public void SendPulse(int counter)
        {
            Clients.All.Pulse(counter, this._key);
        }
        public void Stop()
        {
            this._cts.Cancel();
        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            this._cts.Cancel();
            return base.OnDisconnected(stopCalled);
        }
    }
}