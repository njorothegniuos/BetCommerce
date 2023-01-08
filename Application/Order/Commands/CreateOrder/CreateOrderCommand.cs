using Application.Abstractions.Messaging;

namespace Application.Product.Commands.CreateProduct
{
    public sealed record CreateOrderCommand(Guid UserId, string OderNumber, decimal OderTotals) : ICommand<Guid>;
}
