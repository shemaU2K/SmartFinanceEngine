using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using SFE.Domain.ValueObjects;

namespace SFE.Application.Wallets.Commands.TransferMoney
{
    public record TransferMoneyCommand(Guid SourceWalletId , Guid TargetWalletId , decimal Amount , Currency Currency ) : IRequest<Guid>
    {
    }
}
