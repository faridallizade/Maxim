using Maxim.Helpers;
using Maxim.Models;
using Maxim.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Maxim.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public  IActionResult Register()
        {
            
            return View();
        }
        [HttpPost]
        public  async Task<IActionResult> Register(RegisterVm vm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser user = new AppUser()
            {
                Name = vm.Name,
                Surname = vm.Surname,
                UserName = vm.Username,
                Email = vm.Email
            };
            var result = await _userManager.CreateAsync(user,vm.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            
            return RedirectToAction("Index","Home");

        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVm vm)
        {
            var user =await _userManager.FindByEmailAsync(vm.UsernameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(vm.UsernameOrEmail);
                if (user == null) throw new Exception("Email Adress Or Username is incorrect");
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, vm.Password, false);
            if (!result.Succeeded) { throw new Exception("Email Adress Or Username is incorrect"); }
            await _signInManager.SignInAsync( user, false);
            return RedirectToAction("Index","Home");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }
        public async Task<IActionResult> CreateRole()
        {
            foreach (var role in Enum.GetNames(typeof(UserRole)))
            {
                if ((await _roleManager.FindByNameAsync(role.ToString()) == null))
                {
                    await _roleManager.CreateAsync(new IdentityRole()
                    {
                        Name = role.ToString(),
                    });
                }
            }
            return RedirectToAction("Index","Home");
        }
    }
}
