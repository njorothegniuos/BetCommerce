using Domain.Common;

namespace Application.Client.Commands.CreateClient
{
    public sealed record CreateClientRequest(Guid Id, string Name, string Secret, Roles Role, int AccessTokenLifetimeInMins, int AuthorizationCodeLifetimeInMins,
            bool IsActive, string Description, string ContactEmail, string CreatedBy, string ModifiedBy,
            RecordStatus RecordStatus);
}
