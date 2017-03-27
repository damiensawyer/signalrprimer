using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using HelloSignalR.Hubs;
using Microsoft.AspNet.SignalR;

namespace HelloSignalR.Services
{
    public class SampleService
    {
        private readonly IHubContext _context;

        private static readonly Lazy<SampleService> _instance = new Lazy<SampleService>(
    () => new SampleService(GlobalHost.ConnectionManager.GetHubContext<IOCHub>()));

        private int i = 0;
        private IList<int> _history = new List<int>();
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private bool running;

        public static SampleService Instance => _instance.Value;

        public SampleService(IHubContext context)
        {
            this._context = context;
            StartAutoJob();
        }

        private Task StartAutoJob()
        {
            if (this.running)
                return Task.WhenAll();
            this._cts = new CancellationTokenSource();
            // set up infinate loop that can be cancelled
            Action<CancellationToken> mainLoop = async (t) =>
            {
                this.running = true;
                try
                {
                    while (true)
                    {
                        t.ThrowIfCancellationRequested();
                        ServerGeneratedNumber();
                        await Task.Delay(50, t);
                    }
                }
                catch (OperationCanceledException ex)
                {
                    this.running = false;
                }
            };
            return Task.Run(() => mainLoop(this._cts.Token), this._cts.Token);
        }

        /// <summary>
        /// sample of data generated from the server and sent
        /// </summary>
        private void ServerGeneratedNumber()
        {
            var r = this.i++;
            this._context.Clients.All.Pulse(r);
            this._history.Add(r);
        }

        public void StopTimer()
        {
            if (!this._cts.IsCancellationRequested)
            this._cts.Cancel();
        }

        public void RestartTimer()
        {
            if (this._cts.IsCancellationRequested)
                StartAutoJob();
        }

        /// <summary>
        /// Sample of data in the service requested from the clients 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int> GetHistory()
        {
            return this._history;
        }
    }
}