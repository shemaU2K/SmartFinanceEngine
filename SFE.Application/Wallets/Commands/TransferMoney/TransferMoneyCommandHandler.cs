using MediatR;
using SFE.Domain.Entities;
using SFE.Domain.Interfaces;
using SFE.Domain.Services;
using SFE.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFE.Application.Wallets.Commands.TransferMoney
{
    public class TransferMoneyCommandHandler : IRequestHandler<TransferMoneyCommand, Guid>
    {
        private readonly IWalletRepository _walletRepository;

        private readonly ITransactionRepository _transactionRepository;

        private readonly TransactionServiсe _transactionService;
        public TransferMoneyCommandHandler(IWalletRepository walletRepository, ITransactionRepository transactionRepository , TransactionServiсe transactionService)
        {
            _walletRepository = walletRepository;
            _transactionRepository = transactionRepository;
            _transactionService = transactionService;
        }
        public async Task<Guid> Handle(TransferMoneyCommand request, CancellationToken cancellationToken)
        {
            var fromWallet = await _walletRepository.GetByIdAsync(request.SourceWalletId, cancellationToken);
            if (fromWallet == null)
            {
                throw new Exception($"Source wallet with ID {request.SourceWalletId} not found.");
            }

            var toWallet = await _walletRepository.GetByIdAsync(request.TargetWalletId, cancellationToken);
            if (toWallet == null)
            {
                throw new Exception($"Target wallet with ID {request.TargetWalletId} not found.");
            }

            var amount = new Money(request.Amount, request.Currency);

            var transaction = _transactionService.ExecuteTransaction(fromWallet, toWallet, amount);

            await _walletRepository.UpdateAsync(fromWallet, cancellationToken);
            await _walletRepository.UpdateAsync(toWallet, cancellationToken);
            await _transactionRepository.AddAsync(transaction, cancellationToken);
            return transaction.Id;
        }
    }
}
