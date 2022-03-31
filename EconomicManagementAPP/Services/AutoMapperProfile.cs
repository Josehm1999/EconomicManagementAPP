using AutoMapper;
using EconomicManagementAPP.Models;

namespace EconomicManagementAPP.Services
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<ModifyTransactionViewModel, Transactions>().ReverseMap();
        }

    }
}
