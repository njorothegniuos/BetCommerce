using Domain.Common;

namespace Application.Abstractions;

public interface IJwtProvider
{
    Task<string> GenerateAsync(Guid Id, Roles Role);
}
