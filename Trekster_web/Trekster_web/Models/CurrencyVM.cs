using BusinessLogic.Models;
using Infrastructure.Entities;

namespace Trekster_web.Models
{
    public class CurrencyVM
    {
        public CurrencyModel CurrencyModel { get; set; } = new CurrencyModel();

        public IEnumerable<CurrencyModel> CurrencyModels { get; set; } = new List<CurrencyModel>();
    }
}