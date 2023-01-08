using Domain.Common;

namespace Domain.Entities.OrderModule
{
    public sealed class Order : Entity
    {
        public Order(Guid id, Guid userId, string oderNumber, decimal oderTotals) : base(id)
        {
            UserId = userId;
            OderNumber = oderNumber;
            OderTotals = oderTotals;
        }
        private Order()
        {
        }
        public Guid UserId { get; set; }
        public string OderNumber { get; set; } = string.Empty;
        public decimal OderTotals { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
