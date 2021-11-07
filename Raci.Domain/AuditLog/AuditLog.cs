using System;

namespace Raci.Domain.AuditLog
{
    public class AuditLog
    {
        public Guid AuditId { get; set; }

        public string? AuditData { get; set; }

        public string? EntityType { get; set; }

        public string? AuditUser { get; set; }

        public string? TablePk { get; set; }

        public DateTime AuditDate { get; set; }
    }
}
