using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Audit
{
    public interface IModified
    {
        public DateTime ModifyTime { get; set; }
    }
}
