using Domain.Common;

namespace Domain.Entities.MessagingModule
{
    public sealed class EmailAlert : Entity
    {
        public EmailAlert(Guid id, string from, string to, string cc, string subject, string body, bool isHtml, string dlrStatus,
            string origin, string priority, string sendRetry, string attachmentFilePath) : base(id)
        {
            From = from;
            To = to;
            CC = cc;
            Subject = subject;
            Body = body;
            IsHtml = isHtml;
            DLRStatus = dlrStatus;
            Origin = origin;
            Priority = priority;
            SendRetry = sendRetry;
            AttachmentFilePath = attachmentFilePath;
        }

        private EmailAlert()
        {
        }
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public string CC { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public bool IsHtml { get; set; }
        public string DLRStatus { get; set; } = string.Empty;
        public string Origin { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public string SendRetry { get; set; } = string.Empty;
        public string AttachmentFilePath { get; set; } = string.Empty;
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
