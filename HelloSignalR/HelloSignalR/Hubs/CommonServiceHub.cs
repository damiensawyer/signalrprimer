using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HelloSignalR.Services;
using Microsoft.AspNet.SignalR;

namespace HelloSignalR.Hubs
{
    public class CommonServiceHub : Hub
    {
        private readonly SampleService _sampleService;

        public CommonServiceHub() : this(SampleService.Instance) // this will kick off the service which is Lazy
        {
        }

        public CommonServiceHub(SampleService sampleService)
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
    }
}