using EconomicManagementAPP.Models;

namespace EconomicManagementAPP.Repositories
{
    public interface IRepositorieTransactions
    {
        Task Create(Transactions transactions);

        Task ModifyTransaction(Transactions transactions,
                               decimal previousAmount,
                               int previousAccount);

        Task Delete(int id);

        Task<Transactions> GetTransactionById(int id, int userId);
    }
}
