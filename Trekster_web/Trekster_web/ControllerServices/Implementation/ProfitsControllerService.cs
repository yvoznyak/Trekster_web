using BusinessLogic.Services.ServiceInterfaces;
using Trekster_web.ControllerServices.Interfaces;

namespace Trekster_web.ControllerServices.Implementation
{
    public class ProfitsControllerService : IProfitsControllerService
    {
        private readonly ICurrencyService _currencies;
        private readonly ITransactionService _transaction;
        private readonly ICategoryService _category;
        private readonly ILogger<ProfitsControllerService> _logger;

        public ProfitsControllerService(
            ICurrencyService currencyService,
            ITransactionService transaction,
            ICategoryService category,
            ILogger<ProfitsControllerService> logger
        )
        {
            _currencies = currencyService;
            _transaction = transaction;
            _category = category;
            _logger = logger;
        }

        public string GetSummary()
        {
            var dct = new Dictionary<int, double>();
            var currencies = _currencies.GetAll();
            foreach (var cur in currencies)
            {
                dct.Add(cur.Id, 0);
            }

            var transactions = _transaction.GetAllForUser();
            foreach (var trans in transactions)
            {
                if (_category.GetById(trans.CategoryId).Type == 1)
                {
                    dct[trans.CurrencyId] += _transaction.GetFinalSum(trans.Id);
                }
            }

            var res = "Підсумок: ";
            foreach (var elem in dct)
            {
                if (elem.Value != 0)
                {
                    var currencyName = _currencies.GetById(elem.Key).Name;
                    res += $"{elem.Value} {currencyName}, ";
                }
            }

            res = res.Remove(res.Length - 2);
            _logger.LogInformation($"Summary={res}");
            return res;
        }

        public List<string> GetProfitsByCategory()
        {
            var list = new List<string>();
            var transactions = _transaction.GetAllForUser();
            var dict = new Dictionary<int, Dictionary<int, double>>();
            var currencies = _currencies.GetAll();

            foreach (var transaction in transactions)
            {
                if (!dict.ContainsKey(transaction.CategoryId) && _category.GetById(transaction.CategoryId).Type == 1)
                {
                    var dictCurencies = new Dictionary<int, double>();
                    foreach (var cur in currencies)
                    {
                        dictCurencies.Add(cur.Id, 0);
                    }

                    dict.Add(transaction.CategoryId, dictCurencies);
                }
            }

            foreach (var transaction in transactions)
            {
                if (_category.GetById(transaction.CategoryId).Type == 1)
                {
                    dict[transaction.CategoryId][transaction.CurrencyId] += transaction.Sum;
                }
            }

            foreach (var elem in dict)
            {
                var res = $"{_category.GetById(elem.Key).Name}: ";

                foreach (var elem1 in elem.Value)
                {
                    if (elem1.Value != 0)
                    {
                        var currencyName = _currencies.GetById(elem1.Key).Name;
                        res += $"{elem1.Value} {currencyName}, ";
                    }
                }

                res = res.Remove(res.Length - 2);
                _logger.LogInformation($"Profit={res}, with category");
                list.Add(res);
            }

            return list;
        }
    }
}
