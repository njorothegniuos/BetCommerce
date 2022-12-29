using Domain.Common;

namespace Application.Client.Queries.GetClientById
{
    public sealed record ClientResponse(Guid Id, string Name, RecordStatus recordStatus, bool isActive);
}
