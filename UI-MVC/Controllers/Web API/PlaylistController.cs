﻿using System;
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
using VideoLibrary;
using BB.DAL.EFUser;
using BB.DAL;
using BB.DAL.EFPlaylist;
using BB.BL.Domain.Users;
using BB.UI.Web.MVC.Models;
using System.Text.RegularExpressions;
using Microsoft.AspNet.SignalR;

namespace BB.UI.Web.MVC.Controllers.Web_API
{
    [System.Web.Http.Authorize]
    [RoutePrefix("api/playlist")]
    public class PlaylistController : ApiController
    {
        private readonly IPlaylistManager playlistManager;
        private readonly IUserManager userManager;
        private readonly IOrganisationManager organisationManager;
        private readonly ITrackProvider trackProvider;
        private readonly IAlbumArtProvider albumArtProvider;

        public PlaylistController()
        {
            playlistManager = new PlaylistManager(new PlaylistRepository(new EFDbContext(ContextEnum.BeatBuddy)), new UserRepository(new EFDbContext(ContextEnum.BeatBuddy)));
            userManager = new UserManager(new UserRepository(new EFDbContext(ContextEnum.BeatBuddy)));
            trackProvider = new YouTubeTrackProvider();
            albumArtProvider = new BingAlbumArtProvider();
        }

        public PlaylistController(IPlaylistManager playlistManager, IUserManager userManager, IOrganisationManager organisationManager, ITrackProvider iTrackProvider, IAlbumArtProvider albumArtProvider)
        {
            this.playlistManager = playlistManager;
            this.userManager = userManager;
            this.organisationManager = organisationManager;
            this.trackProvider = iTrackProvider;
            this.albumArtProvider = albumArtProvider;
        }

        public PlaylistController(IPlaylistManager playlistManager)
        {
            this.playlistManager = playlistManager;
        }

        public PlaylistController(IPlaylistManager playlistManager, IUserManager userManager)
        {
            this.playlistManager = playlistManager;
            this.userManager = userManager;
        }

