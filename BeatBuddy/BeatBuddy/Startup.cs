using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BeatBuddy.Startup))]
namespace BeatBuddy
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
