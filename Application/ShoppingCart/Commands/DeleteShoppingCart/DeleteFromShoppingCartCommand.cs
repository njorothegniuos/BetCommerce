using Application.Abstractions.Messaging;

namespace Application.ShoppingCart.Commands.DeleteShoppingCart
{
    public sealed record DeleteFromShoppingCartCommand(Guid Id, Guid UserId, Guid ProductId, int Quantity) : ICommand<Guid>;
}
