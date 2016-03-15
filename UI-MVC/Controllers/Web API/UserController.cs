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
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web.Mvc;
using BB.DAL.EFUser;
using BB.DAL;
using BB.DAL.EFOrganisation;
using BB.DAL.EFPlaylist;
using BB.UI.Web.MVC.Controllers.Utils;
using Google.GData.Client;

namespace BB.UI.Web.MVC.Controllers.Web_API
{
    [System.Web.Http.RoutePrefix("api/users")]
    public class UserController : ApiController
    {
        private readonly IUserManager userManager;
        private readonly IOrganisationManager organisationManager;
        private readonly IPlaylistManager playlistManager;
        private ApplicationUserManager _userManager;

        public UserController()
        {
            this.userManager = new UserManager(new UserRepository(new EFDbContext(ContextEnum.BeatBuddy)));
            this.organisationManager = new OrganisationManager(new OrganisationRepository(new EFDbContext(ContextEnum.BeatBuddy)));
            this.playlistManager = new PlaylistManager(new PlaylistRepository(new EFDbContext(ContextEnum.BeatBuddy)), new UserRepository(new EFDbContext(ContextEnum.BeatBuddy)));
        }

        public UserController(IUserManager userManager, IOrganisationManager organisationManager, IPlaylistManager playlistManager)
        {
            this.userManager = userManager;
            this.organisationManager = organisationManager;
            this.playlistManager = playlistManager;
        }
        

        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("register")]
        public async Task<IHttpActionResult> Register([FromUri] string firstName, [FromUri] string lastName, [FromUri] string nickname, [FromUri] string email, [FromUri] string password, [FromUri] string imageUrl)
        {
            User user;

            if (imageUrl != null)
            {
                // TODO check for file extension / MIME types?

                if(!(imageUrl.ToLower().EndsWith(".png") || imageUrl.ToLower().EndsWith(".jpg") || imageUrl.ToLower().EndsWith(".jpeg") || imageUrl.ToLower().EndsWith(".gif")))
                {
                    return Content(HttpStatusCode.BadRequest, "The supplied image URL is not an image");
                }

                var imageFileName = Path.GetFileName(imageUrl);
                var imagePath = FileHelper.NextAvailableFilename(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(ConfigurationManager.AppSettings["UsersImgPath"]), imageFileName));

                var webClient = new WebClient();
                webClient.DownloadFile(imageUrl, imagePath);
                imageUrl = Path.GetFileName(imagePath);
            }

            try
            {
                user = userManager.CreateUser(email, lastName, firstName, nickname, imageUrl ?? "");
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

        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("gplusRegister")]
        public async Task<IHttpActionResult> GplusRegister([FromUri] string firstName, [FromUri] string lastName, [FromUri] string nickname, [FromUri] string email, [FromUri] string password, [FromUri] string imageUrl)
        {
            var user = userManager.ReadUser(email);
            if (user != null) return Ok(user);

            return await Register(firstName, lastName, nickname, email, password, imageUrl);
        }


        // GET: api/users/heylenmatthias@gmail.com
        [System.Web.Http.Authorize]
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("{email}")]
        public IHttpActionResult GetUser(string email)
        {
            var user = userManager.ReadUser(email);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // GET: api/users/organisations
        [System.Web.Http.Authorize]
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("userOrganisations")]
        public IHttpActionResult GetUserOrganisations()
        {
            var currentUser = (User.Identity as ClaimsIdentity)?.Claims.First(c => c.Type == "sub").Value;
            var user = userManager.ReadUser(currentUser);
            if (user == null)
            {
                return NotFound();
            }

            var organisations = organisationManager.ReadOrganisationsForUser(user.Id)
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

        // GET: api/users/userPlaylists
        [System.Web.Http.Authorize]
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("userPlaylists")]
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
