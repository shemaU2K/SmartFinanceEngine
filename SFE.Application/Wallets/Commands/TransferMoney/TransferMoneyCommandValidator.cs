using System;
using System.Collections.Generic;
using System.Text;

namespace SFE.Application.Wallets.Commands.TransferMoney
{
    /// <summary>
    /// Defines behavioral validation rules for the <see cref="TransferMoneyCommand"/>.
    /// In an enterprise application, this typically inherits from FluentValidation's AbstractValidator 
    /// to ensure the command payload is inherently valid (e.g., Amount > 0) before hitting the handler logic.
    /// </summary>
    internal class TransferMoneyCommandValidator
    {
        // Example implementation expectation:
        //     RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Transfer amount must be positive.");
        //     RuleFor(x => x.SourceWalletId).NotEqual(Guid.Empty);
        //     RuleFor(x => x.TargetWalletId).NotEqual(x => x.SourceWalletId);
    }
}