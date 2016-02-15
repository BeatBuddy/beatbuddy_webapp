using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BB.BL;
using BB.BL.Domain.Organisations;
using BB.BL.Domain.Users;
using BB.UI.Web.MVC.Models;

namespace BB.UI.Web.MVC.Controllers
{
    public class OrganisationsController : Controller
    {

        User user = new User()
        {
            FirstName = "Jonah"
        };
        private readonly IOrganisationManager organisationManager = new OrganisationManager();
        // GET: Organisations
        public ActionResult Index()
        {
            List<Organisation> organisations = organisationManager.ReadOrganisations();
            //organisations = organisations.FindAll(m => m.Users.ContainsKey(user));
            
            return View(organisations);
        }
        
        // GET: Organisations/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Organisations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Organisations/Create
        [HttpPost]
        public ActionResult Create(Organisation organisation)
        {
            try
            {
                organisationManager.CreateOrganisation(organisation.Name, organisation.BannerUrl, organisation.ColorScheme, organisation.Key, user);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
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

        // GET: Organisations/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Organisations/Delete/5
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
