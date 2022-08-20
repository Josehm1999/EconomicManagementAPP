using AutoMapper;
using EconomicManagementAPP.Models;
using EconomicManagementAPP.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EconomicManagementAPP.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly IRepositorieUsers repositorieUsers;
        private readonly IRepositorieAccounts repositorieAccounts;
        private readonly IRepositorieTransactions repositorieTransactions;
        private readonly IRepositorieCategories repositorieCategories;
        private readonly IMapper mapper;

        public TransactionsController(IRepositorieUsers repositorieUsers,
                                      IRepositorieAccounts repositorieAccounts,
                                      IRepositorieCategories repositorieCategories,
                                      IRepositorieTransactions repositorieTransactions,
                                      IMapper mapper)
        {
            this.repositorieUsers = repositorieUsers;
            this.repositorieAccounts = repositorieAccounts;
            this.repositorieTransactions = repositorieTransactions;
            this.repositorieCategories = repositorieCategories;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var userId = repositorieUsers.GetUserId();
            var transactions = await repositorieTransactions.GetTransactionsByUser(userId);
            return View(transactions);
        }

        public async Task<IActionResult> Create()
        {
            var userId = repositorieUsers.GetUserId();
            var model = new CreateTransactionViewModel();
            model.Accounts = await GetAccounts(userId);
            model.Categories = await GetCategories(userId, model.OperationTypesId);
            return View(model);
        }

        private async Task<IEnumerable<SelectListItem>> GetAccounts(int userId)
        {
            var accounts = await repositorieAccounts.GetUserAccounts(userId);
            return accounts.Select(x => new SelectListItem(x.Name, x.Id.ToString()));
        }

        private async Task<IEnumerable<SelectListItem>> GetCategories(int userId, OperationTypes operationTypes)
        {
            var categories = await repositorieCategories.GetCategories(userId, operationTypes);
            return categories.Select(x => new SelectListItem(x.Name, x.Id.ToString()));
        }

        [HttpPost]
        public async Task<IActionResult> GetCategories([FromBody] OperationTypes operationTypes)
        {
            var userId = repositorieUsers.GetUserId();
            var categories = await GetCategories(userId, operationTypes);
            return Ok(categories);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTransactionViewModel model)
        {
            var userId = repositorieUsers.GetUserId();

            if (!ModelState.IsValid)
            {
                model.Accounts = await GetAccounts(userId);
                model.Categories = await GetCategories(userId, model.OperationTypesId);
                return View(model);
            }
            var account = await repositorieAccounts.GetAccountById(model.AccountId, userId);

            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            var category = await repositorieCategories.GetCategorieById(model.CategoryId, userId);

            if (category is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            model.UserId = userId;

            if (model.OperationTypesId == OperationTypes.Expenses)
            {
                model.Total *= -1;
            }

            await repositorieTransactions.Create(model);
            return RedirectToAction("Index");
        }

        //Actualizar
        [HttpGet]
        public async Task<IActionResult> Modify(int id)
        {
            var userId = repositorieUsers.GetUserId();
            var transactions = await repositorieTransactions.GetTransactionById(id, userId);

            if (transactions is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            var model = mapper.Map<ModifyTransactionViewModel>(transactions);

            if (model.OperationTypesId == OperationTypes.Expenses)
            {
                model.PreviousTotal = model.Total * -1;
            }

            model.PreviousAccountId = transactions.AccountId;
            model.Categories = await GetCategories(userId, transactions.OperationTypesId);
            model.Accounts = await GetAccounts(userId);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Modify(ModifyTransactionViewModel model)
        {
            var userId = repositorieUsers.GetUserId();

            if (!ModelState.IsValid)
            {
                model.Accounts = await GetAccounts(userId);
                model.Categories = await GetCategories(userId, model.OperationTypesId);
                return View(model);
            }

            var account = await repositorieAccounts.GetAccountById(model.AccountId, userId);

            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            var category = await repositorieCategories.GetCategorieById(model.CategoryId, userId);

            if (category is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            var transaction = mapper.Map<Transactions>(model);

            model.PreviousTotal = model.Total;
            if (model.OperationTypesId == OperationTypes.Expenses)
            {
                transaction.Total *= -1;
            }

            await repositorieTransactions.ModifyTransaction(transaction,
                                                            model.PreviousTotal,
                                                            model.PreviousAccountId);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var userId = repositorieUsers.GetUserId();
            var transaction = await repositorieTransactions.GetTransactionById(id, userId);

            if (transaction is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(transaction);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTransaction(int id)
        {

            var userId = repositorieUsers.GetUserId();
            var transaction = await repositorieTransactions.GetTransactionById(id, userId);

            if (transaction is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await repositorieTransactions.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
