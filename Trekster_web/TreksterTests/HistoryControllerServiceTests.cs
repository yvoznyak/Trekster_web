using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessLogic.Models;
using BusinessLogic.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Routing;
using Trekster_web.ControllerServices.Implementation;
using Trekster_web.ControllerServices.Interfaces;
using Trekster_web.Models;
using Microsoft.Extensions.Logging;

public class HistoryControllerServiceTests
{
    private Mock<ICurrencyService> _currencyServiceMock;
    private Mock<ITransactionService> _transactionServiceMock;
    private Mock<ICategoryService> _categoryServiceMock;
    private Mock<IAccountService> _accountServiceMock;
    private Mock<IStartBalanceService> _startBalanceServiceMock;
    private HistoryControllerService _historyControllerService;
    private IMapper _mapper;
    private readonly Mock<ILogger<HistoryControllerService>> _loggerMock;

    public HistoryControllerServiceTests()
    {
        _transactionServiceMock = new Mock<ITransactionService>();
        _accountServiceMock = new Mock<IAccountService>();
        _categoryServiceMock = new Mock<ICategoryService>();
        _currencyServiceMock = new Mock<ICurrencyService>();
        _startBalanceServiceMock = new Mock<IStartBalanceService>();
        _loggerMock = new Mock<ILogger<HistoryControllerService>>();

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<TransactionVM, TransactionModel>().ReverseMap();
        });

        _mapper = mapperConfig.CreateMapper();

        _historyControllerService = new HistoryControllerService(
            _transactionServiceMock.Object,
            _accountServiceMock.Object,
            _categoryServiceMock.Object,
            _currencyServiceMock.Object,
            _startBalanceServiceMock.Object,
            _mapper,
            _loggerMock.Object
            );
    }

    [Fact]
    public void GetListOfAccounts_ReturnsCorrectSelectList()
    {
        // Arrange
        var accounts = new List<AccountModel>
        {
            new AccountModel { Id = 1, Name = "Account1" },
            new AccountModel { Id = 2, Name = "Account2" },
        };

        _accountServiceMock.Setup(x => x.GetAll()).Returns(accounts);

        var startBalances = new List<StartBalanceModel>
        {
            new StartBalanceModel { Id = 1, Sum = 1000, AccountId = 1, CurrencyId = 1 },
            new StartBalanceModel { Id = 2, Sum = 2000, AccountId = 2, CurrencyId = 2 },
        };

        _startBalanceServiceMock.Setup(x => x.GetAllForAccount(It.IsAny<AccountModel>())).Returns((AccountModel account) => startBalances.FindAll(s => s.AccountId == account.Id));

        var currency1 = new CurrencyModel { Id = 1, Name = "Currency1" };
        var currency2 = new CurrencyModel { Id = 2, Name = "Currency2" };
        _currencyServiceMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((int id) => id == 1 ? currency1 : currency2);

        // Act
        SelectList result = _historyControllerService.GetListOfAccounts();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal("Account1, Currency1", result.ElementAt(0).Text);
        Assert.Equal("1 1", result.ElementAt(0).Value);
        Assert.Equal("Account2, Currency2", result.ElementAt(1).Text);
        Assert.Equal("2 2", result.ElementAt(1).Value);
    }

    [Fact]
    public void SaveTransaction_SavesCorrectTransactionNotNull()
    {
        // Arrange
        TransactionModel savedTransaction = null;
        _transactionServiceMock.Setup(x => x.Save(It.IsAny<TransactionModel>())).Callback<TransactionModel>(transaction => savedTransaction = transaction);
        var transactionVM = new TransactionVM
        {
            Date = new DateTime(2023, 5, 1),
            Sum = 100,
            AccountsAndCurency = "1 1",
            CategoryId = 1,
        };

        // Act
        _historyControllerService.SaveTransaction(transactionVM);

        // Assert
        Assert.NotNull(savedTransaction);
    }

    [Fact]
    public void SaveTransaction_SavesCorrectTransactionEqualAccountId()
    {
        // Arrange
        TransactionModel savedTransaction = null;
        _transactionServiceMock.Setup(x => x.Save(It.IsAny<TransactionModel>())).Callback<TransactionModel>(transaction => savedTransaction = transaction);
        var transactionVM = new TransactionVM
        {
            Date = new DateTime(2023, 5, 1),
            Sum = 100,
            AccountsAndCurency = "1 1",
            CategoryId = 1,
        };

        // Act
        _historyControllerService.SaveTransaction(transactionVM);

        // Assert
        Assert.Equal(1, savedTransaction.AccountId);
    }

    [Fact]
    public void SaveTransaction_SavesCorrectTransactionEqualDate()
    {
        // Arrange
        TransactionModel savedTransaction = null;
        _transactionServiceMock.Setup(x => x.Save(It.IsAny<TransactionModel>())).Callback<TransactionModel>(transaction => savedTransaction = transaction);
        var transactionVM = new TransactionVM
        {
            Date = new DateTime(2023, 5, 1),
            Sum = 100,
            AccountsAndCurency = "1 1",
            CategoryId = 1,
        };

        // Act
        _historyControllerService.SaveTransaction(transactionVM);

        // Assert
        Assert.Equal(new DateTime(2023, 5, 1), savedTransaction.Date);
    }

    [Fact]
    public void SaveTransaction_SavesCorrectTransactionEqualCurrencyIdCategory()
    {
        // Arrange
        TransactionModel savedTransaction = null;
        _transactionServiceMock.Setup(x => x.Save(It.IsAny<TransactionModel>())).Callback<TransactionModel>(transaction => savedTransaction = transaction);
        var transactionVM = new TransactionVM
        {
            Date = new DateTime(2023, 5, 1),
            Sum = 100,
            AccountsAndCurency = "1 1",
            CategoryId = 1,
        };

        // Act
        _historyControllerService.SaveTransaction(transactionVM);

        // Assert
        Assert.Equal(1, savedTransaction.CurrencyId);
        Assert.Equal(1, savedTransaction.CategoryId);
    }

}