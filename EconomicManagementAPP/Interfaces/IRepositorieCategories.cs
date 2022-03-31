using EconomicManagementAPP.Models;

public interface IRepositorieCategories
{
    Task Create(Categories categories);

    Task<bool> Exist(string Name);

    Task<IEnumerable<Categories>> GetCategories(int userId);

    Task<IEnumerable<Categories>> GetCategories(int userId, OperationTypes operationTypes);

    Task Modify(Categories categories);

    Task<Categories> GetCategorieById(int id, int userId);

    Task Delete(int id);

}
