using NXAI.Member.Application.Contracts.Interfaces;

namespace NXAI.AfterSales.Application.Acl;

/// <summary>转发 Member 契约。</summary>
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
