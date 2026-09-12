using NXAI.Infra.Repository;

namespace NXAI.Shared.Domain.Entities;

public class AggregateRootWithBasicAuditInfo : AggregateRoot, IBasicAuditInfo
{
    public long CreateBy { get; set; }
    public DateTime CreateTime { get; set; }
}
