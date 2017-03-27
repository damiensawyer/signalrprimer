using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
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
        private CancellationTokenSource _cts;
        static ConcurrentDictionary<string, CancellationTokenSource> cancelTokens = new ConcurrentDictionary<string, CancellationTokenSource>();

        public HelloSignalRHub()
        {
            this._key = Guid.NewGuid();

        }

        public void SendPulse(int counter)
        {
            Clients.All.Pulse(counter, this._key);
        }
        public void Stop()
        {
            this.KillCounter(Context.ConnectionId);
        }

        public override async Task OnConnected()
        {
            var connectionId = Context.ConnectionId;
            var cts = new CancellationTokenSource();
            // set up infinate loop that can be cancelled
            cancelTokens.TryAdd(connectionId, cts);
            Action<CancellationToken> mainLoop = async (t) =>
            {
                var i = 0;
                try
                {
                    while (true)
                    {
                        t.ThrowIfCancellationRequested();
                        this.SendPulse(i++);
                        await Task.Delay(2000, t);
                    }
                }
                catch (OperationCanceledException ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            };
            await base.OnConnected();
            await Task.Run(() => mainLoop(cts.Token), cts.Token);
            //Task.Factory.StartNew(()=>mainLoop(this._cts.Token), this._cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }

        private void KillCounter(string connectionId)
        {
            if (cancelTokens.ContainsKey(connectionId))
            {
                cancelTokens[connectionId].Cancel();
                CancellationTokenSource result;
                cancelTokens.TryRemove(connectionId, out result);
            }
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            this.KillCounter(Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }
    }
}