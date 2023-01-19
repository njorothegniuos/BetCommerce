using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.Metrics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Token.Commands.CreateToken
{
    internal class CreateTokenCommandHandler : ICommandHandler<CreateTokenCommand, TokenResponse>
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IJwtProvider _jwtProvider;
        public CreateTokenCommandHandler(ITokenRepository tokenRepository, IUnitOfWork unitOfWork,
            IConfiguration configuration,
        IJwtProvider jwtProvider)
        {
            _tokenRepository = tokenRepository;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _jwtProvider = jwtProvider;
        }

        public async Task<TokenResponse> Handle(CreateTokenCommand request, CancellationToken cancellationToken)
        {;
            DateTime baseDate = DateTime.UtcNow;
            DateTime expiryDate = baseDate.AddMinutes(Convert.ToInt32(_configuration["Security:TokenLifetimeInMins"]));

            string generatedToken =  await _jwtProvider.GenerateAsync(request.Id, request.Role);

            Domain.Entities.ClientModule.Token token = new Domain.Entities.ClientModule.Token(Guid.NewGuid(), request.Role, request.Id.ToString(), generatedToken, expiryDate.ToEpoch(), ServiceOrigin.WebAPI.ToString(), null, RecordStatus.New, null, DateTime.Now, null);

            _tokenRepository.Insert(token);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            TokenResponse tokenResponse = new TokenResponse(generatedToken, expiryDate.ToEpoch(), "Bearer");
            return tokenResponse;
        }
    }
}
