using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Divisas.Utils
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DataAccess.Entities.Currency, Models.Currency>()
                .ForMember(dest => dest.ActualRate, opt => opt.Ignore())
                .ForMember(dest => dest.SuggestedRetailPrice, opt => opt.Ignore());

            CreateMap<DataAccess.Entities.ExchangeRateHistory, Models.ExchangeRateHistory>();
        }
    }
}
