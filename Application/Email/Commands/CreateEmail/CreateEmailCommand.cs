using Application.Abstractions.Messaging;

namespace Application.Email.Commands.CreateEmail
{
    public sealed record CreateEmailCommand(string From, string To, string Cc, string Subject, string Body, bool IsHtml, string DlrStatus,
            string Origin, string Priority, string SendRetry, string AttachmentFilePath) : ICommand<Guid>;
}
