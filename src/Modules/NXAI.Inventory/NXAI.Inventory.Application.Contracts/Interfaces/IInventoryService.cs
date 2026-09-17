using NXAI.Inventory.Application.Contracts.Dtos;
using NXAI.Shared.Application.Contracts.Dtos;
using NXAI.Shared.Application.Contracts.ResultModels;

namespace NXAI.Inventory.Application.Contracts.Interfaces;

/// <summary>会员耗材实例。投放成功后余量 -1，缺料不扣。</summary>
public interface IInventoryService
{
    /// <summary>创建耗材。整包禁止填内部克数再拆。</summary>
    Task<ServiceResult<IdDto>> CreateAsync(long memberId, ConsumableCreationDto input);

    /// <summary>当前会员耗材。</summary>
    Task<List<ConsumableDto>> GetByMemberAsync(long memberId);

    /// <summary>放到仓位：先调 Device 绑仓，再写投影。</summary>
    Task<ServiceResult> PlaceAsync(long memberId, long id, ConsumablePlaceDto input);

    /// <summary>PACKAGE 余量是否够。Cooking 投放前只读。</summary>
    Task<bool> HasEnoughAsync(long memberId, string typeCode, decimal qty);

    /// <summary>投放成功扣减。失败任务不得调用。</summary>
    Task DecrementAsync(IReadOnlyList<long> consumableIds);
}
