using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Organizer.App_Start;
using Organizer.Data;
using Organizer.Scripts;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

[assembly: OwinStartupAttribute(typeof(Startup))]
namespace Organizer.App_Start
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            GlobalHost.DependencyResolver.Register(
                typeof(MessagesHub),
                () => GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(MessagesHub))
            );
            GlobalHost.DependencyResolver.Register(
                typeof(NotificationHub),
                () => GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(NotificationHub))
            );

            var hubConfiguration = new HubConfiguration();
            hubConfiguration.EnableDetailedErrors = true;
            app.MapSignalR(hubConfiguration);
        }

        
    }
}