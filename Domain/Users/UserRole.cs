using BB.BL.Domain.Organisations;
using System.ComponentModel.DataAnnotations;

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
