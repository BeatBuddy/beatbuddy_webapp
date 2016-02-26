﻿using BB.UI.Web.MVC.Controllers.Web_API;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Web.Http;

namespace BB.UI.Web.MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureOwin(app);
            ConfigureAuth(app);
            
        }
    }
}
