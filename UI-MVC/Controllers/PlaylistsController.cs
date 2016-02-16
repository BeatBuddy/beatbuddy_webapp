using BB.BL;
using BB.BL.Domain.Playlists;
using BB.BL.Domain.Users;
using BB.UI.Web.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BB.BL.Domain;

namespace BB.UI.Web.MVC.Controllers
{
    public class PlaylistsController : Controller
    {
        private IPlaylistManager playlistMgr;
        private IUserManager userMgr;

        public PlaylistsController()
        {
            playlistMgr = new PlaylistManager(ContextEnum.BeatBuddy);
            userMgr = new UserManager(ContextEnum.BeatBuddy);
        }

        public PlaylistsController(ContextEnum contextEnum)
        {
            playlistMgr = new PlaylistManager(contextEnum);
            userMgr = new UserManager(contextEnum);
        }
        // GET: Playlists
        public ActionResult Index()
        {
            
            return View(playlistMgr.ReadPlaylists());
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
        public ActionResult IsNameAvailable(string email)
        {
            return Json(userMgr.ReadUsers().All(org => org.Email != email),
                JsonRequestBehavior.AllowGet);
        }
        // POST: Playlists/Create
        [HttpPost]
        public ActionResult Create(PlaylistViewModel collection)
        {
            
            
                string username = User.Identity.Name;
                // TODO: Add insert logic here
                User user = userMgr.ReadUser(username);
                User playlistMaster = userMgr.ReadUser(collection.PlaylistMaster);
                Playlist playlist = playlistMgr.CreatePlaylistForUser(collection.Name, collection.MaximumVotesPerUser, true, collection.ImageUrl, playlistMaster, user);
            
                
                return RedirectToAction("Index");
            
            
        }

        // GET: Playlists/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Playlists/Edit/5
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

        // GET: Playlists/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Playlists/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
