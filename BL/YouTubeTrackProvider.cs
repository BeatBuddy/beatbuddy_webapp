using System;
using System.Collections.Generic;
using System.Linq;
using BB.BL.Domain.Playlists;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

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

        public List<Track> Search(string q)
        {
            var result = new List<Track>();
            
            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = q;
            searchListRequest.MaxResults = 3;

            var queryResult = searchListRequest.Execute();

            foreach (var video in queryResult.Items.Where(v => v.Id.Kind == "youtube#video" && v.Snippet.Title.Contains(" - ")))
            {
                var track = new Track
                {
                    Title = video.Snippet.Title.Split(new[] {" - "}, StringSplitOptions.None)[1],
                    Artist = video.Snippet.Title.Split(new[] {" - "}, StringSplitOptions.None)[0]
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

        public Track LookupTrack(string TrackId)
        {
            var lookupRequest = youtubeService.Videos.List("snippet");
            lookupRequest.Id = TrackId;

            var queryResult = lookupRequest.Execute();
            foreach (var video in queryResult.Items)
            {
                var track = new Track
                {
                    Title = video.Snippet.Title.Split(new[] {" - "}, StringSplitOptions.None)[1],
                    Artist = video.Snippet.Title.Split(new[] {" - "}, StringSplitOptions.None)[0]
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

            return null;
        }
    }
}