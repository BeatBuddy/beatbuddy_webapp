using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Owin;

namespace BB.UI.Web.MVC.Controllers.Web_API
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }


        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            
            var result = await SignInManager.PasswordSignInAsync(context.UserName, context.Password, false, false);
            switch (result)
            {
                case SignInStatus.Success:
                    var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                    identity.AddClaim(new Claim("sub", context.UserName));
                    identity.AddClaim(new Claim("role", "user"));
                    context.Validated(identity); break; 
                
                default:
                    context.SetError("Authorization error", "The username of password is incorrect");
                    //context.Rejected();
                    break;
            }

            

        }

        private ApplicationSignInManager _signInManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                _signInManager = HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
                return _signInManager;
            }
            
        }
    }
}