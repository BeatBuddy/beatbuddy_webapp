using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BB.BL;
using BB.BL.Domain;
using BB.BL.Domain.Organisations;
using BB.BL.Domain.Users;
using BB.UI.Web.MVC.Controllers.Utils;
using BB.UI.Web.MVC.Models;
using System;
using PagedList;
using Google.GData.Client;
using Google.GData.Extensions;
using Google.GData.YouTube;
using System.Net;
using BB.BL.Domain.Playlists;

namespace BB.UI.Web.MVC.Controllers
{
    public class OrganisationsController : Controller
    {
        private readonly IOrganisationManager organisationManager;
        private readonly IUserManager userManager;
        private readonly IPlaylistManager playlistManager;
        private readonly YouTubeTrackProvider youtube = new YouTubeTrackProvider();

        User user = new User()
        {
            FirstName = "Jonah"
        };

        public OrganisationsController(IOrganisationManager organisationManager, IUserManager userManager, IPlaylistManager playlistManager)
        {
            this.organisationManager = organisationManager;
            this.userManager = userManager;
            this.playlistManager = playlistManager;
        }

        // GET: Organisations
        public ActionResult Index()
        {
            IEnumerable<Organisation> organisations = organisationManager.ReadOrganisations();
            List<OrganisationViewModel> organisationViewModels = new List<OrganisationViewModel>();
            foreach (var organisation in organisations)
            {
                organisationViewModels.Add(new OrganisationViewModel()
                {
                    Id = organisation.Id,
                    Name = organisation.Name,
                    BannerUrl = organisation.BannerUrl,
                });
            }
            return View(organisationViewModels);
        }
        
        // GET: Organisations/Details/5

        public ActionResult Details(long id, int? page)
        {
            Organisation organisation = organisationManager.ReadOrganisation(id);
            if (organisation != null)
            {

                User organiser = userManager.ReadOrganiserFromOrganisation(organisation);
                IEnumerable<User> coOrganisers = userManager.ReadCoOrganiserFromOrganisation(organisation);
                
                OrganisationViewWithPlaylist organisationView = new OrganisationViewWithPlaylist()
                {
                    Id = id,
                    BannerUrl = organisation.BannerUrl,
                    Name = organisation.Name,
                    Organiser = organiser,
                    CoOrganiser = coOrganisers
                };
                var playlists = organisation.Playlists;
                int pageSize = 3;
                if (User != null)
                    user = userManager.ReadUser(User.Identity.Name);

                UserRole userRole = userManager.ReadUserRoleForUserAndOrganisation(user.Id, id);
                if (userRole == null)
                    ViewBag.Following = "None";
                else if (userRole.Role == Role.Follower)
                    ViewBag.Following = "Following";
                else if (userRole.Role == Role.Co_Organiser)
                    ViewBag.Following = "Co-Organiser";
                else if (userRole.Role == Role.Organiser)
                    ViewBag.Following = "Organiser";
                
                
                int pageNumber = (page ?? 1);
                organisationView.Playlists = playlists.ToPagedList(pageNumber, pageSize);

                ViewBag.Id = id;
                ViewBag.TotalMinutesOfPlaytime = organisationManager.ReadTotalTimeOfPlaylistsInMinutes(id);
                ViewBag.TotalVotesOnPlaylists = organisationManager.ReadTotalVotesForOrganisation(id);

                return View("Details", organisationView);

            }else
                return View("Error");
        }


        public ActionResult AddCoOrganiser(long organisation, string mail)
        {

            User user = userManager.ReadUser(mail);
            if(user == null)
            {
                return new HttpStatusCodeResult(400);
            }

            Organisation org = organisationManager.ReadOrganisation(organisation);

            IEnumerable<User> coOrganisers = userManager.ReadCoOrganiserFromOrganisation(org);

            if (coOrganisers.Contains(user) || user.Id == userManager.ReadOrganiserFromOrganisation(org).Id)
            {
                return new HttpStatusCodeResult(400);
            }

            userManager.CreateUserRole(user.Id, organisation, Role.Co_Organiser);

            return new HttpStatusCodeResult(200);
            
        }

        public ActionResult FollowOrganisation(long organisationId, string email)
        {
            User user = userManager.ReadUser(email);
            userManager.CreateUserRole(user.Id, organisationId, Role.Follower);
            return new HttpStatusCodeResult(200);
        }


        public ActionResult UnFollowOrganisation(long organisationId, string email)
        {
            User user = userManager.ReadUser(email);
            UserRole userRole = userManager.ReadUserRoleForUserAndOrganisation(user.Id, organisationId);
            userManager.DeleteUserRole(userRole);
            return new HttpStatusCodeResult(200);
        }

        public ActionResult LeaveOrganisation(long organisationId, string email)
        {
            User user = userManager.ReadUser(email);
            UserRole userRole = userManager.ReadUserRoleForUserAndOrganisation(user.Id, organisationId);
            userManager.DeleteUserRole(userRole);
            return new HttpStatusCodeResult(200);
        }

        

        public ActionResult IsNameAvailable(string name)
        {
            return Json(organisationManager.ReadOrganisations().All(org => org.Name != name),
                JsonRequestBehavior.AllowGet);
        }

        // GET: Organisations/Create
        [Authorize(Roles = "User, Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Organisations/Create
        [HttpPost]
        [Authorize(Roles = "User, Admin")]
        public ActionResult Create(OrganisationViewModel organisation, HttpPostedFileBase bannerImage)
        {
            try
            {
                string bannerPath = null;
                if (User != null)
                {
                    user = userManager.ReadUser(User.Identity.Name);
                }
                if(bannerImage != null && bannerImage.ContentLength > 0)
                {
                    var bannerFileName = Path.GetFileName(bannerImage.FileName);
                    bannerPath = FileHelper.NextAvailableFilename(Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["OrganisationsImgPath"]), bannerFileName));
                    bannerImage.SaveAs(bannerPath);
                    bannerPath = Path.GetFileName(bannerPath);
                }
                Organisation org = organisationManager.CreateOrganisation(organisation.Name, bannerPath, user);
                return RedirectToAction("Details/" + org.Id);
            }
            catch
            {
                return RedirectToAction("Create");
            }
        }

        // GET: Organisations/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Organisations/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

      

        [HttpPost]
        [Authorize(Roles = "User, Admin")]
        public ActionResult Delete(long id)
        {
            var user = userManager.ReadUser(User.Identity.Name);
            var organisation = organisationManager.ReadOrganisation(id);
            if (userManager.ReadCoOrganiserFromOrganisation(organisation).Contains(user) || 
                userManager.ReadOrganiserFromOrganisation(organisation).Equals(user))
            {
                var deletedOrganisation = organisationManager.DeleteOrganisation(id);
                if (deletedOrganisation == null) return new HttpStatusCodeResult(400);
                return new HttpStatusCodeResult(200);
            }
            return new HttpStatusCodeResult(403);
        }


        public ActionResult AddPlaylist(long playlistId, string id)
        {
            var youtubeProvider = new YouTubeTrackProvider();

            var tracks = youtubeProvider.LookUpPlaylist(id);

            foreach(Track track in tracks)
            {
                playlistManager.AddTrackToPlaylist(playlistId, track);
            }

            return null;
        }

        public JsonResult SearchPlaylist(string q)
        {
            var youtubeProvider = new YouTubeTrackProvider();
            var searchResult = youtubeProvider.SearchPlaylist(q);

            return Json(searchResult, JsonRequestBehavior.AllowGet);
        }

    }
}
