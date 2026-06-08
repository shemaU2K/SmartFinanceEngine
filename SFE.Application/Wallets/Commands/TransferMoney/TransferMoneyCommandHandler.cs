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
    /// <summary>
    /// Handles the execution of a <see cref="TransferMoneyCommand"/>. 
    /// Orchestrates the retrieval of wallets from storage, applies the domain-level transaction logic, 
    /// and persists state changes entirely.
    /// </summary>
    public class TransferMoneyCommandHandler : IRequestHandler<TransferMoneyCommand, Guid>
    {
        /// <summary>
        /// Data access repository for retrieving and updating <see cref="Wallet"/> states within the database.
        /// </summary>
        private readonly IWalletRepository _walletRepository;

        /// <summary>
        /// Data access repository for logging and persisting historical <see cref="Transaction"/> records.
        /// </summary>
        private readonly ITransactionRepository _transactionRepository;

        /// <summary>
        /// Core domain service responsible for enforcing the actual business rules of a wallet transfer operation.
        /// </summary>
        private readonly TransactionServiсe _transactionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransferMoneyCommandHandler"/> class.
        /// </summary>
        /// <param name="walletRepository">The injected wallet repository instance.</param>
        /// <param name="transactionRepository">The injected transaction repository instance.</param>
        /// <param name="transactionService">The injected domain logic service instance.</param>
        public TransferMoneyCommandHandler(
            IWalletRepository walletRepository,
            ITransactionRepository transactionRepository,
            TransactionServiсe transactionService)
        {
            _walletRepository = walletRepository;
            _transactionRepository = transactionRepository;
            _transactionService = transactionService;
        }

        /// <summary>
        /// Processes the money transfer workflow.
        /// </summary>
        /// <param name="request">The transfer command payload holding the parameters (Ids, amounts).</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>A task representing the asynchronous operation, returning the newly created Transaction's unique <see cref="Guid"/>.</returns>
        /// <exception cref="Exception">Thrown if either the origin or destination wallet cannot be located by ID.</exception>
        public async Task<Guid> Handle(TransferMoneyCommand request, CancellationToken cancellationToken)
        {
            // 1. Validate Source Wallet existence
            var fromWallet = await _walletRepository.GetByIdAsync(request.SourceWalletId, cancellationToken);
            if (fromWallet == null)
            {
                throw new Exception($"Source wallet with ID {request.SourceWalletId} not found.");
            }

            // 2. Validate Target Wallet existence
            var toWallet = await _walletRepository.GetByIdAsync(request.TargetWalletId, cancellationToken);
            if (toWallet == null)
            {
                throw new Exception($"Target wallet with ID {request.TargetWalletId} not found.");
            }

            // 3. Construct domain value objects
            var amount = new Money(request.Amount, request.Currency);

            // 4. Delegate core logic to Domain Service
            var transaction = _transactionService.ExecuteTransaction(fromWallet, toWallet, amount);

            // 5. Persist the state mutations
            await _walletRepository.UpdateAsync(fromWallet, cancellationToken);
            await _walletRepository.UpdateAsync(toWallet, cancellationToken);
            await _transactionRepository.AddAsync(transaction, cancellationToken);

            // 6. Return the resulting transaction ID
            return transaction.Id;
        }
    }
}