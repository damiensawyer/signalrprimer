using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using Autofac;
using Autofac.Builder;

using Autofac.Core;
using Autofac.Integration.SignalR;
using HelloSignalR.Services;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.Owin;
using Owin;
using SignalRWithIOC;
using SignalRWithIOC.Classes;
using SignalRWithIOC.Hubs;
using IDependencyResolver = System.Web.Mvc.IDependencyResolver;

[assembly: OwinStartup(typeof(HelloSignalR.Startup))]

namespace HelloSignalR
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Autofac Owin MVC http://autofac.readthedocs.io/en/latest/integration/mvc.html#owin-integration

            //didn't get working
            // try this next? http://stackoverflow.com/a/29793864/494635



            //dmaien - read teh question and try registring the new HubConfiguration().... 

            var builder = new ContainerBuilder();
            builder.RegisterType<LoggingPipelineModule>(); // see http://docs.autofac.org/en/latest/integration/owin.html for registering custom pipeline modules
            var hubConfiguration = new HubConfiguration();

            builder.RegisterType<NameService>().SingleInstance();
            builder.RegisterInstance(hubConfiguration);

            Autofac.Integration.Mvc.RegistrationExtensions.RegisterControllers(builder, typeof(MvcApplication).Assembly);
            // Register SignalR Specific.
            builder.RegisterHubs(Assembly.GetExecutingAssembly());

            //builder.RegisterType<IOCHub>().ExternallyOwned(); // SignalR hub registration

            //// Register the Hub for DI (THIS IS THE MAGIC LINE)
            //builder.Register(i => config.Resolver.Resolve<IConnectionManager>().GetHubContext<IOCHub, IIOCHub>()).ExternallyOwned();

            builder.RegisterType<AutofacDependencyResolver>().As<Microsoft.AspNet.SignalR.IDependencyResolver>().SingleInstance();
           // builder.Register(context => context.Resolve<Microsoft.AspNet.SignalR.IDependencyResolver>().Resolve<IConnectionManager>()).SingleInstance();

            var container = builder.Build();
            hubConfiguration.Resolver = container.Resolve<Microsoft.AspNet.SignalR.IDependencyResolver>();

            // Attempts to get HubContexts into services... so that they can send to clients. 
            // http://stackoverflow.com/a/37913821

            // had this issue... 
            https://github.com/autofac/Autofac.SignalR/issues/2

                  //builder.Register((ctx, p) =>
                  //ctx.Resolve<IConnectionManager>() // this works... but wtf. Why can't I use ctx? 
                  //        ?.GetHubContext<IOCHub>())
                  //    .Named<IHubContext>("IOCHub");

            //builder.RegisterType<NameService>().WithParameter(
            //    new ResolvedParameter(
            //        (pi, ctx) => pi.ParameterType == typeof(IHubContext),
            //        (pi, ctx) => ctx.ResolveNamed<IHubContext>("IOCHub")
            //    )
            //);

            
            //config.Resolver = new AutofacDependencyResolver(container); // note, this is a different AutofacDependencyResolver to the one in global.asax. Different namespace
            app.MapSignalR("/signalr", hubConfiguration);
            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();
            
            //// To add custom HubPipeline modules, you have to get the HubPipeline
            //// from the dependency resolver, for example:

            var hubPipeline = hubConfiguration.Resolver.Resolve<IHubPipeline>();
            hubPipeline.AddModule(hubConfiguration.Resolver.Resolve<LoggingPipelineModule>());
        }
    }
}
