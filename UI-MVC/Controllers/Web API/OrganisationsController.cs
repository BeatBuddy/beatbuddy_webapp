﻿using BB.BL;
using BB.BL.Domain;
using BB.BL.Domain.Organisations;
using BB.UI.Web.MVC.Controllers.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace BB.UI.Web.MVC.Controllers.Web_API
{

    public class OrganisationsController : ApiController
    {
        private readonly IOrganisationManager organisationManager;
        private readonly IUserManager userManager;

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

        // POST: api/organisations
        [HttpPost]
        [Authorize]
        [Route("organisations")]
        public IHttpActionResult Post([FromUri] string name, [FromUri] string description, [FromUri] string color, FormDataCollection formData)
        {
            string imagePath = "";
            if (formData["banner"] != null && formData["banner"].Length > 0)
            {
                try
                {
                    var bitmap = ImageDecoder.DecodeBase64String(formData["banner"]);

                    string extension = string.Empty;
                    if (bitmap.RawFormat.Equals(ImageFormat.Jpeg)) extension = ".jpg";
                    if (bitmap.RawFormat.Equals(ImageFormat.Png)) extension = ".png";
                    if (bitmap.RawFormat.Equals(ImageFormat.Bmp)) extension = ".bmp";
                    if (bitmap.RawFormat.Equals(ImageFormat.Gif)) extension = ".gif";

                    if (string.IsNullOrEmpty(extension)) return Content(HttpStatusCode.InternalServerError, "The supplied image is not a valid image");

                    imagePath = FileHelper.NextAvailableFilename(Path.Combine(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["OrganisationsImgPath"]), "organisation" + extension));
                    bitmap.Save(imagePath);

                    imagePath = Path.GetFileName(imagePath);
                }
                catch (Exception ex)
                {
                    Content(HttpStatusCode.InternalServerError, ex);
                }
            }

            var userIdentity = RequestContext.Principal.Identity as ClaimsIdentity;
            if (userIdentity == null) return InternalServerError();

            var email = userIdentity.Claims.First(c => c.Type == "sub").Value;
            if (email == null) return InternalServerError();

            var user = userManager.ReadUser(email);
            if (user == null) return InternalServerError();

            if (organisationManager.ReadOrganisation(name) == null) { return Content(HttpStatusCode.InternalServerError,"Organisation name already exists"); }

            Organisation organisation = organisationManager.CreateOrganisation(name, imagePath, color, user);

            return Ok(organisation);

        }

       
    }
}
