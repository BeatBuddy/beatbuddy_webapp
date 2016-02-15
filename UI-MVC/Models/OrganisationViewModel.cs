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

namespace BB.UI.Web.MVC.Models
{
    public class OrganisationViewModel
    {
        [Required, MaxLength(100)]
        [Index(IsUnique = true)]
        [Remote("IsNameAvailable", "Organisations", ErrorMessage = "Name is already in use")]
        public string Name { get; set; }
        [Required, DisplayName("Banner Image: ")]
        public string BannerUrl { get; set; }
        [Required, DisplayName("Avatar Image: ")]
        public string ImageUrl { get; set; }
        [Required, DisplayName("Accent color: ")]
        public string ColorScheme { get; set; }
    }
}