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
            // ok. This totally did my head in. We are using Signal R as well, resgistered in Startup.cs as
            // part of the OWIN pipeline. 
            // I 'think' that by using app.UseAutofacMvc() in that file, the registrations from there are included
            // in the normal MVC pipeline and so can be used in Controller constructors etc.

            var builder = new ContainerBuilder();
            
            // Register MVC components. More here http://docs.autofac.org/en/latest/integration/mvc.html 
            // Notice that we're not registering any services. They're being done in startup.cs and 'flow through' to here.
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
