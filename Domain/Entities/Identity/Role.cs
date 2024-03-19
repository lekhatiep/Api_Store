using Domain.Base;
using System.Collections.Generic;

namespace Domain.Entities.Identity
{
    public class Role :Entity<int>
    {
        private ICollection<UserRole> _userRoles;
        private ICollection<RolePermission> _rolePermissions;
        private ICollection<RoleClaim> _roleClaims;

        public string Name { get; set; }

        public ICollection<UserRole> UserRoles
        {
            get => _userRoles ??= new List<UserRole>();
            set => _userRoles = value;
        }

        public virtual ICollection<RolePermission> RolePermissions
        {
            get => _rolePermissions ??= new List<RolePermission>();
            set => _rolePermissions = value;
        }

        public virtual ICollection<RoleClaim> RoleClaims
        {
            get => _roleClaims ??= new List<RoleClaim>();
            set => _roleClaims = value;
        }
    }
}

