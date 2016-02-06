using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BB.UI.Web.MVC.Startup))]
namespace BB.UI.Web.MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            //
        }
    }
}
