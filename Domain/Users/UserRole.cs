using BB.BL.Domain.Organisations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BB.BL.Domain.Users
{
    public class UserRole
    {
        [Key]
        public long Id { get; set; }
        public User User { get; set; }
        public Role Role { get; set; }
        public Organisation Organisation { get; set; }
    }
}
