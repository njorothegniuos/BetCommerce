using Application.Abstractions.Messaging;

namespace Application.Email.Queries.GetEmailById
{
    public sealed record GetEmailByIdQuery(Guid emailId) : IQuery<EmailResponse>;
}
