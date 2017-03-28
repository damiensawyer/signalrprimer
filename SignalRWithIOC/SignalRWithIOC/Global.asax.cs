using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using HelloSignalR.Services;
using SignalRWithIOC.Classes;
using RegistrationExtensions = Autofac.Integration.SignalR.RegistrationExtensions;

namespace SignalRWithIOC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //// Note that this is setting up Autofac for MVC. It is set up separately for Signal R in startup.cs (as an OWIN pipeline)
            var builder = new ContainerBuilder();
            //RegisterServices.RegisterCommonServices(builder);

            // Register MVC components. More here http://docs.autofac.org/en/latest/integration/mvc.html 
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            RegistrationExtensions.RegisterHubs(builder, typeof(MvcApplication).Assembly);
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
