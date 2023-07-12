﻿namespace sports_store_files.Models {
    public interface IProductRepository {
        IEnumerable<Product> Products { get; }
        void SaveProduct(Product product);
        Product DeleteProduct(int productID);
    }
}
