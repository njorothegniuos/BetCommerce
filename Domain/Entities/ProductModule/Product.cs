using Domain.Common;

namespace Domain.Entities.ProductModule
{
    public sealed class  Product : Entity
    {
        public Product(Guid id,string name, string image, decimal price, int quantity) : base(id)
        {
            Name = name;
            Image = image;  
            Price = price;
            Quantity = quantity;
        }
        private Product()
        {
        }
        public string Name { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
