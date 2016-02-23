using System.ComponentModel.DataAnnotations;
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
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Key")]
        public string Key { get; set; }

        [Required]
        [Display(Name = "Maximum votes per user")]
        public int MaximumVotesPerUser { get; set; }
          
        [Display(Name = "Image Url")]
        public string ImageUrl { get; set; }

        [Display(Name = "Playlist master email")]
        [Remote("IsNameAvailable", "Playlist", ErrorMessage = "Email not found")]
        public string PlaylistMaster { get; set; }

        [Display(Name = "Organisation Name")]
        [Remote("IsOrganisationAvailable", "Playlist", ErrorMessage = "Organisation not found")]
        public string Organisation { get; set; }
        
    }
}