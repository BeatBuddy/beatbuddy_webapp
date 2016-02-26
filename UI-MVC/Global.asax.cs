using System.Configuration;
using System.IO;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BB.UI.Web.MVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
       
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var appData = Server.MapPath("~/App_Data");
            if (!Directory.Exists(appData)) Directory.CreateDirectory(appData);

            var folders = new[] {"OrganisationsImgPath", "PlaylistImgPath", "UsersImgPath"};
            foreach (var folder in folders)
            {
                var mapped = Server.MapPath(ConfigurationManager.AppSettings[folder]);
                if (!Directory.Exists(mapped)) Directory.CreateDirectory(mapped);
            }
        }

    }
}
