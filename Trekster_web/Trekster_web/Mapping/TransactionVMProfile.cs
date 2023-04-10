using AutoMapper;
using BusinessLogic.Models;
using Trekster_web.Models;

namespace Trekster_web.Mapping
{
    public class TransactionVMProfile : Profile
    {
        public TransactionVMProfile()
        {
            CreateMap<TransactionModel, TransactionVM>().ReverseMap();
        }
    }
}
