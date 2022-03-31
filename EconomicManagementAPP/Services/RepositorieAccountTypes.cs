using EconomicManagementAPP.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace EconomicManagementAPP.Services
{
    public class RepositorieAccountTypes : IRepositorieAccountTypes
    {
        private readonly string connectionString;

        public RepositorieAccountTypes(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Create(AccountTypes accountTypes)
        {
            using var connection = new SqlConnection(connectionString);
            // Requiere el await - tambien requiere el Async al final de la query
            var id = await connection.QuerySingleAsync<int>("AccountTypes_Insert",
                                                            new
                                                            {
                                                                userId = accountTypes.UserId,
                                                                name = accountTypes.Name
                                                            },
                                                            commandType: System.Data.CommandType.StoredProcedure);
            accountTypes.Id = id;
        }

        //Cuando retorna un tipo de dato se debe poner en el Task Task<bool>
        public async Task<bool> Exist(string name, int userId)
        {
            using var connection = new SqlConnection(connectionString);
            // El select 1 es traer lo primero que encuentre y el default es 0
            var exist = await connection.QueryFirstOrDefaultAsync<int>(
                                    @"SELECT 1
                                    FROM AccountTypes
                                    WHERE Name = @name AND UserId = @userId;",
                                    new { name, userId });
            return exist == 1;
        }

        // Obtenemos las cuentas del usuario
        public async Task<IEnumerable<AccountTypes>> GetAccounts(int userId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<AccountTypes>(@"SELECT Id, Name, OrderAccount
                                                            FROM AccountTypes
                                                            WHERE UserId = @userId
                                                            ORDER BY OrderAccount", new { userId });
        }

        // Actualizar
        public async Task Modify(AccountTypes accountTypes)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE AccountTypes
                                            SET Name = @Name
                                            WHERE Id = @Id", accountTypes);
        }

        //Para actualizar se necesita obtener el tipo de cuenta por el id
        public async Task<AccountTypes> GetAccountById(int id, int userId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<AccountTypes>(@"
                                                                SELECT Id, Name, UserId, OrderAccount
                                                                FROM AccountTypes
                                                                WHERE Id = @id AND UserID = @userID",
                                                                new { id, userId });
        }

        //Eliminar
        public async Task Delete(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE AccountTypes WHERE Id = @Id", new { id });
        }

        public async Task Sort(IEnumerable<AccountTypes> accountTypesSorted)
        {
            var query = "UPDATE AccountTypes SET OrderAccount = @OrderAccount WHERE Id = @Id;";

            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(query, accountTypesSorted);
        }
    }
}
