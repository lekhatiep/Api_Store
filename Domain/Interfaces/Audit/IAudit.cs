using System;

namespace Domain.Interfaces.Audit
{
    public interface IAudit
    {
        public DateTime CreateTime { get; set; }

        public DateTime ModifyTime { get; set; }

        public bool IsDelete { get; set; }
    }
}
