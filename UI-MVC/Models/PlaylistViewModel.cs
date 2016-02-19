using BB.BL.Domain.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BB.UI.Web.MVC.Models
{
    public class PlaylistViewModel
    {
        [Required]
        [MaxLength(100)]
        [Display(Name = "Playlist Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Maximum votes per user")]
        public int MaximumVotesPerUser { get; set; }
          
        [Display(Name = "Image Url")]
        public string ImageUrl { get; set; }

        [Display(Name = "Playlist master email")]
        [Remote("IsNameAvailable", "Playlists", ErrorMessage = "Email not found")]
        public string PlaylistMaster { get; set; }

        [Display(Name = "Organisation Name")]
        [Remote("IsOrganisationAvailable", "Playlists", ErrorMessage = "Organisation not found")]
        public string Organisation { get; set; }
        
    }
}