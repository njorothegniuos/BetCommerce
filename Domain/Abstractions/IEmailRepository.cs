using Domain.Entities.MessagingModule;

namespace Domain.Abstractions
{
    public interface IEmailRepository
    {
        void Insert(EmailAlert emailAlert);
    }
}
