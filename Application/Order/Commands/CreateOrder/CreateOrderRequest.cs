namespace Application.Product.Commands.CreateProduct
{
    public sealed record CreateOrderRequest(Guid UserId, string OderNumber, decimal OderTotals);
}
