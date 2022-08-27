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

        public async Task<IEnumerable<Transactions>>  GetTransactionsByAccountId(GetTransactionByAccount model)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Transactions>(@"SELECT t.Id, t.Total, t.TransactionDate, c.Name as Categoria,
                                                            acc.Name as Account, c.OperationTypeId
                                                            FROM Transactions t
                                                            INNER JOIN Categories c
                                                            ON c.Id = t.CategoryId
                                                            INNER JOIN Accounts acc
                                                            ON acc.Id = t.AccountId
                                                            WHERE t.AccountId = @AccountId AND t.UserId = @UserId
                                                            AND TransactionDate BETWEEN @StartDate AND @EndDate", model);
        }

        public async Task ModifyTransaction(Transactions transactions,
                                            decimal PreviousTotal,
                                            int PreviousAccountId)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("Transactions_Update",
            new
            {
                transactions.Id,
                transactions.TransactionDate,
                transactions.Total,
                PreviousTotal,
                transactions.AccountId,
                PreviousAccountId,
                transactions.CategoryId,
                transactions.Description,
               
            }, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task Delete(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("Transaction_Delete",
                                          new { id }, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Transactions>> GetTransactionsByUser(int userId)
        {
            using var connections = new SqlConnection(connectionString);
            return await connections.QueryAsync<Transactions>(@"SELECT * FROM Transactions
                                                                WHERE UserId = @userId",
                                                                new { userId });
        }
    }
}