        public PlaylistController(IPlaylistManager playlistManager, IUserManager userManager, ITrackProvider trackProvider, IAlbumArtProvider albumArtProvider)
        {
            this.playlistManager = playlistManager;
            this.userManager = userManager;
            this.trackProvider = trackProvider;
            this.albumArtProvider = albumArtProvider;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{id}")]
        [ResponseType(typeof(Playlist))]
        public IHttpActionResult getPlaylist(long id)
        {
            var playlist = playlistManager.ReadPlaylist(id);
            if (playlist == null) return NotFound();

            return Ok(playlist);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("lookup/{key}")]
        [ResponseType(typeof(Playlist))]
        public IHttpActionResult getPlaylistByKey(string key)
        {
            var playlist = playlistManager.ReadPlaylistByKey(key);
            if (playlist == null) return NotFound();

            return Ok(playlist);
        }

        [HttpGet]
        [Route("{id}/live")]
        [ResponseType(typeof(LivePlaylistViewModel))]
        public IHttpActionResult getLivePlaylist(long id)
        {
            var userIdentity = RequestContext.Principal.Identity as ClaimsIdentity;
            if (userIdentity == null) return NotFound();

            var email = userIdentity.Claims.First(c => c.Type == "sub").Value;
            if (email == null) return NotFound();

            var playlist = playlistManager.ReadPlaylist(id);
            if (playlist == null) return NotFound();

            List<LivePlaylistTrackViewModel> livePlaylistTracks = new List<LivePlaylistTrackViewModel>();
            foreach (var playlistTrack in playlist.PlaylistTracks.Where(t => t.PlayedAt == null))
            {
                var score = playlistTrack.Votes.FirstOrDefault(v => v.User.Email == email)?.Score;
                livePlaylistTracks.Add(new LivePlaylistTrackViewModel()
                {
                    Id = playlistTrack.Id,
                    Track = playlistTrack.Track,
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

            return Ok(livePlaylist);
        }

        [HttpGet]
        [Route("{id}/history")]
        [ResponseType(typeof(IEnumerable<PlaylistTrack>))]
        public IHttpActionResult getHistory(long id)
        {
            var playlist = playlistManager.ReadPlaylist(id);
            if (playlist == null) return NotFound();

            return Ok(playlist.PlaylistTracks.Where(p => p.PlayedAt != null));
        }

        [HttpPost]
        [Route("createPlaylist")]
        [ResponseType(typeof(Playlist))]
        public IHttpActionResult createPlaylist(FormDataCollection formData)
        {
            var userIdentity = RequestContext.Principal.Identity as ClaimsIdentity;
            if (userIdentity == null) return NotFound();

            var email = userIdentity.Claims.First(c => c.Type == "sub").Value;
            if (email == null) return NotFound();

            var user = userManager.ReadUser(email);
            if (user == null) return NotFound();

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

                    if (string.IsNullOrEmpty(extension)) return BadRequest("The supplied image is not a valid image");

                    imagePath = FileHelper.NextAvailableFilename(Path.Combine(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["PlaylistImgPath"]), "playlist" + extension));
                    bitmap.Save(imagePath);

                    imagePath = Path.GetFileName(imagePath);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            if (formData["organisationId"] != null)
            {
                var playlist = playlistManager.CreatePlaylistForOrganisation(formData["name"], formData["description"], formData["key"], 1, false, imagePath, user, long.Parse(formData["organisationId"]));
                if (playlist != null)
                    return Ok(playlist);
            }
            else
            {
                var playlist = playlistManager.CreatePlaylistForUser(formData["name"], formData["description"], formData["key"], 1, false, imagePath, user);
                if (playlist != null)
                    return Ok(playlist);
            }

            return BadRequest();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("searchTrack")]
        [ResponseType(typeof(List<Track>))]
        public IHttpActionResult searchTrack(string query)
        {
            if (query == null) return NotFound();
            var youtubeProvider = new YouTubeTrackProvider();
            var searchResult = youtubeProvider.Search(query);

            return Ok(searchResult);
        }

        [HttpGet]
        [Route("{playlistId}/nextTrack")]
        public IHttpActionResult getNextTrack(long playlistId)
        {
            var userIdentity = RequestContext.Principal.Identity as ClaimsIdentity;
            if (userIdentity == null) return InternalServerError();

            var email = userIdentity.Claims.First(c => c.Type == "sub").Value;
            if (email == null) return InternalServerError();

            var user = userManager.ReadUser(email);
            if (user == null) return InternalServerError();

            if (AssignPlaylistMaster(playlistId, user.Id))
            {

            var playlistTracks = playlistManager.ReadPlaylist(playlistId).PlaylistTracks
                 .Where(t => t.PlayedAt == null);

            if (!playlistTracks.Any()) return NotFound();

            var sortedByVotesPlaylistTracks = playlistTracks.ToList();
            sortedByVotesPlaylistTracks.Sort((t1, t2) => t2.Votes.Sum(v => v.Score) - t1.Votes.Sum(v => v.Score));
                

                var originalPlayListTrack = sortedByVotesPlaylistTracks.First(t => t.PlayedAt == null);
                var newTrack = GetTrackWithFreshYoutubeUrl(originalPlayListTrack.Track);
                
                var success = playlistManager.MarkTrackAsPlayed(originalPlayListTrack.Id, playlistId);
                if (success)
                {
                    return Ok(newTrack);
                }
                else {
                    return InternalServerError(new Exception("Could not mark track as played."));
                }
            }
            else {
                return BadRequest("Playlistmaster already set or you are not the organiser or co-organiser.");
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("getYoutubePlaybackUrl/{id}")]
        [ResponseType(typeof(LivePlaylistViewModel))]
        public IHttpActionResult getYoutubePlaybackTrack(long id)
        {
            var track = playlistManager.ReadTrack(id);
            var playbackTrack = GetTrackWithFreshYoutubeUrl(track);

            return Ok(playbackTrack);
        }

        private Track GetTrackWithFreshYoutubeUrl(Track originalTrack)
        {
            IEnumerable<Track> tracks = playlistManager.ReadTracks().Where(t => t.Artist == originalTrack.Artist && t.Title == originalTrack.Title);

            DateTime timeTrackRequested = new DateTime(1970, 1, 1, 0, 0, 0);
            Track trackWithLatestYoutubeUrl = null;
            string youTubeVideo = null;

            foreach (Track track in tracks)
            {
                if (track.Url != null)
                {
                    Match match = Regex.Match(track.Url, @"lmt(\=[^&]*)?(?=&|$)|^lmt(\=[^&]*)?(&|$)");
                    if (match.Success)
                    {
                        string timestamp;
                        timestamp = match.Groups[1].Value;
                        timestamp.Replace("lmt=", "");
                        DateTime datetime = new DateTime(1970, 1, 1, 0, 0, 0).AddMilliseconds(Convert.ToDouble(timestamp));
                        if (datetime > timeTrackRequested)
                        {
                            timeTrackRequested = datetime;
                            trackWithLatestYoutubeUrl = track;
                        }
                    }

                }

            }
            if (timeTrackRequested.AddHours(6) >= DateTime.UtcNow || trackWithLatestYoutubeUrl == null)
            {
                var youTube = YouTube.Default; // starting point for YouTube actions
                youTubeVideo = youTube.GetVideo(originalTrack.TrackSource.Url).Uri; // gets a Video object with info about the video
            }
            else {
                youTubeVideo = trackWithLatestYoutubeUrl.Url;
            }

            Track newTrack = new Track()
            {
                Id = originalTrack.Id,
                Artist = originalTrack.Artist,
                CoverArtUrl = originalTrack.CoverArtUrl,
                Duration = originalTrack.Duration,
                TrackSource = new TrackSource()
                {
                    Id = originalTrack.TrackSource.Id,
                    SourceType = originalTrack.TrackSource.SourceType,
                    TrackId = originalTrack.TrackSource.TrackId,
                    Url = originalTrack.TrackSource.Url
                },
                Title = originalTrack.Title,
                Url = youTubeVideo
            };

            return newTrack;
        }

        public bool AssignPlaylistMaster(long playlistId, long userId)
        {
            if (playlistManager.CheckIfUserCreatedPlaylist(playlistId, userId))
        {
                var playlist = playlistManager.ReadPlaylist(playlistId);
                playlist.PlaylistMasterId = userId;
                playlist = playlistManager.UpdatePlaylist(playlist);
                return true;
            }
            var organisation = organisationManager.ReadOrganisationForPlaylist(playlistId);
            if (organisation != null)
            {
                if (userManager.ReadOrganiserFromOrganisation(organisation).Id == userId ||
                    userManager.ReadCoOrganiserFromOrganisation(organisation).FirstOrDefault(u => u.Id == userId) != null)
                {
                    var playlist = playlistManager.ReadPlaylist(playlistId);
                    playlist.PlaylistMasterId = userId;
                    playlist = playlistManager.UpdatePlaylist(playlist);
                    return true;
                }
            }
            return false;
        }


        [HttpPost]
        [Route("{playlistId}/addTrack")]
        [ResponseType(typeof(Track))]
        public IHttpActionResult AddTrack(long playlistId, string trackId)
        {
            var track = trackProvider.LookupTrack(trackId);
            if (track == null) return NotFound();

            var albumArtUrl = albumArtProvider.Find(track.Artist + " " + track.Title);
            track.CoverArtUrl = albumArtUrl;

            track = playlistManager.AddTrackToPlaylist(
                playlistId,
                track
            );

            if (track == null) return NotFound();

            return Ok(track);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("recommendations")]
        [ResponseType(typeof(IEnumerable<Track>))]
        public IHttpActionResult GetRecommendations(int count)
        {
            return Ok(playlistManager.ReadPlaylists()
                .Reverse()
                .Take(count)
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

        [HttpPost]
        [Route("{id}/track/{trackId}/upvote")]
        public IHttpActionResult Upvote(long id, long trackId)
        {
            var userIdentity = RequestContext.Principal.Identity as ClaimsIdentity;
            var user = GetUser(userIdentity);
            var createVote = playlistManager.CreateVote(1, user.Id, trackId);

            var playlistTrack = playlistManager.ReadPlaylistTrack(trackId);
            var viewmodel = new LivePlaylistTrackViewModel
            {
                Id = playlistTrack.Id,
                Score = playlistTrack.Votes.Sum(v => v.Score),
                Track = playlistTrack.Track
            };

            var context = GlobalHost.ConnectionManager.GetHubContext<PlaylistHub>();
            context.Clients.Group(id.ToString()).scoreUpdated(trackId, viewmodel);

            return Ok(createVote);
        }

        [HttpPost]
        [Route("{id}/track/{trackId}/downvote")]
        public IHttpActionResult Downvote(long id, long trackId)
        {
            var userIdentity = RequestContext.Principal.Identity as ClaimsIdentity;
            var user = GetUser(userIdentity);
            var createVote = playlistManager.CreateVote(-1, user.Id, trackId);

            var playlistTrack = playlistManager.ReadPlaylistTrack(trackId);
            var viewmodel = new LivePlaylistTrackViewModel
            {
                Id = playlistTrack.Id,
                Score = playlistTrack.Votes.Sum(v => v.Score),
                Track = playlistTrack.Track
            };

            var context = GlobalHost.ConnectionManager.GetHubContext<PlaylistHub>();
            context.Clients.Group(id.ToString()).scoreUpdated(trackId, viewmodel);

            return Ok(createVote);
        }

        private User GetUser(ClaimsIdentity claimsIdentity)
        {
            if (claimsIdentity == null) return null;

            var email = claimsIdentity.Claims.First(c => c.Type == "sub").Value;
            if (email == null) return null;

            var user = userManager.ReadUser(email);

            return user;
        }
    }
}
