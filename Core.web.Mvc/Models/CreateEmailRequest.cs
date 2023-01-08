namespace Core.web.Mvc.Models
{
    public sealed record CreateEmailRequest(string From, string To, string Cc, string Subject, string Body, bool IsHtml, string DlrStatus,
           string Origin, string Priority, string SendRetry, string AttachmentFilePath);
}
