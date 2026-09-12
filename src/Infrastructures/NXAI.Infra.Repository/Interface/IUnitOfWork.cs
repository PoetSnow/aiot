using System.Data;

namespace NXAI.Infra.Repository;

public interface IUnitOfWork : IDisposable
{
    bool IsStartingUow { get; }

    void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, bool distributed = false);

    void Rollback();

    void Commit();

    Task RollbackAsync(CancellationToken cancellationToken = default);

    Task CommitAsync(CancellationToken cancellationToken = default);
}
