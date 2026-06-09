using Moq;
using SFE.Application.Wallets.Commands.TransferMoney;
using SFE.Domain.Entities;
using SFE.Domain.Interfaces;
using SFE.Domain.Services;
using SFE.Domain.ValueObjects;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Xunit;

namespace SFE.Tests.Application.Wallets.Commands
{
    public class TransferMoneyCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ValidCommand_ShouldProcessTransferAndSaveToRepositories()
        {
            // Arrange
            var sourceWalletId = Guid.NewGuid();
            var targetWalletId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var currency = Currency.EUR;
            var transferAmount = 100m;

            var sourceWallet = new Wallet("Source", userId, currency);
            sourceWallet.AddFunds(new Money(300m, currency)); // Balance: 300

            var targetWallet = new Wallet("Target", userId, currency); // Balance: 0

            // 1. Setup Mock Repositories
            var mockWalletRepo = new Mock<IWalletRepository>();
            mockWalletRepo
                .Setup(repo => repo.GetByIdAsync(sourceWalletId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(sourceWallet);

            mockWalletRepo
                .Setup(repo => repo.GetByIdAsync(targetWalletId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(targetWallet);

            var mockTransactionRepo = new Mock<ITransactionRepository>();

            var transactionService = new TransactionServiсe();

            // 2. Instantiate the Handler
            var handler = new TransferMoneyCommandHandler(
                mockWalletRepo.Object,
                mockTransactionRepo.Object,
                transactionService);

            var command = new TransferMoneyCommand(sourceWalletId, targetWalletId, transferAmount, currency);

            // Act
            var transactionId = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotEqual(Guid.Empty, transactionId);

            // Validate Balances
            Assert.Equal(200m, sourceWallet.Balance.Amount);
            Assert.Equal(100m, targetWallet.Balance.Amount);

            // Validate that we informed the database to save our changes
            mockWalletRepo.Verify(repo => repo.UpdateAsync(sourceWallet, It.IsAny<CancellationToken>()), Times.Once);
            mockWalletRepo.Verify(repo => repo.UpdateAsync(targetWallet, It.IsAny<CancellationToken>()), Times.Once);
            mockTransactionRepo.Verify(repo => repo.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_SourceWalletNotFound_ShouldThrowException()
        {
            // Arrange
            var mockWalletRepo = new Mock<IWalletRepository>();
            // Explicitly return null for GetByIdAsync to simulate not found
            mockWalletRepo
                .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Wallet?)null);

            var handler = new TransferMoneyCommandHandler(
                mockWalletRepo.Object,
                new Mock<ITransactionRepository>().Object,
                new TransactionServiсe());

            var command = new TransferMoneyCommand(Guid.NewGuid(), Guid.NewGuid(), 100m, Currency.USD);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
            Assert.Contains("not found", exception.Message);
        }
    }
}