namespace NXAI.Cooking.Application.Acl;

/// <summary>耗材防腐。投放前只读余量，不改账本。</summary>
public interface IInventoryGateway
{
    /// <summary>PACKAGE 余量是否够。</summary>
    Task<bool> HasEnoughAsync(long memberId, string typeCode, decimal qty);
}
