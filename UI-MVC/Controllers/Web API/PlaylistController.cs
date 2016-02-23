using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Description;
using BB.BL;
using BB.BL.Domain;
using BB.BL.Domain.Playlists;

namespace BB.UI.Web.MVC.Controllers.Web_API
{
    [Authorize]
    [RoutePrefix("api/Playlist")]
    public class PlaylistController : ApiController
    {
        private readonly PlaylistManager playlistManager;
        private readonly UserManager userManager;

        public PlaylistController()
        {
            playlistManager = new PlaylistManager(ContextEnum.BeatBuddy);
            userManager = new UserManager(ContextEnum.BeatBuddy);
        }

        public PlaylistController(ContextEnum contextEnum)
        {
            playlistManager = new PlaylistManager(contextEnum);
            userManager = new UserManager(contextEnum);
        }

        [HttpPost]
        [Route("createPlaylist")]
        [ResponseType(typeof(Playlist))]
        public HttpResponseMessage createPlaylist(FormDataCollection formData)
        {
            var userIdentity = RequestContext.Principal.Identity as ClaimsIdentity;
            if (userIdentity == null) return new HttpResponseMessage(HttpStatusCode.Forbidden);

            var email = userIdentity.Claims.First(c => c.Type == "sub").Value;
            if(email == null) return new HttpResponseMessage(HttpStatusCode.Forbidden);

            var user = userManager.ReadUser(email);
            if (user == null) return new HttpResponseMessage(HttpStatusCode.Forbidden);

            var playlist = playlistManager.CreatePlaylistForUser(formData["name"], formData["description"], formData["key"], 1, false, null, null, user);
            if (playlist != null)
                return Request.CreateResponse(HttpStatusCode.OK, playlist);
            
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

    }
}
