using NXAI.Infra.Repository;

namespace NXAI.AfterSales.Repository.Entities;

/// <summary>工单日志，表 afs_ticket_log。</summary>
public class TicketLog : EfEntity
{
    public const int Action_MaxLength = 16;
    public const int Remark_MaxLength = 256;

    /// <summary>工单 Id。</summary>
    public long TicketId { get; set; }

    /// <summary>动作。</summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>备注。</summary>
    public string Remark { get; set; } = string.Empty;

    /// <summary>操作员工 Id。会员开单为 0。</summary>
    public long OperatorStaffId { get; set; }
}
