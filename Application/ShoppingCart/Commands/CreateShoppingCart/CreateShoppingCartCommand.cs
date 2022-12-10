using Application.Abstractions.Messaging;

namespace Application.ShoppingCart.Commands.CreateShoppingCart
{
    public sealed record CreateShoppingCartCommand(Guid UserId, Guid ProductId, int Quantity) : ICommand<Guid>;
}
