using System;
using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using BusinessLogic.Services.ServiceImplementation;
using BusinessLogic.Services.ServiceInterfaces;
using Infrastructure.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Trekster_web.ControllerServices.Implementation;

namespace TreksterTests
{
    public class StartBalanceServiceTests
    {
        private readonly IMapper _mapper;
        private readonly IStartBalanceService _startBalanceService;
        private readonly Mock<ILogger<StartBalanceService>> _loggerMock;

        public StartBalanceServiceTests()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<StartBalance, StartBalanceModel>();
                cfg.CreateMap<StartBalanceModel, StartBalance>();
            });
            _mapper = mapperConfig.CreateMapper();

            var startBalances = new List<StartBalance>
        {
            new StartBalance { Id = 1, Sum = 100.0, AccountId = 1, CurrencyId = 1 },
            new StartBalance { Id = 2, Sum = 200.0, AccountId = 1, CurrencyId = 2 },
            new StartBalance { Id = 3, Sum = 300.0, AccountId = 2, CurrencyId = 1 },
            new StartBalance { Id = 4, Sum = 400.0, AccountId = 2, CurrencyId = 2 },
        };
            var startBalanceRepository = new Mock<IStartBalance>();
            startBalanceRepository.Setup(r => r.GetAll()).Returns(startBalances);
            startBalanceRepository.Setup(r => r.GetById(It.IsAny<int>())).Returns<int>(id => startBalances.FirstOrDefault(sb => sb.Id == id));
            startBalanceRepository.Setup(r => r.Save(It.IsAny<StartBalance>())).Callback<StartBalance>(sb =>
            {
                if (sb.Id == 0)
                {
                    sb.Id = startBalances.Max(x => x.Id) + 1;
                    startBalances.Add(sb);
                }
                else
                {
                    var index = startBalances.FindIndex(x => x.Id == sb.Id);
                    startBalances[index] = sb;
                }
            });
            startBalanceRepository.Setup(r => r.Delete(It.IsAny<int>())).Callback<int>(id =>
            {
                var sb = startBalances.FirstOrDefault(x => x.Id == id);
                if (sb != null)
                {
                    startBalances.Remove(sb);
                }
            });
            _loggerMock = new Mock<ILogger<StartBalanceService>>();
            _startBalanceService = new StartBalanceService(startBalanceRepository.Object, _mapper, _loggerMock.Object);
        }

        [Fact]
        public void GetAll_ReturnsAllStartBalances()
        {
            // Arrange

            // Act
            var result = _startBalanceService.GetAll();

            // Assert
            Assert.Equal(4, result.Count());
        }

        [Fact]
        public void GetAllForAccount_WithNonExistingAccount_ReturnsEmptyList()
        {
            // Arrange
            var accountModel = new AccountModel { Id = 3 };

            // Act
            var result = _startBalanceService.GetAllForAccount(accountModel);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetById_WithExistingStartBalanceId_ReturnsStartBalance()
        {
            // Arrange
            var startBalanceId = 1;

            // Act
            var result = _startBalanceService.GetById(startBalanceId);

            // Assert
            Assert.Equal(startBalanceId, result.Id);
        }

    }

}

