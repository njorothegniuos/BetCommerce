using Domain.Common;
using Domain.Entities.ProductModule;

namespace Domain.Entities.ShoppingCartModule
{
    public sealed class ShoppingCart : Entity
    {
        public ShoppingCart(Guid id, Guid userId,Guid productId, int quantity) : base(id)
        {
            UserId = userId;    
            ProductId = productId;
            Quantity = quantity;
        }
        private ShoppingCart()
        {
        }
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public Product? Product { get; set; }
        public int Quantity { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
