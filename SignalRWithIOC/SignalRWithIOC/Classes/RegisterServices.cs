using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using HelloSignalR.Services;

namespace SignalRWithIOC.Classes
{
    public static class RegisterServices
    {
        /// <summary>
        ///  resgiering services required by both MVC and by Signal R / Owin
        /// </summary>
        /// <param name="builder"></param>
        public static void RegisterCommonServices(ContainerBuilder builder)
        {
            builder.RegisterType<NameService>().AsSelf().AsImplementedInterfaces().SingleInstance();
        }
    }
}