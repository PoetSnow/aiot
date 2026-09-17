using NXAI.Infra.Repository;

namespace NXAI.Asset.Repository.Entities;

/// <summary>出库单行，表 ast_stock_out_line。一行一个 SN。</summary>
public class StockOutLine : EfEntity
{
    /// <summary>SN 最大长度。</summary>
    public const int Sn_MaxLength = 64;

    /// <summary>所属出库单。</summary>
    public long StockOutId { get; set; }

    /// <summary>待出库 SN。</summary>
    public string Sn { get; set; } = string.Empty;
}
