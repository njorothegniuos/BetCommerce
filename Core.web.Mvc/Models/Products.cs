using System;
using System.ComponentModel.DataAnnotations;

namespace Core.web.Mvc.Models
{
    public class Products
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
