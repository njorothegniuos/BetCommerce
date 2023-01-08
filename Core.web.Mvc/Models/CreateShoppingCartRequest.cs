using System;

namespace Core.web.Mvc.Models
{
    public sealed record CreateShoppingCartRequest(Guid UserId, Guid ProductId, int Quantity);
}
