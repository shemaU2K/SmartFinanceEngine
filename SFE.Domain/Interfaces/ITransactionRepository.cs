using SFE.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace SFE.Domain.Interfaces
{
    /// <summary>
    /// Defines the contract for a repository that manages the persistence of <see cref="Transaction"/> entities.
    /// </summary>
    public interface ITransactionRepository
    {
        /// <summary>
        /// Asynchronously adds a new transaction record to the underlying data store.
        /// </summary>
        /// <param name="transaction">The transaction entity to persist.</param>
        /// <param name="ct">A token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddAsync(Transaction transaction, CancellationToken ct = default);
    }
}