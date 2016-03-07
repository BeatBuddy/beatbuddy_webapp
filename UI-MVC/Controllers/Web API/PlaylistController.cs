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
using System.Web.Http.Description;
using BB.BL;
using BB.BL.Domain;
using BB.BL.Domain.Playlists;
using BB.UI.Web.MVC.Controllers.Utils;
using YoutubeExtractor;
using VideoLibrary;
using BB.DAL.EFUser;
using BB.DAL;
using BB.DAL.EFPlaylist;
using BB.UI.Web.MVC.Models;

namespace BB.UI.Web.MVC.Controllers.Web_API
{
    [Authorize]
    [RoutePrefix("api/playlist")]
    public class PlaylistController : ApiController
    {
        private readonly IPlaylistManager playlistManager;
        private readonly IUserManager userManager;
        private readonly ITrackProvider trackProvider;
        private readonly IAlbumArtProvider albumArtProvider;

        public PlaylistController()
        {
            this.playlistManager = new PlaylistManager(new PlaylistRepository(new EFDbContext(ContextEnum.BeatBuddy)));
            this.userManager = new UserManager(new UserRepository(new EFDbContext(ContextEnum.BeatBuddy)));
            this.trackProvider = new YouTubeTrackProvider();
            this.albumArtProvider = new BingAlbumArtProvider();
        }

        public PlaylistController(IPlaylistManager playlistManager, IUserManager userManager, ITrackProvider iTrackProvider, IAlbumArtProvider albumArtProvider)
        {
            this.playlistManager = playlistManager;
            this.userManager = userManager;
            this.trackProvider = iTrackProvider;
            this.albumArtProvider = albumArtProvider;
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("{id}")]
        [ResponseType(typeof(Playlist))]
        public HttpResponseMessage getPlaylist(long id)
        {
            var playlist = playlistManager.ReadPlaylist(id);
            if(playlist == null) return new HttpResponseMessage(HttpStatusCode.NotFound);

            return Request.CreateResponse(HttpStatusCode.OK, playlist);
        }
        
        [HttpGet]
        [Route("{id}/live")]
        [ResponseType(typeof(LivePlaylistViewModel))]
        public HttpResponseMessage getLivePlaylist(long id)
        {
            var userIdentity = RequestContext.Principal.Identity as ClaimsIdentity;
            if (userIdentity == null) return new HttpResponseMessage(HttpStatusCode.Forbidden);

            var email = userIdentity.Claims.First(c => c.Type == "sub").Value;
            if (email == null) return new HttpResponseMessage(HttpStatusCode.Forbidden);

            var playlist = playlistManager.ReadPlaylist(id);
            if (playlist == null) return new HttpResponseMessage(HttpStatusCode.NotFound);

            List<LivePlaylistTrackViewModel> livePlaylistTracks = new List<LivePlaylistTrackViewModel>();
            foreach (var playlistTrack in playlist.PlaylistTracks.Where(t => t.PlayedAt == null))
            {
                var score = playlistTrack.Votes.FirstOrDefault(v => v.User.Email == email)?.Score;
                livePlaylistTracks.Add(new LivePlaylistTrackViewModel()
                {
                    Track = playlistTrack.Track,
                    AlreadyPlayed = false,
                    Score = playlistTrack.Votes.Sum(v => v.Score),
                    PersonalScore = score ?? 0
                });
            }
            livePlaylistTracks.Sort((t1, t2) => t2.Score - t1.Score);

            var livePlaylist = new LivePlaylistViewModel()
            {
                Id = playlist.Id,
                PlaylistTracks = livePlaylistTracks,
                Active = playlist.Active,
                ChatComments = playlist.ChatComments,
                Comments = playlist.Comments,
                CreatedById = playlist.CreatedById,
                Description = playlist.Description,
                ImageUrl = playlist.ImageUrl,
                Key = playlist.Key,
                MaximumVotesPerUser = playlist.MaximumVotesPerUser,
                PlaylistMasterId = playlist.PlaylistMasterId,
                Name = playlist.Name
            };

            return Request.CreateResponse(HttpStatusCode.OK, livePlaylist);
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

            string imagePath = "";
            if (formData["image"] != null && formData["image"].Length > 0)
            {
                try
                {
                    var bitmap = ImageDecoder.DecodeBase64String(formData["image"]);

                    string extension = string.Empty;
                    if (bitmap.RawFormat.Equals(ImageFormat.Jpeg)) extension = ".jpg";
                    if (bitmap.RawFormat.Equals(ImageFormat.Png)) extension = ".png";
                    if (bitmap.RawFormat.Equals(ImageFormat.Bmp)) extension = ".bmp";
                    if (bitmap.RawFormat.Equals(ImageFormat.Gif)) extension = ".gif";

                    if (string.IsNullOrEmpty(extension)) return Request.CreateResponse(HttpStatusCode.BadRequest, "The supplied image is not a valid image");

                    imagePath = FileHelper.NextAvailableFilename(Path.Combine(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["PlaylistImgPath"]), "playlist" + extension));
                    bitmap.Save(imagePath);

                    imagePath = Path.GetFileName(imagePath);
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
                }
            }

            var playlist = playlistManager.CreatePlaylistForUser(formData["name"], formData["description"], formData["key"], 1, false, imagePath, user);
            if (playlist != null)
                return Request.CreateResponse(HttpStatusCode.OK, playlist);
            
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("searchTrack")]
        [ResponseType(typeof(List<Track>))]
        public HttpResponseMessage searchTrack(string query)
        {
            if (query == null) return new HttpResponseMessage(HttpStatusCode.NotFound);
            var youtubeProvider = new YouTubeTrackProvider();
            var searchResult = youtubeProvider.Search(query);

            return Request.CreateResponse(HttpStatusCode.OK, searchResult);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("{playlistId}/nextTrack")]
        public IHttpActionResult getNextTrack(long playlistId)
        {
            var playlistTracks = playlistManager.ReadPlaylist(playlistId).PlaylistTracks
                 .Where(t => t.PlayedAt == null);

            if (!playlistTracks.Any()) return NotFound();

            var playListTrack = playlistTracks.First(t => t.PlayedAt == null);

            var youTube = YouTube.Default; // starting point for YouTube actions
            var video = youTube.GetVideo(playListTrack.Track.TrackSource.Url); // gets a Video object with info about the video
            playListTrack.Track.TrackSource.Url = video.Uri;



            return Ok(playListTrack.Track);
        }

        [HttpPost]
        [Route("{playlistId}/addTrack")]
        [ResponseType(typeof(Track))]
        public HttpResponseMessage AddTrack(long playlistId, string trackId)
        {
            var track = trackProvider.LookupTrack(trackId);
            if (track == null) return new HttpResponseMessage(HttpStatusCode.NotFound);

            var albumArtUrl = albumArtProvider.Find(track.Artist + " " + track.Title);
            track.CoverArtUrl = albumArtUrl;

            track = playlistManager.AddTrackToPlaylist(
                playlistId,
                track
            );

            if (track == null) return new HttpResponseMessage(HttpStatusCode.NotFound);

            return Request.CreateResponse(HttpStatusCode.OK, track);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("recommendations")]
        [ResponseType(typeof (IEnumerable<Track>))]
        public HttpResponseMessage GetRecommendations(int count)
        {
            return Request.CreateResponse(HttpStatusCode.OK, playlistManager.ReadPlaylists()
                .Reverse()
                .Take(3)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Key,
                    p.MaximumVotesPerUser,
                    p.Active,
                    p.ImageUrl,
                    p.CreatedById,
                    p.Description
                }));
        }
    }
}
