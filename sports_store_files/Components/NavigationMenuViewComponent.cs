using Microsoft.AspNetCore.Mvc;
using System.Linq;
using sports_store_files.Models;

namespace sports_store_files.Components {
	public class NavigationMenuViewComponent : ViewComponent{
        private IProductRepository repository;
        public NavigationMenuViewComponent(IProductRepository repo) {
            repository = repo;
        }
        public IViewComponentResult Invoke() {
            ViewBag.SelectedCategory = RouteData?.Values["category"];
            return View(repository.Products
            .Select(x => x.Category)
            .Distinct()
            .OrderBy(x => x));
        }
    }
}
