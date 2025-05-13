﻿namespace ERP.Models
{
    public class CartItemViewModel
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => Price * Quantity;

    }
}
