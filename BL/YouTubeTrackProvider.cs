using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using BB.BL.Domain.Playlists;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System.Collections.ObjectModel;
using Google.GData.Client;

namespace BB.BL
{
    public class YouTubeTrackProvider : ITrackProvider
    {
        private readonly YouTubeService youtubeService;

        public YouTubeTrackProvider()
        {
            youtubeService = new YouTubeService(new BaseClientService.Initializer
            {
                ApiKey = Constants.YouTubeApiKey(),
                ApplicationName = Constants.YoutubeApplicationName
            });
        }

        public List<Track> Search(string q, long maxResults = 5)
        {
            var result = new List<Track>();

            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = q;
            searchListRequest.MaxResults = 20;

            var queryResult = searchListRequest.Execute();

            foreach (var video in queryResult.Items.Where(v => v.Id.Kind == "youtube#video" && v.Snippet.Title.Contains(" - ")))
            {
                if (result.Count >= maxResults) continue;

                var track = new Track
                {
                    Title = video.Snippet.Title.Split(new[] { " - " }, StringSplitOptions.None)[1],
                    Artist = video.Snippet.Title.Split(new[] { " - " }, StringSplitOptions.None)[0],
                };

                Thumbnail[] thumbnails = { video.Snippet.Thumbnails.Maxres, video.Snippet.Thumbnails.High, video.Snippet.Thumbnails.Medium, video.Snippet.Thumbnails.Default__ };
                track.CoverArtUrl = thumbnails.First(t => t != null).Url;
                track.TrackSource = new TrackSource()
                {
                    SourceType = SourceType.YouTube,
                    Url = $"https://www.youtube.com/watch?v={video.Id.VideoId}",
                    TrackId = video.Id.VideoId
                };

                result.Add(track);
            }

            return result;
        }

        public List<Domain.Playlists.Playlist> SearchPlaylist(string q, long maxResult = 3)
        {
            var lijstje = new List<Domain.Playlists.Playlist>();
            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = q;
            searchListRequest.MaxResults = 20;
            searchListRequest.Type = "playlist";

            var queryResult = searchListRequest.Execute();

            foreach (var video in queryResult.Items.Where(v => v.Id.Kind == "youtube#playlist"))
            {
                var playlist = new Domain.Playlists.Playlist()
                {
                    Description = video.Id.PlaylistId,
                    Name = video.Snippet.Title + " - " + video.Snippet.ChannelTitle,

                };

                    Thumbnail[] thumbnails = { video.Snippet.Thumbnails.Maxres, video.Snippet.Thumbnails.High, video.Snippet.Thumbnails.Medium, video.Snippet.Thumbnails.Default__ };

      
                
                playlist.ImageUrl = thumbnails.First(t => t != null).Url;
                lijstje.Add(playlist);
            }
            return lijstje;
       }

        public Track LookupTrack(string TrackId)
        {
            var lookupRequest = youtubeService.Videos.List("contentDetails,snippet");
            lookupRequest.Id = TrackId;

            var queryResult = lookupRequest.Execute();
            foreach (var video in queryResult.Items)
            {
                var span = XmlConvert.ToTimeSpan(video.ContentDetails.Duration);
                try {
                    var track = new Track
                    {
                        Title = video.Snippet.Title.Split(new[] { " - " }, StringSplitOptions.None)[1],
                        Artist = video.Snippet.Title.Split(new[] { " - " }, StringSplitOptions.None)[0],
                        Duration = (int)span.TotalSeconds
                    };

                    Thumbnail[] thumbnails = { video.Snippet.Thumbnails.Maxres, video.Snippet.Thumbnails.High, video.Snippet.Thumbnails.Medium, video.Snippet.Thumbnails.Default__ };
                    track.CoverArtUrl = thumbnails.First(t => t != null).Url;
                    track.TrackSource = new TrackSource()
                    {
                        SourceType = SourceType.YouTube,
                        Url = $"https://www.youtube.com/watch?v={video.Id}",
                        TrackId = video.Id
                    };

                    return track;
                }
                catch 
                {
                    return null;
                }
            }

            return null;
        }

        public List<Track> LookUpPlaylist(string playlistId)
        {

            var videos = youtubeService.PlaylistItems.List("snippet");
            videos.PlaylistId = playlistId;
            videos.MaxResults = 50;

            var query = videos.Execute();
            List<Track> tracks = new List<Track>();

            foreach (var item in query.Items)
            {
                Track track = LookupTrack(item.Snippet.ResourceId.VideoId);
                if (track != null)
                {
                    tracks.Add(track);
                }
            }
            

            return tracks;
        }
    }
}