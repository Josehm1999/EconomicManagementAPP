using System.Security.Claims;
using Dapper;
using EconomicManagementAPP.Models;
using Microsoft.Data.SqlClient;

namespace EconomicManagementAPP.Services
{

    public class RepositorieUser : IRepositorieUsers
    {
        private readonly string connectionString;

        public IConfiguration Configuration { get; }
        private readonly HttpContext _httpContext;

        public RepositorieUser(IConfiguration configuration, IHttpContextAccessor httpContext)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
            Configuration = configuration;
            this._httpContext = httpContext.HttpContext;
        }


        public int GetUserId()
        {
            if (_httpContext.User.Identity.IsAuthenticated)
            {
                //Only if user is authnticated
                var claims = _httpContext.User.Claims.ToList();
                var userId = claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
                var id = int.Parse(userId.Value);
                return id;
            }
            else
            {
                throw new ApplicationException("User is not authenticated");
            }
        }

        public async Task<int> Create(Users users)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>($@"INSERT INTO Users
                                                        (Email, StandarEmail,Password)
                                                        VALUES(@Email, @StandarEmail, @Password); SELECT SCOPE_IDENTITY();",
                                                                users);
            return id;
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

        public async Task<Users> GetUserByEmail(string standarEmail)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Users>(
                "SELECT * FROM Users WHERE standarEmail = @standarEmail",
                new { standarEmail });
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
            await connection.ExecuteAsync("Users_Delete",
                                          new { id }, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<Users> Login(string email, string password)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Users>(@"SELECT * FROM Users WHERE Email = @Email
                                                            AND Password = @Password", new { email, password });
        }
    }
}
