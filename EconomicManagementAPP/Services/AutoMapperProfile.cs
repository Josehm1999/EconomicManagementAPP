using AutoMapper;
using EconomicManagementAPP.Models;

namespace EconomicManagementAPP.Services
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Accounts, CreateAccountViewModel>();
            CreateMap<ModifyTransactionViewModel, Transactions>().ReverseMap();
        }

    }
}
