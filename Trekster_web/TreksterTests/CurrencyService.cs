using System;
using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using BusinessLogic.Services.ServiceImplementation;
using BusinessLogic.Services.ServiceInterfaces;
using Infrastructure.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace TreksterTests
{
    public class CurrencyServiceTests
    {
        private readonly Mock<ICurrency> _currencyMock;
        private readonly Mock<IStartBalanceService> _startBalanceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CurrencyService _currencyService;
        private readonly Mock<ILogger<CurrencyService>> _loggerMock;

        public CurrencyServiceTests()
        {
            _currencyMock = new Mock<ICurrency>();
            _startBalanceMock = new Mock<IStartBalanceService>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<CurrencyService>>();
            _currencyService = new CurrencyService(_currencyMock.Object, _mapperMock.Object, _startBalanceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void GetAll_ReturnsCorrectModel()
        {
            // Arrange
            var entities = new List<Currency>();
            entities.Add(new Currency { Id = 1, Name = "USD" });
            entities.Add(new Currency { Id = 2, Name = "EUR" });
            _currencyMock.Setup(c => c.GetAll()).Returns(entities);

            var models = new List<CurrencyModel>();
            models.Add(new CurrencyModel { Id = 1, Name = "USD" });
            models.Add(new CurrencyModel { Id = 2, Name = "EUR" });
            _mapperMock.Setup(m => m.Map<List<CurrencyModel>>(entities)).Returns(models);

            // Act
            var result = _currencyService.GetAll();

            // Assert
            Assert.Equal(models, result);
        }

        [Fact]
        public void GetAllByAccount_ReturnsCorrectModel()
        {
            // Arrange
            var account = new AccountModel { Id = 1, Name = "Test Account", UserId = "123" };
            var startBalances = new List<StartBalance>();
            startBalances.Add(new StartBalance { AccountId = 1, CurrencyId = 1, Sum = 100 });
            startBalances.Add(new StartBalance { AccountId = 1, CurrencyId = 2, Sum = 200 });
            _startBalanceMock.Setup(s => s.GetAllForAccount(account)).Returns(startBalances.Select(s => new StartBalanceModel { AccountId = s.AccountId, CurrencyId = s.CurrencyId, Sum = s.Sum }));

            var currencies = new List<Currency>();
            currencies.Add(new Currency { Id = 1, Name = "USD" });
            currencies.Add(new Currency { Id = 2, Name = "EUR" });
            _currencyMock.Setup(c => c.GetById(1)).Returns(currencies[0]);
            _currencyMock.Setup(c => c.GetById(2)).Returns(currencies[1]);

            var models = new List<CurrencyModel>();
            models.Add(new CurrencyModel { Id = 1, Name = "USD" });
            models.Add(new CurrencyModel { Id = 2, Name = "EUR" });
            _mapperMock.Setup(m => m.Map<List<CurrencyModel>>(It.IsAny<List<Currency>>())).Returns(models);

            // Act
            var result = _currencyService.GetAllByAccount(account);

            // Assert
            Assert.Equal(models, result);
        }

        [Fact]
        public void GetAll_ReturnsExpectedResult()
        {
            // Arrange
            var currencies = new List<Currency>
            {
                new Currency { Id = 1, Name = "USD" },
                new Currency { Id = 2, Name = "EUR" },
                new Currency { Id = 3, Name = "GBP" }
            };
            var currencyModels = new List<CurrencyModel>
            {
                new CurrencyModel { Id = 1, Name = "USD" },
                new CurrencyModel { Id = 2, Name = "EUR" },
                new CurrencyModel { Id = 3, Name = "GBP" }
            };
            _currencyMock.Setup(c => c.GetAll()).Returns(currencies);
            _mapperMock.Setup(m => m.Map<List<CurrencyModel>>(currencies)).Returns(currencyModels);

            // Act
            var result = _currencyService.GetAll();

            // Assert
            Assert.Equal(currencyModels, result);
            _currencyMock.Verify(c => c.GetAll(), Times.Once);
            _mapperMock.Verify(m => m.Map<List<CurrencyModel>>(currencies), Times.Once);
        }

        [Fact]
        public void GetByName_WithExistingCurrency_ReturnsExpectedResult()
        {
            // Arrange
            var expectedCurrency = new Currency
            {
                Id = 1,
                Name = "USD"
            };
            var currencyList = new List<Currency> { expectedCurrency };

            _currencyMock.Setup(repo => repo.GetAll()).Returns(currencyList);
            _mapperMock.Setup(mapper => mapper.Map<CurrencyModel>(expectedCurrency))
                      .Returns(new CurrencyModel { Id = expectedCurrency.Id, Name = expectedCurrency.Name });

            // Act
            var result = _currencyService.GetByName("USD");

            // Assert
            Assert.Equal(expectedCurrency.Id, result.Id);
            Assert.Equal(expectedCurrency.Name, result.Name);
        }

        [Fact]
        public void Delete_WithValidCurrencyId_CallsDeleteMethodOfCurrencyRepository()
        {
            // Arrange
            var currencyId = 1;

            // Act
            _currencyService.Delete(currencyId);

            // Assert
            _currencyMock.Verify(c => c.Delete(currencyId), Times.Once);
        }

        [Fact]
        public void GetByIdWithCurrencyReturnsExpectedResult()
        {
            // Arrange
            var currencyId = 1;
            var currency = new Currency { Id = currencyId, Name = "USD" };
            var currencyModel = new CurrencyModel { Id = currencyId, Name = "USD" };
            _currencyMock.Setup(c => c.GetById(currencyId)).Returns(currency);
            _mapperMock.Setup(m => m.Map<CurrencyModel>(currency)).Returns(currencyModel);

            // Act
            var result = _currencyService.GetById(currencyId);

            // Assert
            Assert.Equal(currencyModel, result);
            _currencyMock.Verify(c => c.GetById(currencyId), Times.Once);
            _mapperMock.Verify(m => m.Map<CurrencyModel>(currency), Times.Once);
        }
    }
}

