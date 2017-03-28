using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.SignalR;
using HelloSignalR.Hubs;
using HelloSignalR.Services;
using Microsoft.AspNet.SignalR;

namespace SignalRWithIOC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            //To get Autofac integrated with SignalR you need to reference the SignalR integration NuGet package, register your hubs, and set the dependency resolver.
            // See notes here on setting up AutoFac with SignalR

            var builder = new ContainerBuilder();
            builder.RegisterType<NameService>();

            // Register your SignalR hubs with Autofac
            // You can register hubs all at once using assembly scanning...
            //builder.RegisterHubs(Assembly.GetExecutingAssembly());
            
            // this has moved to startup.cs... I think... when using OWIN

            // ...or you can register individual hubs manually.
            builder.RegisterType<IOCHub>().ExternallyOwned();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            GlobalHost.DependencyResolver = new AutofacDependencyResolver(container);
            
            
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
