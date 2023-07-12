using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using sports_store_files.Models;

namespace sports_store_files.Controllers {
    //[Authorize]
    public class AdminController : Controller {
        private IProductRepository repository;
        private SignInManager<IdentityUser> signInManager;

        public AdminController(IProductRepository repo, SignInManager<IdentityUser> signInMgr) {
            repository = repo;
            signInManager = signInMgr;
        }
        [Route("/Admin/Index")]
        public ActionResult Index() {
            if (User.Identity != null && User.Identity.IsAuthenticated) {
                return View(repository.Products);
            }
            return RedirectToAction("Login", "Account");
        } 
        public ViewResult Edit(int productId) => View(repository.Products.FirstOrDefault(p => p.ProductID == productId));

        [HttpPost]
        public IActionResult Edit(Product product) {
            if (ModelState.IsValid) {
                repository.SaveProduct(product);
                TempData["message"] = $"{product.Name} has been saved";
                return RedirectToAction("Index");
            } else {
                // there is something wrong with the data values
                return View(product);
            }
        }
        public ViewResult Create() => View("Edit", new Product());

        [HttpPost]
        public IActionResult Delete(int productId) {
            Product deletedProduct = repository.DeleteProduct(productId);
            if (deletedProduct != null) {
                TempData["message"] = $"{deletedProduct.Name} was deleted";
            }
            return RedirectToAction("Index");
        }
        public async Task<ActionResult> Logout(string returnUrl = "/") {
            await signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }
    }
}
