using NXAI.AfterSales.Application.Contracts.Dtos;
using NXAI.Shared.Application.Contracts.Dtos;
using NXAI.Shared.Application.Contracts.ResultModels;

namespace NXAI.AfterSales.Application.Contracts.Interfaces;

/// <summary>售后工单。维修开单后 SN 不能再绑定。</summary>
public interface ITicketService
{
    /// <summary>后台开单。</summary>
    Task<ServiceResult<IdDto>> CreateByStaffAsync(long staffId, TicketCreationDto input);

    /// <summary>会员开单。</summary>
    Task<ServiceResult<IdDto>> CreateByMemberAsync(long memberId, TicketCreationDto input);

    /// <summary>后台工单列表。</summary>
    Task<List<TicketDto>> GetConsoleListAsync();

    /// <summary>会员工单。</summary>
    Task<List<TicketDto>> GetByMemberAsync(long memberId);

    /// <summary>受理。</summary>
    Task<ServiceResult> AcceptAsync(long staffId, long id);

    /// <summary>结案。回库走 Asset，不直接改 ast_sn 表。</summary>
    Task<ServiceResult> CloseAsync(long staffId, long id, TicketCloseDto input);
}
