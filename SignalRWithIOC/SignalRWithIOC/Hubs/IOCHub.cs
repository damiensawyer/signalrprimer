using System.Collections.Generic;
using HelloSignalR.Services;
using Microsoft.AspNet.SignalR;

namespace HelloSignalR.Hubs
{
    /// <summary>
    /// Shows a hub which uses a common service to store state and other things. The service 
    /// can also push data to the clients. Note that, to do that, the hub doesn't actually need to keep 
    /// a reference to the service. We only get a reference in here so that clicents can call methods on the service.
    /// 
    /// I haven't set up any client code to consume this service.... or tested it for that matter!!
    /// </summary>
    public class Second_CommonServiceHub : Hub
    {
        private readonly SampleService _sampleService;

        public Second_CommonServiceHub() : this(SampleService.Instance) // this will kick off the service which is Lazy
        {
        }

        Second_CommonServiceHub(SampleService sampleService)
        {
            this._sampleService = sampleService;
        }
        public void StopTimer()
        {
            this._sampleService.StopTimer();
        }

        public void RestartTimer()
        {
            this._sampleService.RestartTimer();
        }

        /// <summary>
        /// shows getting history from common service
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int> GetHistory()
        {
            return this._sampleService.GetHistory();
        }
    }
}