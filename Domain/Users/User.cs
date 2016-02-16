using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BB.BL.Domain.Organisations;
using BB.BL.Domain.Playlists;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BB.BL.Domain.Users
{
    public class User
    {
        [Key]
        public long Id { get; set; }
        [MaxLength(100)]
        [Index(IsUnique = true)]
        public string Email { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Nickname { get; set; }
        public string ImageUrl { get; set; }
        public Dictionary<Organisation, Role> Roles { get; set; }
       
    }

}
