using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BB.BL.Domain.Organisations;
using BB.BL.Domain.Playlists;
using BB.BL.Domain.Users;

namespace BB.BL.Domain.Organisations
{
    public class Organisation
    {
        public long OrganisationId { get; set; }
        public string OranisationName { get; set; }
        public string BannerUrl { get; set; }
        public string ColorScheme { get; set; }
        public string OrganisationKey { get; set; }
        public Collection<DashboardBlock> DashboardBlocks { get; set; }
        public Collection<Playlist> Playlists { get; set; }
        public User Organisator { get; set; }
        public Collection<User> CoOrganisators { get; set; }
    }
}
