using NXAI.Cooking.Application.Contracts.Dtos;
using NXAI.Shared.Application.Contracts.ResultModels;

namespace NXAI.Cooking.Application.Contracts.Interfaces;

/// <summary>制作任务。唯一允许对 Device 说「去投」的模块。</summary>
public interface ICookingService
{
    /// <summary>校验并启动。失败为 Rejected，零投放指令。</summary>
    Task<ServiceResult<CookingTaskDto>> CreateAsync(long memberId, CookingTaskCreationDto input);

    /// <summary>任务详情。</summary>
    Task<CookingTaskDto?> GetAsync(long memberId, long id);

    /// <summary>取消，下发同 epoch 的 STOP。</summary>
    Task<ServiceResult> CancelAsync(long memberId, long id);

    /// <summary>处理设备 ACK。影子水温不能作为投放扳机。</summary>
    Task HandleAckAsync(long commandId, string result, int? epoch);

    /// <summary>超时看门狗：120s 无终态则 Failed 并 STOP。</summary>
    Task FailTimedOutAsync();
}
