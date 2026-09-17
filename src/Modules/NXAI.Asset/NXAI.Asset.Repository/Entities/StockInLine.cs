using NXAI.Infra.Repository;

namespace NXAI.Asset.Repository.Entities;

/// <summary>入库单行，表 ast_stock_in_line。一行一个 SN。</summary>
public class StockInLine : EfEntity
{
    /// <summary>SN 最大长度。</summary>
    public const int Sn_MaxLength = 64;

    /// <summary>型号编码最大长度。</summary>
    public const int ModelCode_MaxLength = 32;

    /// <summary>所属入库单。</summary>
    public long StockInId { get; set; }

    /// <summary>待入库 SN。</summary>
    public string Sn { get; set; } = string.Empty;

    /// <summary>型号编码。</summary>
    public string ModelCode { get; set; } = string.Empty;
}
