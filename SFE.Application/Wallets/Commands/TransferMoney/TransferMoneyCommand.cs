using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using SFE.Domain.ValueObjects;

namespace SFE.Application.Wallets.Commands.TransferMoney
{
    /// <summary>
    /// Represents a command parameter block to transfer a specific monetary amount between two wallets.
    /// Implements <see cref="IRequest{Guid}"/> signifying it will return a completed transaction's unique ID.
    /// </summary>
    /// <param name="SourceWalletId">The unique identifier of the wallet to deduct the funds from.</param>
    /// <param name="TargetWalletId">The unique identifier of the wallet to receive the funds.</param>
    /// <param name="Amount">The decimal amount of funds to transfer.</param>
    /// <param name="Currency">The currency value object dictating the denomination of the transfer.</param>
    public record TransferMoneyCommand(Guid SourceWalletId, Guid TargetWalletId, decimal Amount, Currency Currency) : IRequest<Guid>
    {
    }
}