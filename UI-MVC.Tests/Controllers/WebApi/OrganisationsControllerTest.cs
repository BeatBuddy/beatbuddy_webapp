using System;
using System.Text;
using System.Collections.Generic;
using BB.BL;
using BB.BL.Domain;
using BB.BL.Domain.Organisations;
using BB.BL.Domain.Playlists;
using BB.BL.Domain.Users;
using BB.DAL;
using BB.DAL.EFOrganisation;
using BB.DAL.EFUser;
using BB.UI.Web.MVC.Controllers.Web_API;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyTested.WebApi;
using MyTested.WebApi.Builders.Contracts.Controllers;

namespace BB.UI.Web.MVC.Tests.Controllers.WebApi
{

    [TestClass]
    public class OrganisationsControllerTest
    {
        UserManager userManager;
        OrganisationManager organisationManager;
        User user;
        Organisation organisation;

        IAndControllerBuilder<OrganisationsController> _organisationControllerWithAuthenticatedUser;


        [TestInitialize]
        public void Initialize()
        {
            userManager = new UserManager(new UserRepository(new EFDbContext(ContextEnum.BeatBuddyTest)));
            organisationManager = new OrganisationManager(new OrganisationRepository(new EFDbContext(ContextEnum.BeatBuddyTest)));
            user = userManager.CreateUser("werknu@gmail.com", "matthias", "test", "acidshards", "");

            _organisationControllerWithAuthenticatedUser = MyWebApi.Controller<OrganisationsController>()
               .WithResolvedDependencyFor<IUserManager>(userManager)
               .WithResolvedDependencyFor<IOrganisationManager>(organisationManager)
               .WithAuthenticatedUser(
                u => u.WithIdentifier("NewId")
                      .WithUsername(user.Email)
                      .WithClaim(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, user.Email))
                      .WithClaim(new System.Security.Claims.Claim("sub", user.Email))
               );

           organisation =  organisationManager.CreateOrganisation("testorganisatie", "", user);

        }

        [TestMethod]
        public void GetOrganisationTest()
        {
            _organisationControllerWithAuthenticatedUser
                .Calling(o => o.Get(organisation.Id))
                .ShouldReturn()
                .Ok();
        }

        [TestCleanup]
        public void Cleanup()
        {
            organisationManager.DeleteOrganisation(organisation.Id);
            userManager.DeleteUser(user.Id);
        }


    }
}
