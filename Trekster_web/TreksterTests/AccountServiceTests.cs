using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using BusinessLogic.Services.ServiceImplementation;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Trekster_web.Controllers;
using Trekster_web.ControllerServices.Implementation;
using Xunit;

namespace YourProjectNamespace.Tests
{
    public class AccountServiceTests
    {
        private readonly Mock<IAccount> _accountMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly AccountService _accountService;
        private readonly Mock<ILogger<AccountService>> _loggerMock;

        public AccountServiceTests()
        {
            _accountMock = new Mock<IAccount>();
            _mapperMock = new Mock<IMapper>();
            _userManagerMock = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _loggerMock = new Mock<ILogger<AccountService>>();
            _accountService = new AccountService(_accountMock.Object, _mapperMock.Object, _userManagerMock.Object, _httpContextAccessorMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void GetById_ReturnsAccountByIdNotNull()
        {
            // Arrange
            int testAccountId = 1;
            var testAccount = new Account { Id = testAccountId, Name = "Account1" };

            _accountMock.Setup(x => x.GetById(testAccountId)).Returns(testAccount);
            _mapperMock.Setup(x => x.Map<AccountModel>(testAccount)).Returns(new AccountModel { Id = testAccountId, Name = "Account1" });

            // Act
            var result = _accountService.GetById(testAccountId);

            // Assert
            Assert.NotNull(result);
            
        }
        [Fact]
        public void GetById_ReturnsAccountByIdEqualId()
        {
            // Arrange
            int testAccountId = 1;
            var testAccount = new Account { Id = testAccountId, Name = "Account1" };

            _accountMock.Setup(x => x.GetById(testAccountId)).Returns(testAccount);
            _mapperMock.Setup(x => x.Map<AccountModel>(testAccount)).Returns(new AccountModel { Id = testAccountId, Name = "Account1" });

            // Act
            var result = _accountService.GetById(testAccountId);

            // Assert

            Assert.Equal(testAccountId, result.Id);
        }

        [Fact]
        public void GetById_ReturnsAccountByIdEqualIdHardcoded()
        {
            // Arrange
            int testAccountId = 1;
            var testAccount = new Account { Id = testAccountId, Name = "Account1" };

            _accountMock.Setup(x => x.GetById(testAccountId)).Returns(testAccount);
            _mapperMock.Setup(x => x.Map<AccountModel>(testAccount)).Returns(new AccountModel { Id = testAccountId, Name = "Account1" });

            // Act
            var result = _accountService.GetById(testAccountId);

            // Assert

            Assert.Equal("Account1", result.Name);
        }

        [Fact]
        public void Delete_DeletesAccountById()
        {
            // Arrange
            int testAccountId = 1;

            _accountMock.Setup(x => x.Delete(testAccountId));

            // Act
            _accountService.Delete(testAccountId);

            // Assert
            _accountMock.Verify(x => x.Delete(testAccountId), Times.Once);
        }

        [Fact]
        public void GetLast_ReturnsLastAccount()
        {
            // Arrange
                    var testAccounts = new List<Account>
            {
                new Account { Id = 1, Name = "Account1" },
                new Account { Id = 2, Name = "Account2" }
            };

            _accountMock.Setup(x => x.GetAll()).Returns(testAccounts.AsQueryable());
            _mapperMock.Setup(x => x.Map<AccountModel>(testAccounts[1])).Returns(new AccountModel { Id = 2, Name = "Account2" });

            // Act
            var result = _accountService.GetLast();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Id);
            Assert.Equal("Account2", result.Name);
        }
    }
}