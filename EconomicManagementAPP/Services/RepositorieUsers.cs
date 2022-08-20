using Dapper;
using EconomicManagementAPP.Models;
using Microsoft.Data.SqlClient;

namespace EconomicManagementAPP.Services
{

    public class RepositorieUser : IRepositorieUsers
    {
        private readonly string connectionString;

        public RepositorieUser(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        public int GetUserId()
        {
            return 1;
        }

        public async Task Create(Users users)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>($@"INSERT INTO Users
                                                        (Email, StandarEmail,Password)
                                                        VALUES(@Email, @StandarEmail, @Password); SELECT SCOPE_IDENTITY();",
                                                                users);
            users.Id = id;
        }


        public async Task<bool> Exist(string Email)
        {
            using var connection = new SqlConnection(connectionString);

            var exist = await connection.QueryFirstOrDefaultAsync<int>(@"SELECT 1
                                                                      FROM Users
                                                                      WHERE Email = @Email;",
                                                                      new { Email });
            return exist == 1;
        }



        public async Task<Users> getAccountById(int id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Users>(@"SELECT Id, Email, StandarEmail
                                                                    FROM Users
                                                                    WHERE Id = @Id",
                                                                    new { id });
        }

        public async Task<IEnumerable<Users>> getUsers()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Users>(@"SELECT Id, Email, StandarEmail
                                                    FROM Users;");
        }

        public async Task Modify(Users users)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Users
                                            SET StandarEmail = @StandarEmail,
                                            Password = @Password
                                            WHERE Id = @Id", users);
        }

        public async Task Delete(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE Users WHERE Id = @Id", new { id });
        }

        public async Task<Users> Login(string email, string password)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Users>(@"SELECT * FROM Users WHERE Email = @Email
                                                            AND Password = @Password", new { email, password });
        }
    }
}
