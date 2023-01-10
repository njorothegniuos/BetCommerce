using System;

namespace Core.web.Mvc.Models
{
    public sealed record CreateOrderRequest(Guid UserId, string OderNumber, decimal OderTotals);
}
