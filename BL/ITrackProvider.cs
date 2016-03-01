using System.Collections.Generic;
using BB.BL.Domain.Playlists;

namespace BB.BL
{
    public interface ITrackProvider
    {
        List<Track> Search(string query, long maxResults = 5);
        Track LookupTrack(string TrackId);
    }
}
