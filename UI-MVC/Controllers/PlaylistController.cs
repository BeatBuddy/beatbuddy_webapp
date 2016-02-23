using System.Linq;
using System.Web.Mvc;
using BB.BL;
using BB.BL.Domain;
using BB.BL.Domain.Playlists;
using BB.BL.Domain.Users;
using BB.UI.Web.MVC.Models;
using BB.BL.Domain.Organisations;
using System.Web;
using System.IO;
using BB.UI.Web.MVC.Controllers.Utils;
using System.Configuration;
using System.Collections.Generic;
using System;

namespace BB.UI.Web.MVC.Controllers
{
    public class PlaylistController : Controller
    {
        private readonly IPlaylistManager playlistManager;
        private readonly ITrackProvider trackProvider;
        private readonly IUserManager userManager;
        private readonly IOrganisationManager organisationManager;


        string testName = "jonah@gmail.com";

        public PlaylistController(IPlaylistManager playlistManager, ITrackProvider trackProvider, UserManager userManager)
        {
            this.playlistManager = playlistManager;
            this.trackProvider = trackProvider;
            this.userManager = userManager;
        }

        public PlaylistController()
        {
            playlistManager = new PlaylistManager(ContextEnum.BeatBuddy);
            userManager = new UserManager(ContextEnum.BeatBuddy);
            organisationManager = new OrganisationManager(ContextEnum.BeatBuddy);
            trackProvider = new YouTubeTrackProvider();
        }

        public PlaylistController(ContextEnum contextEnum)
        {
            playlistManager = new PlaylistManager(contextEnum);
            userManager = new UserManager(contextEnum);
            organisationManager = new OrganisationManager(contextEnum);
            trackProvider = new YouTubeTrackProvider();
        }

        public ActionResult View(long id)
        {
            var playlist = playlistManager.ReadPlaylist(id);
            ViewBag.PlaylistId = id;
            return View(playlist);
        }

        [HttpPost]
        public ActionResult AddVote(int vote, long id)
        {
            Vote v = new Vote();
            v.Score = vote;
            return new HttpStatusCodeResult(200);
        }

        public ActionResult AddTrack(long id)
        {
            ViewBag.PlaylistId = id; // TODO: remove
            return View("AddTrack");
        }

        [HttpPost]
        public ActionResult AddTrack(long playlistId, string id)
        {
            if (!ModelState.IsValid) return View("View");

            var track = trackProvider.LookupTrack(id);
            if (track == null) return new HttpStatusCodeResult(400);

            track = playlistManager.AddTrackToPlaylist(
                playlistId,
                track.Artist,
                track.Title,
                track.TrackSource,
                track.CoverArtUrl
            );

            if(track == null) return new HttpStatusCodeResult(400);

            return new HttpStatusCodeResult(200);
        }

        public JsonResult SearchTrack(string q)
        {
            var youtubeProvider = new YouTubeTrackProvider();
            var searchResult = youtubeProvider.Search(q);

            return Json(searchResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IsNameAvailable(string email)
        {
            return Json(userManager.ReadUsers().All(org => org.Email!=email),
                JsonRequestBehavior.AllowGet);
        }

        public ActionResult IsOrganisationAvailable(string organisation)
        {
            User user = userManager.ReadUser(User.Identity.Name);
            return Json(organisationManager.ReadOrganisations(user).All(org => org.Name.Equals(organisation)),
                JsonRequestBehavior.AllowGet);
        }

        // GET: Playlists
        public ActionResult Index()
        {
            return View(playlistManager.ReadPlaylists());
        }

        // GET: Playlists/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Playlists/Create
        [Authorize(Roles = "User, Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Playlists/Create
        [HttpPost]
        [Authorize(Roles = "User, Admin")]
        public ActionResult Create(PlaylistViewModel collection, HttpPostedFileBase image)
        {
            User playlistMaster = null;
            Organisation org = null;
            Playlist playlist = null;
            User user = null;
            string path = null;
            bool organiserFromOrganisation = false;
            if(User != null)
            {
                string username = User.Identity.Name;
                user = userManager.ReadUser(username);
            }
            else
            {
                user = userManager.ReadUser(testName);
            }

            playlistMaster = userManager.ReadUser(collection.PlaylistMaster);

            if (playlistMaster == null)
            {
                ModelState.AddModelError("UserFault", "Email of playlist master is not found");
                return View("Create");
            }


            if (collection.Organisation != null)
            {
                try
                {
                    org = organisationManager.ReadOrganisation(collection.Organisation);
                    List<UserRole> userRoles = userManager.ReadOrganisationsForUser(user);
                    foreach (UserRole userRole in userRoles)
                    {
                        if (org.Id == (userRole.Organisation.Id))
                        {
                            organiserFromOrganisation = true;
                        }
                    }
                    if (organiserFromOrganisation == false)
                        throw new Exception();
                    
                }
                catch
                {
                    ModelState.AddModelError("OrganisationFault", "The organisation could not be found or you have no rights");
                    return View("Create");
                }
            }
            if (image != null && image.ContentLength > 0)
            {
                var bannerFileName = Path.GetFileName(image.FileName);
                path = FileHelper.NextAvailableFilename(Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["PlaylistImgPath"]), bannerFileName));
                image.SaveAs(path);
                path = Path.GetFileName(path);
            }
            if (org != null)
            {
                playlist = playlistManager.CreatePlaylistForOrganisation(collection.Name, collection.MaximumVotesPerUser, true, path, playlistMaster, user, org);
            }
            else
            {
                playlist = playlistManager.CreatePlaylistForUser(collection.Name, collection.Description, collection.Key, collection.MaximumVotesPerUser, true, path, playlistMaster, user);
            }
            
            

            return RedirectToAction("View/" + playlist.Id);

        }
    }
}