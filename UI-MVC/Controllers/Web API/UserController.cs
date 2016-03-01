using BB.BL;
using BB.BL.Domain;
using BB.UI.Web.MVC.Models;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Threading.Tasks;
using BB.BL.Domain.Users;
using System;
using System.Linq;
using System.Net;
using System.Security.Claims;

namespace BB.UI.Web.MVC.Controllers.Web_API
{
    [RoutePrefix("api/users")]
    public class UserController : ApiController
    {
        private readonly IUserManager userManager;
        private readonly IOrganisationManager organisationManager;
        private readonly IPlaylistManager playlistManager;
        private ApplicationUserManager _userManager;

        public UserController()
        {
            userManager = new UserManager(ContextEnum.BeatBuddy);
            organisationManager = new OrganisationManager(ContextEnum.BeatBuddy);
            playlistManager = new PlaylistManager(ContextEnum.BeatBuddy);
        }

        public UserController(ContextEnum contextEnum)
        {
            userManager = new UserManager(contextEnum);
            organisationManager = new OrganisationManager(contextEnum);
            playlistManager = new PlaylistManager(contextEnum);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<IHttpActionResult> Register([FromUri] string firstName, [FromUri] string lastName, [FromUri] string nickname, [FromUri] string email, [FromUri] string password)
        {
            User user;
            try
            {
                user = userManager.CreateUser(email, lastName, firstName, nickname, "");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.GetBaseException().Message);
            }

            var applicationUser = new ApplicationUser { UserName = email, Email = email };
            var resultUser = await UserManager.CreateAsync(applicationUser, password);

            if (resultUser.Succeeded){
                UserManager.AddToRole(applicationUser.Id, "User");
                return Ok(user);
            } else {
                userManager.DeleteUser(user.Id);
                return Content(HttpStatusCode.InternalServerError, "ASP.NET Identity Usermanager could not create user");
            }
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
        [Route("userOrganisations")]
        public IHttpActionResult GetUserOrganisations()
        {
            var currentUser = (User.Identity as ClaimsIdentity)?.Claims.First(c => c.Type == "sub").Value;
            var user = userManager.ReadUser(currentUser);
            if (user == null)
            {
                return NotFound();
            }

            var organisations = organisationManager.ReadOrganisations(user.Id)
                .Select(o => new
                {
                    o.Id,
                    o.Name,
                    o.BannerUrl,
                    o.ColorScheme
                })
                .AsEnumerable();
            return Ok(organisations);
        }

        // GET: api/users/1234/playlists
        [Authorize]
        [HttpGet]
        [Route("userPlaylists")]
        public IHttpActionResult GetUserPlaylists()
        {
            var currentUser = (User.Identity as ClaimsIdentity)?.Claims.First(c => c.Type == "sub").Value;
            var user = userManager.ReadUser(currentUser);
            if (user == null)
            {
                return NotFound();
            }

            var playlists = playlistManager.ReadPlaylists(user.Id)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Key,
                    p.MaximumVotesPerUser,
                    p.Active,
                    p.ImageUrl,
                    p.CreatedById,
                    p.Description
                });

            return Ok(playlists);
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
                _userManager = value;
            }
        }

    }
}
