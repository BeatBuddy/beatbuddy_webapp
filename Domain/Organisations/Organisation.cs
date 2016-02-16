using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BB.BL.Domain.Organisations;
using BB.BL.Domain.Playlists;
using BB.BL.Domain.Users;

namespace BB.BL.Domain.Organisations
{
    public class Organisation
    {
        [Key]
        public long Id { get; set; }
        [Index(IsUnique = true), MaxLength(100)]
        public string Name { get; set; }
        public string BannerUrl { get; set; }
        public string ImageUrl { get; set; }
        public string ColorScheme { get; set; }
        public Collection<DashboardBlock> DashboardBlocks { get; set; }
        public Collection<Playlist> Playlists { get; set; }
        public Dictionary<User, Role> Users { get; set; }
    }
}
