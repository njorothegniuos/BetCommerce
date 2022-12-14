using Domain.Common;

namespace Domain.Entities.ClientModule
{
    public class Token : Entity
    {
        public Token(Guid id, Roles role, string subjectId, string jsonCode, long expires, string createdBy,
            string? modifiedBy, RecordStatus recordStatus, DateTime? authorizationDate, DateTime createdAt,
            DateTime? modifiedAt) : base(id)
        {
            Role = role;
            SubjectId = subjectId;
            JsonCode = jsonCode;
            Expires = expires;
            CreatedBy = createdBy;
            ModifiedBy = modifiedBy;
            RecordStatus = recordStatus;
            AuthorizationDate = authorizationDate;
            CreatedAt = createdAt;
            ModifiedAt = modifiedAt;
        }

        public Token()
        {
        }

        public Roles Role { get; set; }
        public string SubjectId { get; set; } = string.Empty;
        public string JsonCode { get; set; } = string.Empty;
        public long Expires { get; set; } = default;
        public string CreatedBy { get; set; } = string.Empty;
        public string? ModifiedBy { get; set; } = string.Empty;
        public RecordStatus RecordStatus { get; set; }
        public DateTime? AuthorizationDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
