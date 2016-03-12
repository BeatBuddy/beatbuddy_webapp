using BB.UI.Web.MVC.Controllers.Web_API;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Web.Http;
using BB.UI.Web.MVC.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
namespace BB.UI.Web.MVC
{
    public partial class Startup
    {
        public virtual void Configuration(IAppBuilder app)
        {
            
            ConfigureAuth(app);
            app.Map("/signalr", map =>
            {
                map.UseCors(CorsOptions.AllowAll);
                var hubConfig = new HubConfiguration()
                {
                    EnableJSONP = true
                };
                map.RunSignalR(hubConfig);

            });
            app.Map("/api", inner =>
        {
            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
            
            ConfigureOAuth(inner);
            inner.UseWebApi(config);
        });
        }
         
    }
}
