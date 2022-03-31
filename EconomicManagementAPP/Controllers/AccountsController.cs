using EconomicManagementAPP.Models;
using EconomicManagementAPP.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EconomicManagementAPP.Controllers
{
    public class AccountsController : Controller
    {
        private IRepositorieAccounts _repositorieAccounts;
        private IRepositorieUsers _repositorieUsers;
        private IRepositorieAccountTypes _repositorieAccountTypes;

        public AccountsController(IRepositorieAccounts repositorieAccounts,
                                  IRepositorieUsers repositorieUsers,
                                  IRepositorieAccountTypes repositorieAccountTypes)
        {
            this._repositorieAccounts = repositorieAccounts;
            this._repositorieUsers = repositorieUsers;
            this._repositorieAccountTypes = repositorieAccountTypes;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _repositorieUsers.GetUserId();
            var accounts = await _repositorieAccounts.GetUserAccounts(userId);

            var model = accounts
                        .GroupBy(x => x.AccountType)
                        .Select(group => new IndexAccountViewModel
                        {
                            AccounType = group.Key,
                            Accounts = group.AsEnumerable()
                        }).ToList();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var userId = _repositorieUsers.GetUserId();
            var model = new CreateAccountViewModel();
            model.AccountTypes = await GetAccountTypes(userId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAccountViewModel accounts)
        {
            var userId = _repositorieUsers.GetUserId();
            var accountType = await _repositorieAccountTypes.GetAccountById(accounts.AccountTypeId, userId);
           
            if(accountType is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            
            if (!ModelState.IsValid)
            {
                accounts.AccountTypes = await GetAccountTypes(userId); 
                return View(accounts);
            }
            
            await _repositorieAccounts.Create(accounts);
            return RedirectToAction("Index");
        }
        
        private async Task<IEnumerable<SelectListItem>> GetAccountTypes(int userId)
        {
            var accountTypes = await _repositorieAccountTypes.GetAccounts(userId);
            return accountTypes.Select(x => new SelectListItem(x.Name, x.Id.ToString()));
        }
        public async Task<IActionResult> Modify(int Id)
        {
            var userId = _repositorieUsers.GetUserId();
            var account = await _repositorieAccounts.GetAccountById(Id, userId);
            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            return View(account);
        }

        [HttpPost]
        public async Task<IActionResult> Modify(Accounts accounts)
        {
            var userId = _repositorieUsers.GetUserId();
            var account = await _repositorieAccounts.GetAccountById(accounts.Id, userId);

            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            await _repositorieAccounts.Modify(accounts);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {

            var userId = _repositorieUsers.GetUserId();
            var account = await _repositorieAccounts.GetAccountById(Id, userId);
            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            return View(account);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAccount(int Id)
        {
            var userId = _repositorieUsers.GetUserId();
            var account = await _repositorieAccounts.GetAccountById(Id, userId);
            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            await _repositorieAccounts.Delete(Id);
            return RedirectToAction("Index");
        }

    }
}
