using NXAI.Infra.Repository;

namespace NXAI.AfterSales.Repository.Entities;

/// <summary>售后工单，表 afs_ticket。</summary>
public class Ticket : EfEntity
{
    public const int TicketNo_MaxLength = 32;
    public const int Sn_MaxLength = 64;
    public const int Symptom_MaxLength = 256;
    public const int CloseResult_MaxLength = 32;

    /// <summary>工单号。</summary>
    public string TicketNo { get; set; } = string.Empty;

    /// <summary>设备 SN。</summary>
    public string Sn { get; set; } = string.Empty;

    /// <summary>会员 Id。</summary>
    public long? MemberId { get; set; }

    /// <summary>设备会话 Id，解绑后可空。</summary>
    public long? DeviceId { get; set; }

    /// <summary>类型，见 <see cref="TicketType"/>。</summary>
    public int Type { get; set; } = TicketType.Repair;

    /// <summary>状态，见 <see cref="TicketStatus"/>。</summary>
    public int Status { get; set; } = TicketStatus.Opened;

    /// <summary>开单时冻结的质保结论。</summary>
    public bool WarrantyValid { get; set; }

    /// <summary>症状。</summary>
    public string Symptom { get; set; } = string.Empty;

    /// <summary>结案结果：return / scrap / replace。</summary>
    public string? CloseResult { get; set; }
}

/// <summary>工单类型。</summary>
public static class TicketType
{
    /// <summary>咨询。</summary>
    public const int Consult = 0;

    /// <summary>维修。</summary>
    public const int Repair = 1;

    /// <summary>退货。</summary>
    public const int Return = 2;

    /// <summary>换货。</summary>
    public const int Replace = 3;
}

/// <summary>工单状态。</summary>
public static class TicketStatus
{
    /// <summary>已开单。</summary>
    public const int Opened = 0;

    /// <summary>已受理。</summary>
    public const int Accepted = 1;

    /// <summary>已结案。</summary>
    public const int Closed = 2;
}
