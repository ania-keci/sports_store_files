using System.Collections.Generic;
using sports_store_files.Models;

namespace sports_store_files.Models.ViewModels {
	public class ProductsListViewModel {
        public IEnumerable<Product> Products { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentCategory { get; set; }
    }
}
