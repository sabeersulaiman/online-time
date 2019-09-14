using System;

namespace api.Models
{
    public class UserRole
    {
        public long UserRoleId { get; set; }
        public long UserId { get; set; }
        public string RoleName { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateModified { get; set; }

        public bool IsValid()
        {
            if (RoleName == nameof(UserRoles.Admin) || RoleName == nameof(UserRoles.User))
            {
                return true;
            }

            return false;
        }
    }

    public enum UserRoles
    {
        User,
        Admin
    }

    public enum TimeTrackClaims {
        UserId,
        Roles
    }
}