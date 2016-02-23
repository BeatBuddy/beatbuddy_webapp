using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using BB.BL;
using BB.BL.Domain;
using BB.BL.Domain.Organisations;
using BB.BL.Domain.Users;
using BB.UI.Web.MVC.Controllers.Utils;
using BB.UI.Web.MVC.Models;

namespace BB.UI.Web.MVC.Controllers
{
    public class OrganisationsController : Controller
    {
        private readonly IOrganisationManager organisationManager;
        private readonly IUserManager userManager;

        User user = new User()
        {
            FirstName = "Jonah"
        };

        public OrganisationsController()
        {
            organisationManager = new OrganisationManager(ContextEnum.BeatBuddy);
            userManager = new UserManager(ContextEnum.BeatBuddy);
        }

        public OrganisationsController(ContextEnum contextEnum)
        {
            organisationManager = new OrganisationManager(contextEnum);
            userManager = new UserManager(contextEnum);
        }

        // GET: Organisations
        public ActionResult Index()
        {
            List<Organisation> organisations = organisationManager.ReadOrganisations();
            List<OrganisationViewModel> organisationViewModels = new List<OrganisationViewModel>();
            foreach (var organisation in organisations)
            {
                organisationViewModels.Add(new OrganisationViewModel()
                {
                    Name = organisation.Name,
                    BannerUrl = organisation.BannerUrl,
                    ColorScheme = organisation.ColorScheme,
                    ImageUrl = organisation.ImageUrl
                });
            }
            return View(organisationViewModels);
        }
        
        // GET: Organisations/Details/5
        public ActionResult Details(long id)
        {
            Organisation organisation = organisationManager.ReadOrganisation(id);
            if (organisation != null)
            {
                OrganisationViewModel organisationView = new OrganisationViewModel()
                {
                    BannerUrl = organisation.BannerUrl,
                    ColorScheme = organisation.ColorScheme,
                    Name = organisation.Name,
                    ImageUrl = organisation.ImageUrl
                };
                return View("Details", organisationView);

            }else
                return View("Error");
        }

        // GET: Organisations/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult IsNameAvailable(string name)
        {
            return Json(organisationManager.ReadOrganisations().All(org => org.Name != name),
                JsonRequestBehavior.AllowGet);
        }

        // POST: Organisations/Create
        [HttpPost]
        public ActionResult Create(OrganisationViewModel organisation, HttpPostedFileBase bannerImage, HttpPostedFileBase avatarImage)
        {
            try
            {
                string bannerPath = null;
                string avatarPath = null;
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
                if(avatarImage != null && avatarImage.ContentLength > 0)
                {
                    var avatarFileName = Path.GetFileName(avatarImage.FileName);
                    avatarPath = FileHelper.NextAvailableFilename(Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["OrganisationsImgPath"]), avatarFileName));
                    avatarImage.SaveAs(avatarPath);
                    avatarPath = Path.GetFileName(avatarPath);
                }
                organisationManager.CreateOrganisation(organisation.Name, bannerPath, avatarPath, organisation.ColorScheme, user);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(organisation);
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
