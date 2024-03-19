using Domain.Base;
using System.Collections.Generic;

namespace Domain.Entities.Identity
{
    public class Permission : Entity<int>
    {
        private ICollection<RolePermission> _rolePermissions;

        public string  Name { get; set; }

        public virtual ICollection<RolePermission> RolePermissions
        {
            get => _rolePermissions ??= new List<RolePermission>();
            set => _rolePermissions = value;
        }
    }
}
