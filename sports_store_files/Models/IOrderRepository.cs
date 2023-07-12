﻿namespace sports_store_files.Models {
	public interface IOrderRepository {
        IEnumerable<Order> Orders { get; }
        void SaveOrder(Order order);
    }
}
