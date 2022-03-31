using EconomicManagementAPP.Models;
using EconomicManagementAPP.Repositories;
using Microsoft.Data.SqlClient;
using Dapper;

namespace EconomicManagementAPP.Services
{
    public class RepositorieTransactions : IRepositorieTransactions
    {
        private readonly string connectionString;
        public RepositorieTransactions(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Create(Transactions transactions)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>
                (@"Transactions_Insert",
                            new
                            {
                                transactions.UserId,
                                transactions.TransactionDate,
                                transactions.Total,
                                transactions.CategoryId,
                                transactions.AccountId,
                                transactions.Description
                            },
                              commandType: System.Data.CommandType.StoredProcedure);

            transactions.Id = id;
        }



        public async Task<Transactions> GetTransactionById(int id, int userId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Transactions>(@"
                                                                SELECT Transactions.*, cat.OperationTypeId
                                                                FROM Transactions
                                                                INNER JOIN Categories cat
                                                                ON cat.Id = Transactions.CategoryId
                                                                WHERE Transactions.Id = @Id AND Transactions.UserId = @UserId",
                                                                new { id, userId });

        }

        public async Task ModifyTransaction(Transactions transactions,
                                            decimal previousTotal,
                                            int previousAccountId)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("Transactions_Update",
            new
            {
                transactions.Id,
                transactions.TransactionDate,
                transactions.Total,
                transactions.CategoryId,
                transactions.AccountId,
                transactions.Description,
                previousTotal,
                previousAccountId
            }, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task Delete(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("Transactions_Delete",
                                          new { id }, commandType: System.Data.CommandType.StoredProcedure);
        }

    }
}
