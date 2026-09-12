using NXAI.Infra.EventBus;
using NXAI.Infra.EventBus.Tracker;
using NXAI.Infra.IdGenerater.Yitter;
using NXAI.Infra.Repository;
using NXAI.Shared.Repository.EfCoreEntities;

namespace NXAI.Shared.Application.Services.Trackers;

/// <summary>
/// 基于数据库 <see cref="EventTracker"/> 表的 CAP 消息幂等追踪。
/// </summary>
public class DbMessageTrackerService(IEfRepository<EventTracker> trackerRepo) : IMessageTracker
{
    public IEfRepository<EventTracker> _trackerRepo = trackerRepo;

    public TrackerKind Kind => TrackerKind.Db;

    public async Task<bool> HasProcessedAsync(long eventId, string trackerName)
    {
        return await _trackerRepo.AnyAsync(x => x.EventId == eventId && x.TrackerName == trackerName, true);
    }

    public async Task MarkAsProcessedAsync(long eventId, string trackerName)
    {
        await _trackerRepo.InsertAsync(new EventTracker
        {
            Id = IdGenerater.GetNextId(),
            EventId = eventId,
            TrackerName = trackerName
        });
    }

    public async Task<bool> HasProcessedAsync(BaseEvent eventModel)
    {
        return await HasProcessedAsync(eventModel.Id, eventModel.EventTarget);
    }

    public async Task MarkAsProcessedAsync(BaseEvent eventModel)
    {
        await MarkAsProcessedAsync(eventModel.Id, eventModel.EventTarget);
    }
}
