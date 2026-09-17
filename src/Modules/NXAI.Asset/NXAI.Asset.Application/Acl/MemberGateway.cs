using NXAI.Member.Application.Contracts.Interfaces;

namespace NXAI.Asset.Application.Acl;

/// <summary>转发到 Member 契约。对方 DTO 变化只改这里。</summary>
public sealed class MemberGateway(IMemberService members) : IMemberGateway
{
    public async Task<bool> ExistsActiveAsync(long memberId)
    {
        if (!await members.ExistsAsync(memberId))
        {
            return false;
        }

        return !await members.IsCancelledAsync(memberId);
    }
}
