using AutoMapper;
using BusinessLogic.Models;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Mapping.Profiles
{
    public class StartBalanceProfile : Profile
    {
        public StartBalanceProfile()
        {
            CreateMap<StartBalance, StartBalanceModel>().ReverseMap();
        }
    }
}
