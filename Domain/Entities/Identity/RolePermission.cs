using Domain.Base;

namespace Domain.Entities.Identity
{
    public class RolePermission : Entity<int>
    {
        public int RoleId { get; set; }

        public int PermissionId { get; set; }

        public virtual Role Role { get; set; }

        public virtual Permission Permission { get; set; }
    }
}
