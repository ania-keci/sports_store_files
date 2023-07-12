using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sports_store_files.Models;
using sports_store_files.Models.ViewModels;

namespace sports_store_files.Controllers {
	public class OrderController : Controller {
        private IOrderRepository repository;
        private Cart cart;
        public OrderController(IOrderRepository repoService, Cart cartService) {
            repository = repoService;
            cart = cartService;
        }
        [Authorize]
        public ViewResult List() =>
        View(repository.Orders.Where(o => !o.Shipped));
        [HttpPost]
        [Authorize]
        public IActionResult MarkShipped(int orderID) {
            Order order = repository.Orders.FirstOrDefault(o => o.OrderID == orderID);
            if (order != null) {
                order.Shipped = true;
                repository.SaveOrder(order);
            }
            return RedirectToAction(nameof(List));
        }
        public ViewResult Checkout() => View(new Order());
        [HttpPost]
        [HttpPost]
        //public IActionResult Checkout(Order order) {
        //    if (cart.Lines.Count() == 0) {
        //        ModelState.AddModelError("", "Sorry, your cart is empty!");
        //    }

        //    if (ModelState.IsValid) {
        //        order.Lines = cart.Lines.ToList(); // Assign the cart lines to the order

        //        repository.SaveOrder(order);
        //        return RedirectToAction(nameof(Completed));
        //    } else {
        //        return View(order);
        //    }
        //}
        [HttpPost]
        public IActionResult Checkout(OrderViewModel orderViewModel) {
            if (cart.Lines.Count() == 0) {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }
            if (ModelState.IsValid) {
                var order = new Order {
                    OrderID = orderViewModel.OrderID,
                    Shipped = orderViewModel.Shipped,
                    Name = orderViewModel.Name,
                    Line1 = orderViewModel.Line1,
                    Line2 = orderViewModel.Line2,
                    Line3 = orderViewModel.Line3,
                    City = orderViewModel.City,
                    State = orderViewModel.State,
                    Zip = orderViewModel.Zip,
                    Country = orderViewModel.Country,
                    GiftWrap = orderViewModel.GiftWrap,
                    Lines = cart.Lines.ToArray()
                };

                repository.SaveOrder(order);
                return RedirectToAction(nameof(Completed));
            } else {
                return View(orderViewModel);
            }
        }
        public ViewResult Completed() {
            cart.Clear();
            return View();
        }
    }
}
