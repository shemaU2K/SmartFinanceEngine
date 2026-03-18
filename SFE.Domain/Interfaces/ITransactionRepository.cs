using SFE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFE.Domain.Interfaces
{
    public interface ITransactionRepository
    {
        Task AddAsync(Transaction transaction, CancellationToken ct = default);
    }
}
