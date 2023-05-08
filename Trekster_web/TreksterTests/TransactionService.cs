using System;
using System.Security.Claims;
using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using BusinessLogic.Services.ServiceImplementation;
using BusinessLogic.Services.ServiceInterfaces;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Moq;

namespace TreksterTests
{
    public class TransactionServiceTests
    {
        private readonly Mock<ITransaction> _transactionMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<IAccountService> _accountMock;

        private readonly TransactionService _transactionService;
        private readonly Mock<ILogger<TransactionService>> _loggerMock;

        public TransactionServiceTests()
        {
            _transactionMock = new Mock<ITransaction>();
            _mapperMock = new Mock<IMapper>();
            _userManagerMock = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _accountMock = new Mock<IAccountService>();
            _loggerMock = new Mock<ILogger<TransactionService>>();
            _transactionService = new TransactionService(_transactionMock.Object, _mapperMock.Object,
                                                          _userManagerMock.Object, _httpContextAccessorMock.Object,
                                                          _accountMock.Object,
                                                          _loggerMock.Object);
        }

        [Fact]
        public void GetAll_ReturnsAllTransactions()
        {
            // Arrange
            var transactions = new List<Transaction>
            {
                new Transaction { Id = 1, Sum = 100 },
                new Transaction { Id = 2, Sum = 200 }
            };
            var expectedTransactionModels = new List<TransactionModel>
            {
                new TransactionModel { Id = 1, Sum = 100 },
                new TransactionModel { Id = 2, Sum = 200 }
            };
            _transactionMock.Setup(x => x.GetAll()).Returns(transactions);
            _mapperMock.Setup(x => x.Map<List<TransactionModel>>(transactions)).Returns(expectedTransactionModels);

            // Act
            var result = _transactionService.GetAll();

            // Assert
            Assert.Equal(expectedTransactionModels, result);
        }

        [Fact]
        public void GetById_ReturnsTransactionById()
        {
            // Arrange
            var transactionId = 1;
            var transaction = new Transaction { Id = transactionId, Sum = 100 };
            var expectedTransactionModel = new TransactionModel { Id = transactionId, Sum = 100 };
            _transactionMock.Setup(x => x.GetById(transactionId)).Returns(transaction);
            _mapperMock.Setup(x => x.Map<TransactionModel>(transaction)).Returns(expectedTransactionModel);

            // Act
            var result = _transactionService.GetById(transactionId);

            // Assert
            Assert.Equal(expectedTransactionModel, result);
        }

        [Fact]
        public void Save_CallsTransactionSave()
        {
            // Arrange
            var transactionModel = new TransactionModel { Sum = 100 };

            // Act
            _transactionService.Save(transactionModel);

            // Assert
            _transactionMock.Verify(x => x.Save(It.IsAny<Transaction>()), Times.Once);
        }

        [Fact]
        public void Delete_CallsTransactionDelete()
        {
            // Arrange
            var transactionId = 1;

            // Act
            _transactionService.Delete(transactionId);

            // Assert
            _transactionMock.Verify(x => x.Delete(transactionId), Times.Once);
        }

        [Fact]
        public void GetFinalSum_ReturnsSumOfTransactionsForTransactionId()
        {
            // Arrange
            var transactionId = 1;
            var transaction = new Transaction { Id = transactionId, Sum = 100, Category = new Category { Type = 2 } };
            _transactionMock.Setup(x => x.GetById(transactionId)).Returns(transaction);

            // Act
            var result = _transactionService.GetFinalSum(transactionId);

            // Assert
            Assert.Equal(200, result);
        }

        [Fact]
        public void GetAllForAccount_ReturnsTransactionsForAccount()
        {
            // Arrange
            var accountModel = new AccountModel { Id = 1, Name = "Test Account", UserId = "Test User" };
            var transactions = new List<Transaction>
            {
                new Transaction { Id = 1, Sum = 100, AccountId = accountModel.Id },
                new Transaction { Id = 2, Sum = 200, AccountId = accountModel.Id },
                new Transaction { Id = 3, Sum = 300, AccountId = accountModel.Id + 1 }
            };
                    var expectedTransactionModels = new List<TransactionModel>
            {
                new TransactionModel { Id = 1, Sum = 100, AccountId = accountModel.Id },
                new TransactionModel { Id = 2, Sum = 200, AccountId = accountModel.Id }
            };
            _transactionMock.Setup(x => x.GetAll()).Returns(transactions);
            _mapperMock.Setup(x => x.Map<List<TransactionModel>>(It.Is<IEnumerable<Transaction>>(t => t.All(t => t.AccountId == accountModel.Id)))).Returns(expectedTransactionModels);

            // Act
            var result = _transactionService.GetAllForAccount(accountModel);

            // Assert
            Assert.Equal(expectedTransactionModels, result);
        }

        [Fact]
        public void GetAllForUser_ReturnsTransactionsForUser()
        {
            // Arrange
            var user = new User { Id = "Test User" };
            var account1 = new Account { Id = 1, Name = "Test Account 1", UserId = user.Id };
            var account2 = new Account { Id = 2, Name = "Test Account 2", UserId = "Other User" };
            var account3 = new Account { Id = 3, Name = "Test Account 3", UserId = user.Id };
            var transactions = new List<Transaction>
            {
                new Transaction { Id = 1, Sum = 100, Account = account1 },
                new Transaction { Id = 2, Sum = 200, Account = account2 },
                new Transaction { Id = 3, Sum = 300, Account = account3 }
            };
                    var expectedTransactionModels = new List<TransactionModel>
            {
                new TransactionModel { Id = 1, Sum = 100, AccountId = account1.Id },
                new TransactionModel { Id = 3, Sum = 300, AccountId = account3.Id }
            };
            _transactionMock.Setup(x => x.GetAll()).Returns(transactions);
            _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            _mapperMock.Setup(x => x.Map<List<TransactionModel>>(It.Is<IEnumerable<Transaction>>(t => t.All(t => t.Account.UserId == user.Id)))).Returns(expectedTransactionModels);

            // Act
            var result = _transactionService.GetAllForUser();

            // Assert
            Assert.Equal(expectedTransactionModels, result);
        }

    }
}

