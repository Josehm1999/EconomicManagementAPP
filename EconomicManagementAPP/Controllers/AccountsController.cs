using AutoMapper;
using EconomicManagementAPP.Models;
using EconomicManagementAPP.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EconomicManagementAPP.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IRepositorieAccounts _repositorieAccounts;
        private readonly IRepositorieUsers _repositorieUsers;
        private readonly IRepositorieAccountTypes _repositorieAccountTypes;
        private readonly IMapper _mapper;
        private readonly IRepositorieTransactions _repositorieTransactions;

        public AccountsController(IRepositorieAccounts repositorieAccounts,
                                  IRepositorieUsers repositorieUsers,
                                  IRepositorieAccountTypes repositorieAccountTypes,
                                  IMapper mapper,
                                  IRepositorieTransactions repositorieTransactions)
        {
            this._repositorieAccounts = repositorieAccounts;
            this._repositorieUsers = repositorieUsers;
            this._repositorieAccountTypes = repositorieAccountTypes;
            this._repositorieTransactions = repositorieTransactions;
            this._mapper = mapper;
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


        public async Task<IActionResult> Detail(int id, int month, int year)
        {
            var userId = _repositorieUsers.GetUserId();
            var account = await _repositorieAccounts.GetAccountById(id, userId);

            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            DateTime startDate;
            DateTime endDate;

            if (month <= 0 || month > 12 || year <= 1900)
            {
                var today = DateTime.Today;
                startDate = new DateTime(today.Year, today.Month, 1);
            }
            else
            {
                startDate = new DateTime(year, month, 1);
            }
            endDate = startDate.AddMonths(1).AddDays(-1);

            var _getTransactionsByAccount = new GetTransactionByAccount()
            {
                AccountId = id,
                UserId = userId,
                StartDate = startDate,
                EndDate = endDate,
            };
             
            var transactionns = await _repositorieTransactions.GetTransactionsByAccountId(_getTransactionsByAccount);
            var model = new DetailedTransactionsReport();
            ViewBag.Account = account.Name;

            var transactionsByDate = transactionns.OrderByDescending(x => x.TransactionDate)
                      .GroupBy(x => x.TransactionDate)
                      .Select(group => new DetailedTransactionsReport.TransactionsByDate()
                      {
                          TransactionDate = group.Key,
                          Transactions = group.AsEnumerable()
                      });
            
            model.GroupedTransactions = transactionsByDate;
            model.StartDate = startDate;
            model.EndDate = endDate;

            ViewBag.lastMonth = startDate.AddMonths(-1).Month;
            ViewBag.lastYear = startDate.AddMonths(-1).Year;

            ViewBag.lastMonth = startDate.AddMonths(+1).Month;
            ViewBag.nextYear = startDate.AddMonths(+1).Year;
           
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

            if (accountType is null)
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

            var model = _mapper.Map<CreateAccountViewModel>(account);
            model.AccountTypes = await GetAccountTypes(userId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Modify(CreateAccountViewModel model)
        {
            var userId = _repositorieUsers.GetUserId();
            var account = await _repositorieAccounts.GetAccountById(model.Id, userId);

            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            var accountType = await _repositorieAccountTypes.GetAccountById(model.AccountTypeId, userId);
            if (accountType is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            await _repositorieAccounts.Modify(model);
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
