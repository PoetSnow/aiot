using NXAI.Shared.Application.Contracts.Dtos;

namespace NXAI.Device.Application.Contracts.Dtos;

/// <summary>已绑定设备摘要。</summary>
/// <param name="Id">设备 Id。</param>
/// <param name="DeviceSn">SN。</param>
/// <param name="ModelCode">型号。</param>
/// <param name="MemberId">主人会员 Id。</param>
/// <param name="ActiveTaskId">在途任务。</param>
/// <param name="ActiveEpoch">任务世代。</param>
public record DeviceDto(long Id, string DeviceSn, string ModelCode, long MemberId, long? ActiveTaskId, int ActiveEpoch) : IDto;
