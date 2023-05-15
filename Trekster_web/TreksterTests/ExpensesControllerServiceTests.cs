using System;
using BusinessLogic.Models;
using BusinessLogic.Services.ServiceInterfaces;
using EmailService;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using Trekster_web.ControllerServices.Implementation;
using Trekster_web.ControllerServices.Interfaces;

namespace treksterTests
{
    public class ExpensesControllerServiceTests
    {
        private Mock<ICurrencyService> _currencyServiceMock;
        private Mock<ITransactionService> _transactionServiceMock;
        private Mock<ICategoryService> _categoryServiceMock;
        private ExpensesControllerService _service;
        private readonly Mock<ILogger<ExpensesControllerService>> _loggerMock;

        public ExpensesControllerServiceTests()
        {
            _currencyServiceMock = new Mock<ICurrencyService>();
            _transactionServiceMock = new Mock<ITransactionService>();
            _categoryServiceMock = new Mock<ICategoryService>();
            _loggerMock = new Mock<ILogger<ExpensesControllerService>>();
            _service = new ExpensesControllerService(_currencyServiceMock.Object, _transactionServiceMock.Object, _categoryServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void GetSummary_ReturnsCorrectString()
        {
            // Arrange
            var currencies = new List<CurrencyModel>
        {
            new CurrencyModel { Id = 1, Name = "USD" },
            new CurrencyModel { Id = 2, Name = "EUR" },
            new CurrencyModel { Id = 3, Name = "UAH" },
        };
            _currencyServiceMock.Setup(c => c.GetAll()).Returns(currencies);

            var transactions = new List<TransactionModel>
        {
            new TransactionModel { Id = 1, CurrencyId = 1, CategoryId = 1, Sum = 10 },
            new TransactionModel { Id = 2, CurrencyId = 2, CategoryId = 1, Sum = 20 },
            new TransactionModel { Id = 3, CurrencyId = 2, CategoryId = 2, Sum = 30 },
        };
            _transactionServiceMock.Setup(t => t.GetAllForUser()).Returns(transactions);

            var categories = new List<CategoryModel>
        {
            new CategoryModel { Id = 1, Name = "Category 1", Type = -1 },
            new CategoryModel { Id = 2, Name = "Category 2", Type = 1 },
        };
            _categoryServiceMock.Setup(c => c.GetById(1)).Returns(categories[0]);
            _categoryServiceMock.Setup(c => c.GetById(2)).Returns(categories[1]);

            _currencyServiceMock.Setup(c => c.GetById(1)).Returns(currencies[0]);
            _currencyServiceMock.Setup(c => c.GetById(2)).Returns(currencies[1]);

            // Act
            var result = _service.GetSummary();

            // Assert
            Assert.Equal("Підсумок: 10 USD, 20 EUR", result);
        }

        [Fact]
        public void GetExpencesByCategory_ReturnsEmptyList_WhenThereAreNoTransactions()
        {
            // Arrange
            _transactionServiceMock.Setup(ts => ts.GetAllForUser()).Returns(new List<TransactionModel>());

            // Act
            var result = _service.GetExpencesByCategory();

            // Assert
            Assert.Empty(result);
        }

    }
}

