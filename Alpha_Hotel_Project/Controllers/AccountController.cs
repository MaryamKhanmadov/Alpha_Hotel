using Alpha_Hotel_Project.Data;
using Alpha_Hotel_Project.Models;
using Alpha_Hotel_Project.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alpha_Hotel_Project.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _appDbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, AppDbContext appDbContext, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(MemberRegisterViewModel memberRegisterVM)
        {
            if (!ModelState.IsValid) return View();

            AppUser appUser = null;

            appUser = await _userManager.FindByNameAsync(memberRegisterVM.Username);

            if (appUser != null)
            {
                ModelState.AddModelError("Username", "Already exist!");
                return View();
            }

            appUser = _appDbContext.Users.FirstOrDefault(x => x.NormalizedEmail == memberRegisterVM.Email.ToUpper());

            if (appUser != null)
            {
                ModelState.AddModelError("Email", "Already exist!");
                return View();
            }

            appUser = new AppUser
            {
                Fullname = memberRegisterVM.Fullname,
                Email = memberRegisterVM.Email,
                UserName = memberRegisterVM.Username,
            };

            var result = await _userManager.CreateAsync(appUser, memberRegisterVM.Password);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            await _userManager.AddToRoleAsync(appUser, "Member");

            await _signInManager.SignInAsync(appUser, isPersistent: false);

            return RedirectToAction("Index", "Home");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(MemberLoginViewModel memberLoginVM)
        {
            if (!ModelState.IsValid) return View();
            AppUser appUser = await _userManager.FindByNameAsync(memberLoginVM.Username);
            if (appUser is null)
            {
                ModelState.AddModelError("", "Username or password is incorrect");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(appUser, memberLoginVM.Password, false, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or password is incorrect");
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("login","account");
        }
    }

}
