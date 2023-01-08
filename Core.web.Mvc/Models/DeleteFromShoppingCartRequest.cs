using System;

namespace Core.web.Mvc.Models
{
    public sealed record DeleteFromShoppingCartRequest(Guid Id, Guid UserId, Guid ProductId, int Quantity);
}
