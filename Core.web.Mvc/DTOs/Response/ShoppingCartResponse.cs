﻿using System;

namespace Core.web.Mvc.DTOs.Response
{
    public class ShoppingCartResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
