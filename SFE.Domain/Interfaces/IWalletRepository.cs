using System;
using System.Threading;
using System.Threading.Tasks;
using SFE.Domain.Entities;

namespace SFE.Domain.Interfaces
{
    /// <summary>
    /// Defines the contract for a repository that manages the persistence and retrieval of <see cref="Wallet"/> entities.
    /// </summary>
    public interface IWalletRepository
    {
        /// <summary>
        /// Asynchronously retrieves a wallet by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the wallet.</param>
        /// <param name="ct">A token to monitor for cancellation requests.</param>
        /// <returns>The wallet if found; otherwise, null.</returns>
        Task<Wallet?> GetByIdAsync(Guid id, CancellationToken ct = default);

        /// <summary>
        /// Asynchronously adds a newly created wallet to the underlying data store.
        /// </summary>
        /// <param name="wallet">The wallet entity to persist.</param>
        /// <param name="ct">A token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddAsync(Wallet wallet, CancellationToken ct = default);

        /// <summary>
        /// Asynchronously updates an existing wallet in the underlying data store.
        /// </summary>
        /// <param name="wallet">The modified wallet entity to save.</param>
        /// <param name="ct">A token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateAsync(Wallet wallet, CancellationToken ct = default);
    }
}