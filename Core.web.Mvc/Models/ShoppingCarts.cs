using System;
using System.ComponentModel.DataAnnotations;

namespace Core.web.Mvc.Models
{
    public class ShoppingCarts
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public Products? Product { get; set; }
        public int Quantity { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
