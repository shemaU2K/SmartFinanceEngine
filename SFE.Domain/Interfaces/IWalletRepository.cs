using System;
using System.Collections.Generic;
using System.Text;
using SFE.Domain.Entities;

namespace SFE.Domain.Interfaces
{
    public interface IWalletRepository
    {
            Task<Wallet?> GetByIdAsync(Guid id, CancellationToken ct = default);
            
            Task AddAsync(Wallet wallet, CancellationToken ct = default);
    
            Task UpdateAsync(Wallet wallet, CancellationToken ct = default);
    }
}
