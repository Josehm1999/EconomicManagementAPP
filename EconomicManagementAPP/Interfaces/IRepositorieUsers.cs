
using EconomicManagementAPP.Models;

public interface IRepositorieUsers
{
    Task Create(Users users);

    Task<bool> Exist(string Email);

    Task<IEnumerable<Users>> getUsers();

    Task Modify(Users users);

    Task<Users> getAccountById(int id);

    Task Delete(int id);

    Task<Users> Login(string email, string password);

    int GetUserId();

}
