using System;
using System.Xml.Linq;
using BusinessLogic.Models;
using BusinessLogic.Services.ServiceInterfaces;
using Infrastructure.Migrations;
using Microsoft.AspNetCore.Routing;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Microsoft.Extensions.Logging;
using Moq;
using Trekster_web.ControllerServices.Implementation;
using static System.Runtime.InteropServices.JavaScript.JSType;
public class ProfitsControllerServiceTests
{
    private readonly Mock<ICurrencyService> _currencyServiceMock;
    private readonly Mock<ITransactionService> _transactionServiceMock;
    private readonly Mock<ICategoryService> _categoryServiceMock;
    private readonly ProfitsControllerService _profitsControllerService;
    private readonly Mock<ILogger<ProfitsControllerService>> _loggerMock;

    public ProfitsControllerServiceTests()
    {
        _currencyServiceMock = new Mock<ICurrencyService>();
        _transactionServiceMock = new Mock<ITransactionService>();
        _categoryServiceMock = new Mock<ICategoryService>();
        _loggerMock = new Mock<ILogger<ProfitsControllerService>>();

        _profitsControllerService = new ProfitsControllerService(
            _currencyServiceMock.Object,
            _transactionServiceMock.Object,
            _categoryServiceMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public void GetProfitsByCategory_ShouldReturnEmptyList_WhenNoTransactionsFound()
    {
        // Arrange
        _transactionServiceMock.Setup(m => m.GetAllForUser()).Returns(new List<TransactionModel>());

        // Act
        var result = _profitsControllerService.GetProfitsByCategory();

        // Assert
        
        Assert.Empty(result);
    }

    [Fact]
    public void GetProfitsByCategory_ShouldReturnNotNull_WhenNoTransactionsFound()
    {
        // Arrange
        _transactionServiceMock.Setup(m => m.GetAllForUser()).Returns(new List<TransactionModel>());

        // Act
        var result = _profitsControllerService.GetProfitsByCategory();

        // Assert
        Assert.NotEqual(result, null);
    }

    [Fact]
    public void GetProfitsByCategory_ShouldReturnListType_WhenNoTransactionsFound()
    {
        // Arrange
        _transactionServiceMock.Setup(m => m.GetAllForUser()).Returns(new List<TransactionModel>());

        // Act
        var result = _profitsControllerService.GetProfitsByCategory();

        // Assert
        Assert.IsType<List<string>>(result);
    }
}
