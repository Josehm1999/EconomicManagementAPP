using EconomicManagementAPP.Models;
using EconomicManagementAPP.Repositories;
using Microsoft.Data.SqlClient;
using Dapper;

namespace EconomicManagementAPP.Services
{
    public class RepositorieAccounts : IRepositorieAccounts
    {
        private readonly string connectionString;

        public RepositorieAccounts(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Create(Accounts accounts)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>
                ($@"INSERT INTO Accounts
                           (Name, AccountTypeId, Balance, Description)
                           VALUES(@Name, @AccountTypeId, ABS(@Balance), @Description);
                            SELECT SCOPE_IDENTITY();",
                            accounts);
            accounts.Id = id;
        }

        public async Task<bool> Exist(string Name, int Id)
        {
            using var connection = new SqlConnection(connectionString);
            var exist = await connection.QueryFirstOrDefaultAsync<int>
                ($@"SELECT 1
                        FROM Accounts WHERE Name = @Name AND Id=@Id;",
                        new { Name, Id });
            return exist == 1;
        }

        public async Task<IEnumerable<Accounts>> GetAccounts()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Accounts>(
                @"SELECT
                    Id, Name, AccountTypeId, Balance, Description
                    FROM Accounts;");
        }

        public async Task<Accounts> GetAccountById(int id, int userId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Accounts>(
                @"SELECT Accounts.Id, Accounts.Name, Balance, Description, at.Name AS AccountType
                FROM Accounts
                INNER JOIN AccountTypes at
                ON at.Id = Accounts.AccountTypeId
                WHERE at.UserId = @userId AND Accounts.Id = @id;", new { id, userId });
        }

        public async Task Modify(Accounts accounts)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Accounts
                        SET Name = @Name, AccountTypeId=@AccountTypeId, Balance=@Balance, Description=@Description
                        WHERE Id = @Id;", accounts);
        }

        public async Task Delete(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE Accounts WHERE Id = @Id", new { id });
        }

        public async Task<IEnumerable<Accounts>> GetUserAccounts(int userId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Accounts>(@"SELECT Accounts.Id, Accounts.Name, Balance, at.Name AS AccountType
                                  FROM Accounts
                                  INNER JOIN AccountTypes at
                                  ON at.Id = Accounts.AccountTypeId
                                  WHERE at.UserId = @UserId
                                  ORDER BY at.OrderAccount", new { userId });
        }
    }
}
