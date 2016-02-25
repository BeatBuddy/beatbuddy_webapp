using BB.BL;
using BB.BL.Domain;
using System.Web.Http;

namespace BB.UI.Web.MVC.Controllers.Web_API
{
    [RoutePrefix("api/users")]
    public class UserController : ApiController
    {
        private readonly IUserManager userManager;
        private readonly IOrganisationManager organisationManager;

  
        public UserController()
        {
            userManager = new UserManager(ContextEnum.BeatBuddy);
            organisationManager = new OrganisationManager(ContextEnum.BeatBuddy);
        }

        public UserController(ContextEnum contextEnum) {
            userManager = new UserManager(contextEnum);
            organisationManager = new OrganisationManager(contextEnum);
        }
    

        // GET: api/users/heylenmatthias@gmail.com
        [Authorize]
        [HttpGet]
        [Route("{email}")]
        public IHttpActionResult GetUser(string email)
        {
            var user = userManager.ReadUser(email);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // GET: api/users/1234/organisations
        [Authorize]
        [HttpGet]
        [Route("{id}/organisations")]
        public IHttpActionResult GetUserOrganisations(long id)
        {
            var user = userManager.ReadUser(id);
            if (user == null)
            {
                return NotFound();
            }

            var organisations = organisationManager.ReadOrganisations(id);

            return Ok(organisations);
        }

    }
}
