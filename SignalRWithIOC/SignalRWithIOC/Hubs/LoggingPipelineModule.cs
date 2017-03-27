using System.Diagnostics;
using Microsoft.AspNet.SignalR.Hubs;

namespace HelloSignalR
{
    /// This is just a demo to show how to create a custom hub pipeline module. This will log each incoming method call received from the client and outgoing method call invoked on the client:
    /// </summary>
    public class LoggingPipelineModule : HubPipelineModule
    {
        protected override bool OnBeforeIncoming(IHubIncomingInvokerContext context)
        {
            Debug.WriteLine("=> Invoking " + context.MethodDescriptor.Name + " on hub " + context.MethodDescriptor.Hub.Name);
            return base.OnBeforeIncoming(context);
        }
        protected override bool OnBeforeOutgoing(IHubOutgoingInvokerContext context)
        {
            Debug.WriteLine("<= Invoking " + context.Invocation.Method + " on client hub " + context.Invocation.Hub);
            return base.OnBeforeOutgoing(context);
        }
    }
}