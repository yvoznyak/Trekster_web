using AutoMapper;
using BusinessLogic.Models;
using BusinessLogic.Services.ServiceInterfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using Trekster_web.Controllers;
using Trekster_web.ControllerServices.Implementation;
using Trekster_web.Models;
using Xunit;

public class AccountControllerServiceTests
{
    private readonly Mock<ICurrencyService> _currencyServiceMock = new Mock<ICurrencyService>();
    private readonly Mock<IAccountService> _accountServiceMock = new Mock<IAccountService>();
    private readonly Mock<IStartBalanceService> _startBalanceServiceMock = new Mock<IStartBalanceService>();
    private readonly Mock<ITransactionService> _transactionServiceMock = new Mock<ITransactionService>();
    private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();
    private readonly Mock<ILogger<AccountControllerService>> _loggerMock;
    private readonly AccountControllerService _accountControllerService;

    public AccountControllerServiceTests()
    {
        _loggerMock = new Mock<ILogger<AccountControllerService>>();
        _accountControllerService = new AccountControllerService(
            _currencyServiceMock.Object,
            _accountServiceMock.Object,
            _startBalanceServiceMock.Object,
            _transactionServiceMock.Object,
            _mapperMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public void GetAccountsInfo_ShouldReturnListOfAccountInfos()
    {
        // Arrange
        var accountModel = new AccountModel { Id = 1, Name = "TestAccount", UserId = "1" };
        var currencyModel = new CurrencyModel { Id = 1, Name = "USD" };
        var startBalanceModel = new StartBalanceModel { Id = 1, Sum = 100, AccountId = accountModel.Id, CurrencyId = currencyModel.Id };
        var transactionModel = new TransactionModel { Id = 1, Sum = -20, AccountId = accountModel.Id, CurrencyId = currencyModel.Id };
        var accounts = new List<AccountModel> { accountModel };
        var startBalances = new List<StartBalanceModel> { startBalanceModel };
        var transactions = new List<TransactionModel> { transactionModel };
        var expected = new List<string> { "TestAccount: 80 USD " };

        _accountServiceMock.Setup(x => x.GetAll()).Returns(accounts);
        _startBalanceServiceMock.Setup(x => x.GetAllForAccount(accountModel)).Returns(startBalances);
        _transactionServiceMock.Setup(x => x.GetAllForAccount(accountModel)).Returns(transactions);
        _currencyServiceMock.Setup(x => x.GetById(currencyModel.Id)).Returns(currencyModel);
        _transactionServiceMock.Setup(x => x.GetFinalSum(transactionModel.Id)).Returns(-20);

        // Act
        var result = _accountControllerService.GetAccountsInfo();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void StartBalancesNotEmpty_WithNonZeroBalances_ShouldReturnTrue()
    {
        // Arrange
        var accountsVM = new AccountsVM { StartBalances = new Dictionary<string, float> { { "USD", 100f } } };

        // Act
        var result = _accountControllerService.StartBalancesNotEmpty(accountsVM);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void StartBalancesNotEmpty_WithZeroBalances_ShouldReturnFalse()
    {
        // Arrange
        var accountsVM = new AccountsVM { StartBalances = new Dictionary<string, float> { { "USD", 0f } } };

        // Act
        var result = _accountControllerService.StartBalancesNotEmpty(accountsVM);

        // Assert
        Assert.False(result);
    }
}
