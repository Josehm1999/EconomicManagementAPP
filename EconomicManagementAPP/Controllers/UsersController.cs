using System.Security.Claims;
using EconomicManagementAPP.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EconomicManagementAPP.Controllers
{
    public class UsersController : Controller
    {

        private readonly IRepositorieUsers _repositorieUser;
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;

        public UsersController(IRepositorieUsers repositorieUser,
                              UserManager<Users> userManager,
                              SignInManager<Users> signInManager)
        {
            this._repositorieUser = repositorieUser;
            this._userManager = userManager;
            this._signInManager = signInManager;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var users = await _repositorieUser.getUsers();
            return View(users);
        }

        [AllowAnonymous]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]

        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            var result = await _signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, loginViewModel.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Accounts");
            }
            else
            {
                ModelState.AddModelError(String.Empty, "Wrong Email or Password");
                return View(loginViewModel);
            }

        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]

        [AllowAnonymous]
        public async Task<IActionResult> Create(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userExist = await _repositorieUser.Exist(model.Email);
            if (userExist)
            {
                ModelState.AddModelError(nameof(model.Email),
                    $"User with email {model.Email} already exist.");
                return View(model);
            }

            var user = new Users() { Email = model.Email };
            var result = await _userManager.CreateAsync(user, password: model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: true);
                return RedirectToAction("Index", "Accounts");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        //Actualizar
        [HttpGet]
        public async Task<ActionResult> Modify(int id)
        {
            var user = await _repositorieUser.getAccountById(id);

            if (user is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> Modify(Users users)
        {
            var user = await _repositorieUser.getAccountById(users.Id);

            if (user is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await _repositorieUser.Modify(users);// el que llega
            return RedirectToAction("Index");
        }

        // Delete User
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _repositorieUser.getAccountById(id);

            if (user is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _repositorieUser.getAccountById(id);

            if (user is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await _repositorieUser.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
