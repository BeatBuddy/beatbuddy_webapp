using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BB.BL.Domain.Playlists;

namespace BB.BL.Domain.Organisations
{
    public class Organisation
    {
        [Key]
        public long Id { get; set; }
        [Index(IsUnique = true), MaxLength(100)]
        public string Name { get; set; }
        public string BannerUrl { get; set; }
        public string ColorScheme { get; set; }
        public virtual ICollection<DashboardBlock> DashboardBlocks { get; set; }
        public virtual ICollection<Playlist> Playlists { get; set; }
    }
}
