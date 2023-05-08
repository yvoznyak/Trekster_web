using System;
using AutoMapper;
using BusinessLogic.Models;
using BusinessLogic.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using Trekster_web.Controllers;
using Trekster_web.ControllerServices.Implementation;
using Trekster_web.Models;

public class HomeControllerServiceTests
{
    private readonly Mock<ILogger<HomeControllerService>> _loggerMock;
    private readonly Mock<ITransactionService> _transactionMock;
    private readonly Mock<IAccountService> _accountMock;
    private readonly Mock<ICurrencyService> _currencyMock;
    private readonly Mock<ICategoryService> _categoryMock;
    private readonly Mock<IStartBalanceService> _startBalanceMock;
    private readonly IMapper _mapper;
    private readonly HomeControllerService _homeControllerService;

    public HomeControllerServiceTests()
    {
        _loggerMock = new Mock<ILogger<HomeControllerService>>();
        _transactionMock = new Mock<ITransactionService>();
        _accountMock = new Mock<IAccountService>();
        _currencyMock = new Mock<ICurrencyService>();
        _categoryMock = new Mock<ICategoryService>();
        _startBalanceMock = new Mock<IStartBalanceService>();

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });
        _mapper = mapperConfig.CreateMapper();

        _homeControllerService = new HomeControllerService(
            _loggerMock.Object,
            _transactionMock.Object,
            _mapper,
            _accountMock.Object,
            _currencyMock.Object,
            _categoryMock.Object,
            _startBalanceMock.Object
        );
    }

    [Fact]
    public void GetListOfAccounts_ReturnsSelectList()
    {
        // Arrange
        var accounts = new List<AccountModel>
        {
            new AccountModel { Id = 1, Name = "Account1", UserId = "1" },
            new AccountModel { Id = 2, Name = "Account2", UserId = "1" }
        };

        var startBalances = new List<StartBalanceModel>
        {
            new StartBalanceModel { Id = 1, AccountId = 1, CurrencyId = 1, Sum = 100 },
            new StartBalanceModel { Id = 2, AccountId = 2, CurrencyId = 2, Sum = 200 }
        };

        var currencies = new List<CurrencyModel>
        {
            new CurrencyModel { Id = 1, Name = "Currency1" },
            new CurrencyModel { Id = 2, Name = "Currency2" }
        };

        _accountMock.Setup(a => a.GetAll()).Returns(accounts);
        _startBalanceMock.Setup(s => s.GetAllForAccount(It.IsAny<AccountModel>())).Returns((AccountModel account) => startBalances.Where(sb => sb.AccountId == account.Id));
        _currencyMock.Setup(c => c.GetById(It.IsAny<int>())).Returns((int id) => currencies.FirstOrDefault(c => c.Id == id));

        // Act
        SelectList result = _homeControllerService.GetListOfAccounts();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal("Account1, Currency1", result.ElementAt(0).Text);
        Assert.Equal("1 1", result.ElementAt(0).Value);
        Assert.Equal("Account2, Currency2", result.ElementAt(1).Text);
    }

    
 
    public void ButtonExist_ReturnsTrueIfTransactionsExist()
    {
        // Arrange
        var transactions = new List<TransactionModel>
    {
        new TransactionModel { Id = 1, Sum = 100 },
        new TransactionModel { Id = 2, Sum = -50 },
        new TransactionModel { Id = 3, Sum = -25 }
    };

        _transactionMock.Setup(t => t.GetAllForUser()).Returns(transactions);

        // Act
        bool result = _homeControllerService.ButtonExist();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ButtonExist_ReturnsFalseIfNoTransactionsExist()
    {
        // Arrange
        var transactions = new List<TransactionModel>();

        _transactionMock.Setup(t => t.GetAllForUser()).Returns(transactions);

        // Act
        bool result = _homeControllerService.ButtonExist();

        // Assert
        Assert.False(result);
    }

  
}
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AccountModel, AccountModel>().ReverseMap();
            CreateMap<TransactionModel, TransactionModel>().ReverseMap();
            CreateMap<CurrencyModel, CurrencyModel>().ReverseMap();
            CreateMap<CategoryModel, CategoryModel>().ReverseMap();
            CreateMap<StartBalanceModel, StartBalanceModel>().ReverseMap();
            // Add other mappings as needed.
        }
    }