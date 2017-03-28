using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using HelloSignalR.Hubs;
using HelloSignalR.Services;

namespace SignalRWithIOC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            //To get Autofac integrated with SignalR you need to reference the SignalR integration NuGet package, register your hubs, and set the dependency resolver.
            // See notes here on setting up AutoFac with SignalR

            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterType<NameService>();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
