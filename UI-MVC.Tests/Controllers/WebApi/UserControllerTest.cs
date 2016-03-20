using Microsoft.VisualStudio.TestTools.UnitTesting;
using BB.BL;
using BB.BL.Domain.Users;
using BB.BL.Domain.Playlists;
using BB.DAL.EFPlaylist;
using BB.DAL;
using BB.BL.Domain;
using BB.DAL.EFUser;
using BB.BL.Domain.Organisations;
using BB.UI.Web.MVC.Controllers.Web_API;
using MyTested.WebApi;
using BB.DAL.EFOrganisation;
using System.Collections.Generic;
using System.Linq;
using BB.UI.Web.MVC.Models;
using MyTested.WebApi.Builders.Contracts.Controllers;

namespace BB.UI.Web.MVC.Tests.Controllers.WebApi
{
    [TestClass]
    public class UserControllerTest
    {
        
        PlaylistManager playlistManager;
        UserManager userManager;
        OrganisationManager organisationManager;
        User _userWithOrganisation;
        private User _userWithoutOrganisation;
        Playlist playlist;
        Organisation organisation;

        private IAndControllerBuilder<UserController> _userControllerWithAuthenticatedUserWithOrganisation;
        private IAndControllerBuilder<UserController> _userControllerWithAuthenticatedUserWithoutOrganisation;

        [TestInitialize]
        public void Initialize()
        {
           
            playlistManager = new PlaylistManager(new PlaylistRepository(new EFDbContext(ContextEnum.BeatBuddyTest)), new UserRepository(new EFDbContext(ContextEnum.BeatBuddyTest)));
            userManager = new UserManager(new UserRepository(new EFDbContext(ContextEnum.BeatBuddyTest)));
            organisationManager = new OrganisationManager(new OrganisationRepository(new EFDbContext(ContextEnum.BeatBuddyTest)));
            _userWithOrganisation = userManager.CreateUser("werknu@gmail.com", "matthias", "test", "acidshards", "");
            _userWithoutOrganisation = userManager.CreateUser("testje@gmail.com", "heylen", "jos", "acidshards", "");

            playlist = playlistManager.CreatePlaylistForUser("testplaylist", "gekke playlist om te testen", "125", 5, true, "", _userWithOrganisation);
            _userControllerWithAuthenticatedUserWithOrganisation = MyWebApi.Controller<UserController>()
                .WithResolvedDependencyFor<PlaylistManager>(playlistManager)
                .WithResolvedDependencyFor<UserManager>(userManager)
                .WithResolvedDependencyFor<OrganisationManager>(organisationManager)
                .WithAuthenticatedUser(
                 u => u.WithIdentifier("NewId")
                       .WithUsername(_userWithOrganisation.Email)
                       .WithClaim(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, _userWithOrganisation.Email))
                       .WithClaim(new System.Security.Claims.Claim("sub", _userWithOrganisation.Email))
                );

            _userControllerWithAuthenticatedUserWithoutOrganisation = MyWebApi.Controller<UserController>()
                .WithResolvedDependencyFor<PlaylistManager>(playlistManager)
                .WithResolvedDependencyFor<UserManager>(userManager)
                .WithResolvedDependencyFor<OrganisationManager>(organisationManager)
                .WithAuthenticatedUser(
                 u => u.WithIdentifier("NewId")
                       .WithUsername(_userWithOrganisation.Email)
                       .WithClaim(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, _userWithoutOrganisation.Email))
                       .WithClaim(new System.Security.Claims.Claim("sub", _userWithoutOrganisation.Email))
                );

            Track track = new Track()
            {
                Artist = "Metallica",
                Title = "Master of Puppets (live)",
                CoverArtUrl = "",
                Duration = 800,
                TrackSource = new TrackSource
                {
                    SourceType = SourceType.YouTube,
                    Url = "https://www.youtube.com/watch?v=kV-2Q8QtCY4"
                }
            };
            Track addedtrack = playlistManager.AddTrackToPlaylist(playlist.Id, track);


            organisation = organisationManager.CreateOrganisation("gek organisatie test","",_userWithOrganisation);
        }

        [TestMethod]
        public void GetUserOrganisationsTest() {
            _userControllerWithAuthenticatedUserWithOrganisation
                .Calling(c => c.GetUserOrganisations())
                .ShouldReturn()
                .Ok()
                .WithResponseModelOfType<IEnumerable<SmallOrganisationViewModel>>()
                .Passing(o => o.Count() == 1 
                && o.First().Id == organisation.Id 
                && o.First().BannerUrl == organisation.BannerUrl
                && o.First().Name == organisation.Name);
        }

        [TestMethod]
        public void GetUserWithoutOrganisationOrganisationsTest()
        {
            _userControllerWithAuthenticatedUserWithoutOrganisation
                .Calling(c => c.GetUserOrganisations())
                .ShouldReturn()
                .Ok()
                .WithResponseModelOfType<IEnumerable<SmallOrganisationViewModel>>()
                .Passing(o => o.FirstOrDefault() == null);
        }

        [TestMethod]
        public void GetUserPlaylistsTest()
        {
            _userControllerWithAuthenticatedUserWithOrganisation
                .Calling(c => c.GetUserPlaylists())
                .ShouldReturn()
                .Ok();
        }

        [TestMethod]
        public void GetUserTest()
        {
            _userControllerWithAuthenticatedUserWithOrganisation
                 .Calling(c => c.GetUser (_userWithOrganisation.Email))
                 .ShouldReturn()
                 .Ok()
                 .WithResponseModelOfType<User>()
                 .Passing(p => p.Id == _userWithOrganisation.Id
                 && p.Email == _userWithOrganisation.Email
                 && p.FirstName == _userWithOrganisation.FirstName
                 && p.ImageUrl == _userWithOrganisation.ImageUrl
                 && p.LastName == _userWithOrganisation.LastName
                 && p.Nickname == _userWithOrganisation.Nickname);
        }

        [TestMethod]
        public void GetNullUserTest()
        {
            _userControllerWithAuthenticatedUserWithOrganisation
                .Calling(c => c.GetUser(""))
                .ShouldReturn()
                .NotFound();
        }

        [TestCleanup]
        public void Cleanup() {
            playlistManager.DeletePlaylist(playlist.Id);
            organisationManager.DeleteOrganisation(organisation.Id);
            userManager.DeleteUser(_userWithOrganisation.Email);
            userManager.DeleteUser(_userWithoutOrganisation.Email);
        }
    }
}
