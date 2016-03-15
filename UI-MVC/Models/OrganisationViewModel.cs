using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BB.BL.Domain.Organisations;
using BB.BL.Domain.Playlists;
using BB.BL.Domain.Users;
using PagedList;

namespace BB.UI.Web.MVC.Models
{
    public class OrganisationViewModel
    {
        public long Id { get; set; }
        [Required, MaxLength(100)]
        [Index(IsUnique = true)]
        [Remote("IsNameAvailable", "Organisations", ErrorMessage = "Name is already in use")]
        public string Name { get; set; }
        [DisplayName("Banner Image: ")]
        public string BannerUrl { get; set; }
        [DisplayName("Avatar Image: ")]
        public string ImageUrl { get; set; }
        [Required, DisplayName("Accent color: ")]
        public string ColorScheme { get; set; }
    }

    public class SmallOrganisationViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string BannerUrl { get; set; }
        public string ColorScheme { get; set; }
    }

    public class OrganisationViewWithPlaylist
    {
        public long Id { get; set; }
        [Required, MaxLength(100)]
        [Index(IsUnique = true)]
        [Remote("IsNameAvailable", "Organisations", ErrorMessage = "Name is already in use")]
        public string Name { get; set; }
        [DisplayName("Banner Image: ")]
        public string BannerUrl { get; set; }
        [DisplayName("Avatar Image: ")]
        public string ImageUrl { get; set; }
        [Required, DisplayName("Accent color: ")]
        public string ColorScheme { get; set; }
        public IPagedList<Playlist> Playlists { get; set; }
        public User Organiser { get; set; }
        public IEnumerable<User> CoOrganiser { get; set; }
    }
}