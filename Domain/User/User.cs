using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BB.BL.Domain
{
    public class User
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string NickName { get; set; }
        public string AvatarUrl { get; set; }
        public Dictionary<Organisation, Role> Roles { get; set; }
        public Collection<Vote> Votes { get; set; }
        public Collection<Organisation> Organisations { get; set; }
        public Collection<Comment> Comments { get; set; } 
    }

}
