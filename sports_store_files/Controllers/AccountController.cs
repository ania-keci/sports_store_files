using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using sports_store_files.Models;
using sports_store_files.Models.ViewModels;

namespace sports_store_files.Controllers {
    [Authorize]
    public class AccountController : Controller {
        private UserManager<IdentityUser> userManager;
        private SignInManager<IdentityUser> signInManager; 
        public AccountController(UserManager<IdentityUser> userMgr,
        SignInManager<IdentityUser> signInMgr) {
            userManager = userMgr;
            signInManager = signInMgr;
        }
        [AllowAnonymous]
        public ViewResult Login(string returnUrl) {
            return View(new LoginModel {
                ReturnUrl = returnUrl
            });
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel) {
            if (ModelState.IsValid) {
                IdentityUser user =
                await userManager.FindByNameAsync(loginModel.Name);
                if (user != null) {
                    await signInManager.SignOutAsync();
                    if ((await signInManager.PasswordSignInAsync(user,
                    loginModel.Password, false, false)).Succeeded) {
                        await signInManager.SignInAsync(user, false);
                        return RedirectToAction("Index", "Admin");
                    }
                    return  RedirectToAction("Index", "Account");
                }
            }
            ModelState.AddModelError("", "Invalid name or password");
            return View(loginModel);
        }
        [Route("/Account/Logout")]
        public async Task<ActionResult> Logout(string returnUrl = "/") {
            await signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }

    }
}
