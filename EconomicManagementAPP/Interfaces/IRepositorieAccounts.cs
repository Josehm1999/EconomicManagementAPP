using EconomicManagementAPP.Models;

namespace EconomicManagementAPP.Repositories
{
    public interface IRepositorieAccounts
    {
        Task Create(Accounts accounts);

        Task<bool> Exist(string Name, int Id);

        Task<IEnumerable<Accounts>> GetAccounts();

        Task<Accounts> GetAccountById(int Id, int userId);

        Task<IEnumerable<Accounts>> GetUserAccounts(int id);

        Task Modify(Accounts accounts);

        Task Delete(int Id);

    }
}
