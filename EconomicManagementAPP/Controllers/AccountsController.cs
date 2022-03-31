using EconomicManagementAPP.Models;
using EconomicManagementAPP.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EconomicManagementAPP.Controllers
{
    public class AccountsController : Controller
    {
        private IRepositorieAccounts repositorieAccounts;
        private IRepositorieUsers repositorieUsers;
        private IRepositorieAccountTypes repositorieAccountTypes;

        public AccountsController(IRepositorieAccounts repositorieAccounts,
                                  IRepositorieUsers repositorieUsers,
                                  IRepositorieAccountTypes repositorieAccountTypes)
        {
            this.repositorieAccounts = repositorieAccounts;
            this.repositorieUsers = repositorieUsers;
            this.repositorieAccountTypes = repositorieAccountTypes;
        }

        public async Task<ActionResult> Index()
        {
            var accounts = await repositorieAccounts.GetAccounts();
            return View(accounts);
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            // var userId = repositorieUsers.GetUserId();
            // var accountType = await repositorieAccountTypes.
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Accounts accounts)
        {
            if (!ModelState.IsValid)
            {
                return View(accounts);
            }
            var accountsExist =
               await repositorieAccounts.Exist(accounts.Name, accounts.AccountTypeId);
            if (accountsExist)
            {
                ModelState.AddModelError(nameof(accounts.Name),
                    $"The account {accounts.Name} already exist.");
                return View(accounts);
            }
            await repositorieAccounts.Create(accounts);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Modify(int Id)
        {
            var userId = repositorieUsers.GetUserId();
            var account = await repositorieAccounts.GetAccountById(Id, userId);
            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            return View(account);
        }

        [HttpPost]
        public async Task<ActionResult> Modify(Accounts accounts)
        {
            var userId = repositorieUsers.GetUserId();
            var account = await repositorieAccounts.GetAccountById(accounts.Id, userId);

            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            await repositorieAccounts.Modify(accounts);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {

            var userId = repositorieUsers.GetUserId();
            var account = await repositorieAccounts.GetAccountById(Id, userId);
            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            return View(account);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAccount(int Id)
        {
            var userId = repositorieUsers.GetUserId();
            var account = await repositorieAccounts.GetAccountById(Id, userId);
            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            await repositorieAccounts.Delete(Id);
            return RedirectToAction("Index");
        }
    }
}
