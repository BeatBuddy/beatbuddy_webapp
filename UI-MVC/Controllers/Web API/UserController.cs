﻿using BB.BL;
using BB.BL.Domain;
using BB.BL.Domain.Users;
using System.Web.Http;

namespace BB.UI.Web.MVC.Controllers.Web_API
{
    [RoutePrefix("api/users")]
    public class UserController : ApiController
    {
        private UserManager userMgr;

  
        public UserController()
        {
            userMgr = new UserManager(ContextEnum.BeatBuddy);
        }

        public UserController(ContextEnum contextEnum) {
            this.userMgr = new UserManager(contextEnum);
        }
    

        // GET: api/users/heylenmatthias@gmail.com
        [Authorize]
        [HttpGet]
        [Route("{email}")]
        public IHttpActionResult GetUser(string email)
        {
            User user = userMgr.ReadUser(email);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

    }
}
