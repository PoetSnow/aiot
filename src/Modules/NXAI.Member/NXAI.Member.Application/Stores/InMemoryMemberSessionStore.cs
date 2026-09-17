using System.Collections.Concurrent;

namespace NXAI.Member.Application.Stores;

/// <summary>会员会话 jti。进程内保存，未接 Redis 前刷新/作废只在本机有效。</summary>
public sealed class InMemoryMemberSessionStore
{
    private readonly ConcurrentDictionary<long, (string Jti, bool Active)> _sessions = new();

    public void Set(long memberId, string jti, bool active) => _sessions[memberId] = (jti, active);

    public bool TryGet(long memberId, out (string Jti, bool Active) session) => _sessions.TryGetValue(memberId, out session);
}
