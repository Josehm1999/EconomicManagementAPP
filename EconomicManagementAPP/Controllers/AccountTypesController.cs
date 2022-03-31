using EconomicManagementAPP.Models;
using Microsoft.AspNetCore.Mvc;

namespace EconomicManagementAPP.Controllers
{
    public class AccountTypesController : Controller
    {
        private readonly IRepositorieAccountTypes repositorieAccountTypes;
        private readonly IRepositorieUsers repositorieUsers;

        public AccountTypesController(IRepositorieAccountTypes repositorieAccountTypes,
                                      IRepositorieUsers repositorieUsers)
        {
            this.repositorieAccountTypes = repositorieAccountTypes;
            this.repositorieUsers = repositorieUsers;
        }

        // Creamos index para ejecutar la interfaz
        public async Task<IActionResult> Index()
        {
            var userId = repositorieUsers.GetUserId();
            var accountTypes = await repositorieAccountTypes.GetAccounts(userId);
            return View(accountTypes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AccountTypes accountTypes)
        {
            if (!ModelState.IsValid)
            {
                return View(accountTypes);
            }

            var userId = repositorieUsers.GetUserId();
            accountTypes.UserId = userId;
            accountTypes.OrderAccount = 1;

            // Validamos si ya existe antes de registrar
            var accountTypeExist =
               await repositorieAccountTypes.Exist(accountTypes.Name, accountTypes.UserId);

            if (accountTypeExist)
            {
                // AddModelError ya viene predefinido en .net
                // nameOf es el tipo del campo
                ModelState.AddModelError(nameof(accountTypes.Name),
                    $"The account {accountTypes.Name} already exist.");

                return View(accountTypes);
            }
            await repositorieAccountTypes.Create(accountTypes);
            // Redireccionamos a la lista
            return RedirectToAction("Index");
        }

        // Hace que la validacion se active automaticamente desde el front
        [HttpGet]
        public async Task<IActionResult> VerifyAccountType(string name)
        {
            var userId = repositorieUsers.GetUserId();
            var accountTypeExist = await repositorieAccountTypes.Exist(name, userId);

            if (accountTypeExist)
            {
                // permite acciones directas entre front y back
                return Json($"The account {name} already exist");
            }

            return Json(true);
        }

        //Actualizar
        [HttpGet]
        public async Task<ActionResult> Modify(int id)
        {
            var userId = repositorieUsers.GetUserId();
            var accountType = await repositorieAccountTypes.GetAccountById(id, userId);

            if (accountType is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(accountType);
        }

        [HttpPost]
        public async Task<ActionResult> Modify(AccountTypes accountTypes)
        {
            var userId = repositorieUsers.GetUserId();
            var accountType = await repositorieAccountTypes.GetAccountById(accountTypes.Id, userId);

            if (accountType is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await repositorieAccountTypes.Modify(accountTypes);// el que llega
            return RedirectToAction("Index");
        }
        // Eliminar
        public async Task<IActionResult> Delete(int id)
        {
            var userId = repositorieUsers.GetUserId();
            var account = await repositorieAccountTypes.GetAccountById(id, userId);

            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(account);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var userId = repositorieUsers.GetUserId();
            var account = await repositorieAccountTypes.GetAccountById(id, userId);

            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await repositorieAccountTypes.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Sort([FromBody] int[] ids)
        {
            var userId = repositorieUsers.GetUserId();
            var accountTypes = await repositorieAccountTypes.GetAccounts(userId);
            var idsAccountTypes = accountTypes.Select(x => x.Id);

            var idsAccountTypesNotBelongToUser = ids.Except(idsAccountTypes).ToList();

            if (idsAccountTypesNotBelongToUser.Count > 0)
            {
                return Forbid();
            }

            var sortedAccountTypes = ids.Select((value, index) => new AccountTypes() { Id = value, OrderAccount = index + 1 }).AsEnumerable();

            await repositorieAccountTypes.Sort(sortedAccountTypes);

            return Ok();
        }
    }
}
