using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Identity
{
    public class RoleClaim
    {
        public Guid Id { get; set; }

        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }

        public int RoleId { get; set; }

        public virtual Role Role { get; set; }
    }
}
