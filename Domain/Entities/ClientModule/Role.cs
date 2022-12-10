using Domain.Common;

namespace Domain.Entities.ClientModule
{
    public class Role : Entity
    {
        public Role(Guid id, DateTime createdAt, DateTime modifiedAt) : base(id)
        {
            CreatedAt = createdAt;
            ModifiedAt= modifiedAt;
        }
        public Role()
        {

        }
        public DateTime CreatedAt { get; set; }= DateTime.Now;
        public DateTime ModifiedAt { get; set; }

    }
}
