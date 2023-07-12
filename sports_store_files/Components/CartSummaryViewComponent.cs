using Microsoft.AspNetCore.Mvc;
using sports_store_files.Models;

namespace sports_store_files.Components {
	public class CartSummaryViewComponent : ViewComponent{
        private Cart cart;
        public CartSummaryViewComponent(Cart cartService) {
            cart = cartService;
        }
        public IViewComponentResult Invoke() {
            return View(cart);
        }
    }
}
