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
            //didn't get working
            // try this next? http://stackoverflow.com/a/29793864/494635

            var builder = new ContainerBuilder();
            builder.RegisterType<LoggingPipelineModule>(); // see http://docs.autofac.org/en/latest/integration/owin.html for registering custom pipeline modules
            var config = new HubConfiguration();

            // Services common with SignalR and MVC
            //RegisterServices.RegisterCommonServices(builder);

            // Register SignalR Specific.
            builder.RegisterHubs(Assembly.GetExecutingAssembly());




            //        builder.Register(ctx =>
            //ctx.Resolve<IDependencyResolver>()
            //   .Resolve<IConnectionManager>()
            //   .GetHubContext<EventHub>())
            //   .Named<IHubContext>("EventHub");

            //builder.RegisterInstance(GlobalHost.ConnectionManager).As<IConnectionManager>()

            // Attempts to get HubContexts into services... so that they can send to clients. 
            // http://stackoverflow.com/a/37913821
            builder.Register((c, p) =>
            //c.Resolve<Microsoft.AspNet.SignalR.IDependencyResolver>()
            config.Resolver.Resolve<IConnectionManager>()
                    .GetHubContext<IOCHub>())
                .Named<IHubContext>("IOCHub");

            builder.RegisterType<NameService>().WithParameter(
                new ResolvedParameter(
                    (pi, ctx) => pi.ParameterType == typeof(IHubContext),
                    (pi, ctx) => ctx.ResolveNamed<IHubContext>("IOCHub")
                )
            );

            var container = builder.Build();
            config.Resolver = new AutofacDependencyResolver(container); // note, this is a different AutofacDependencyResolver to the one in global.asax. Different namespace

            //// Register the Autofac middleware FIRST, then the standard SignalR middleware.
            //// This will add the Autofac middleware as well as the middleware
            //// registered in the container - ie LoggingPipelineModule
            app.UseAutofacMiddleware(container);
            
            app.MapSignalR("/signalr", config);

            //// HOWEVER.
            //// the Autofac Signal R doc says:
            //// To add custom HubPipeline modules, you have to get the HubPipeline
            //// from the dependency resolver, for example:

            var hubPipeline = config.Resolver.Resolve<IHubPipeline>();
            hubPipeline.AddModule(config.Resolver.Resolve<LoggingPipelineModule>());

        }
    }
}
