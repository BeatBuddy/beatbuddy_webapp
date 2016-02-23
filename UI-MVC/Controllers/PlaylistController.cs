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
            return View(playlist);
        }

        public ActionResult AddTrack(long id)
        {
            ViewBag.PlaylistId = id; // TODO: remove
            return View();
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: Playlists/Create
        [HttpPost]
        public ActionResult Create(PlaylistViewModel collection, HttpPostedFileBase image)
        {
            User playlistMaster = null;
            Organisation org = null;
            Playlist playlist = null;
            string path = null;
            bool organiserFromOrganisation = false;
            string username = User.Identity.Name;
            // TODO: Add insert logic here
            User user = userManager.ReadUser(username);
            try
            {
                playlistMaster = userManager.ReadUser(collection.PlaylistMaster);
            }
            catch
            {
                ModelState.AddModelError("user niet gevonden", new Exception("user niet gevonden"));
                return View("Create");
            }

            if (collection.Organisation != null)
            {
                try
                {
                    List<Organisation> organisations = organisationManager.ReadOrganisations(user);
                    foreach (Organisation organ in organisations)
                    {
                        if (organ.Name.Equals(collection.Organisation))
                        {
                            organiserFromOrganisation = true;
                        }
                    }
                    if (organiserFromOrganisation == false)
                        throw new Exception("You're not allowed to add playlist to this organisation");
                    org = organisationManager.ReadOrganisation(collection.Organisation);
                }
                catch
                {
                    ModelState.AddModelError("Organisation not found", new Exception("Organisation not found"));
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
                org.Playlists.Add(playlist);
            }
            playlist = playlistManager.CreatePlaylistForUser(collection.Name, collection.MaximumVotesPerUser, true, path, playlistMaster, user);

            organisationManager.UpdateOrganisation(org);
            return RedirectToAction("View/" + playlist.Id);

        }
    }
}