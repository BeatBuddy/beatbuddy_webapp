using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BB.BL.Domain.Users
{
    public enum Role
    {
        Organiser,
        // ReSharper disable once InconsistentNaming
        Co_Organiser,
        PlaylistMaster,
        RegisteredUser,
        Guest
    }
}
