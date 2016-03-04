using System;
using System.Web.Http;
using System.Web.Mvc;
using BB.BL;
using BB.BL.Domain;
using BB.DAL;
using BB.DAL.EFOrganisation;
using BB.DAL.EFPlaylist;
using BB.DAL.EFUser;
using BB.UI.Web.MVC.Controllers;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Mvc;


namespace BB.UI.Web.MVC
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();
            // TODO: Register your types here
            // container.RegisterType<IProductRepository, ProductRepository>();
            EFDbContext efDbContext = new EFDbContext(ContextEnum.BeatBuddy);
            container.RegisterType<IOrganisationManager, OrganisationManager>(new InjectionConstructor(new OrganisationRepository(efDbContext)));
            container.RegisterType<IUserManager, UserManager>(new InjectionConstructor(new UserRepository(efDbContext)));
            container.RegisterType<IPlaylistManager, PlaylistManager>(new InjectionConstructor(new PlaylistRepository(efDbContext)));
            container.RegisterType<ITrackProvider, YouTubeTrackProvider>();
            container.RegisterType<IAlbumArtProvider, BingAlbumArtProvider>();
            container.RegisterType<AccountController>(new InjectionConstructor(new UserManager(new UserRepository(efDbContext))));
            container.RegisterType<ManageController>(new InjectionConstructor());
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
        }
    }
}
