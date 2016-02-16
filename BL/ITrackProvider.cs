using System.Collections.Generic;
using BB.BL.Domain.Playlists;

namespace BB.BL
{
    public interface ITrackProvider
    {
        List<Track> Search(string query);
        Track LookupTrack(string TrackId);
    }
}
