using Domain.Entities.ClientModule;

namespace Domain.Abstractions
{
    public interface ITokenRepository
    {
        void Insert(Token token);
    }
}
