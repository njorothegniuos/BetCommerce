using Domain.Common;

namespace Domain.Entities.ClientModule
{
    public class Client : Entity
    {
        public Client(Guid id, string name, string secret, Roles role, int accessTokenLifetimeInMins, int authorizationCodeLifetimeInMins,
            bool isActive, string description, string contactEmail, string createdBy, string modifiedBy,
            RecordStatus recordStatus) : base(id)
        {
            Name = name;
            Secret = secret;    
            Role = role;
            AccessTokenLifetimeInMins = accessTokenLifetimeInMins;
            AuthorizationCodeLifetimeInMins = authorizationCodeLifetimeInMins;
            IsActive = isActive;
            Description = description;
            ContactEmail = contactEmail;
            CreatedBy = createdBy;
            ModifiedBy = modifiedBy;
            RecordStatus = recordStatus;
        }
        public Client()
        {

        }
        public string Name { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
        public Roles Role { get; set; }
        public int AccessTokenLifetimeInMins { get; set; } = 60;
        public int AuthorizationCodeLifetimeInMins { get; set; } = 60;
        public bool IsActive { get; set; } = false;
        public string Description { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public string ModifiedBy { get; set; } = string.Empty;
        public RecordStatus RecordStatus { get; set; }
        public DateTime? AuthorizationDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ModifiedAt { get; set; }
    }
}
