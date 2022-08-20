using EconomicManagementAPP.Models;
using Microsoft.AspNetCore.Mvc;

namespace EconomicManagementAPP.Controllers
{
    public class UsersController : Controller
    {

        private readonly IRepositorieUsers repositorieUser;

        public UsersController(IRepositorieUsers repositorieUser)
        {
            this.repositorieUser = repositorieUser;
        }

        public async Task<IActionResult> Index()
        {
            var users = await repositorieUser.getUsers();
            return View(users);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            var result = await repositorieUser.Login(loginViewModel.Email, loginViewModel.Password);

            if (result is null)
            {
                ModelState.AddModelError(String.Empty, "Wrong Email or Password");
                return View(loginViewModel);
            }
            else
            {
                return RedirectToAction("Index", "AccountTypes");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(Users users)
        {
            if (!ModelState.IsValid)
            {
                return View(users);
            }
            var userExist = await repositorieUser.Exist(users.Email);
            if (userExist)
            {
                ModelState.AddModelError(nameof(users.Email),
                    $"User with email {users.Email} already exist.");
                return View(users);
            }
            if (users.Email == users.StandarEmail)
            {
                ModelState.AddModelError(nameof(users.Email),
                    $"User with email {users.Email} and {users.StandarEmail} are equals.");
                return View(users);
            }
            await repositorieUser.Create(users);
            return RedirectToAction("Index");
        }

        //Actualizar
        [HttpGet]
        public async Task<ActionResult> Modify(int id)
        {
            var user = await repositorieUser.getAccountById(id);

            if (user is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> Modify(Users users)
        {
            var user = await repositorieUser.getAccountById(users.Id);

            if (user is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await repositorieUser.Modify(users);// el que llega
            return RedirectToAction("Index");
        }

        // Delete User
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await repositorieUser.getAccountById(id);

            if (user is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await repositorieUser.getAccountById(id);

            if (user is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await repositorieUser.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
