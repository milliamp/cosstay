using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosStay.Model
{

    public class AccomodationVenuePermission
    {
        public int AccomodationVenuePermissionId { get; set; }
        public virtual AccomodationVenue Venue { get; set; }
        public AccomodationVenueRole Role { get; set; }
    }

    public enum AccomodationVenueRole
    {
        Owner,
        Administrator,
        View
    }

    public class AdministrationPermission
    {
        public int AdministrationPermissionId { get; set; }
        public string SecurableEntity { get; set; }
    }

    public enum PermissionLevel
    {
        Deny,
        Inherit,
        Moderate,
        Edit,
        FullControl
    }

    public class RolePermission
    {
        public int RolePermissionId { get; set; }
        public virtual AdministrationPermission AdministrationPermission { get; set; }
        public PermissionLevel PermissionLevel { get; set; }
    }

    public class Role
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
        public virtual List<RolePermission> Permissions { get; set; }
        public virtual List<User> Users { get; set; }
    }
}
