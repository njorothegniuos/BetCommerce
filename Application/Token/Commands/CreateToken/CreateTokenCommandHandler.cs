using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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

        public CreateTokenCommandHandler(ITokenRepository tokenRepository, IUnitOfWork unitOfWork,
            IConfiguration configuration)
        {
            _tokenRepository = tokenRepository;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<TokenResponse> Handle(CreateTokenCommand request, CancellationToken cancellationToken)
        {
            //security key for token validation
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Security:Key"]));

            //credentials for signing token
            SigningCredentials signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            DateTime baseDate = DateTime.UtcNow;

            Roles role = request.Role;
            string subjectId = request.Id.ToString();
            DateTime expiryDate = baseDate.AddMinutes(Convert.ToInt32(_configuration["Security:TokenLifetimeInMins"]));

            Guid jti = Guid.NewGuid();
            //add claims
            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, $"{jti}"),
                new Claim(JwtRegisteredClaimNames.Sub, $"{subjectId}"),
                new Claim("cli", $"{request.Id}"),
                new Claim (ClaimTypes.Role, role.ToString())
            };


            //create token
            JwtSecurityToken jwtToken = new JwtSecurityToken(
                issuer: _configuration["Security:Issuer"],
                audience: _configuration["Security:Audience"],
                signingCredentials: signingCredentials,
                expires: expiryDate,
                notBefore: baseDate,
                claims: claims);

            string generatedToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            Domain.Entities.ClientModule.Token token = new Domain.Entities.ClientModule.Token(Guid.NewGuid(), role, subjectId, generatedToken, expiryDate.ToEpoch(), ServiceOrigin.WebAPI.ToString(), null, RecordStatus.New, null, DateTime.Now, null);

            _tokenRepository.Insert(token);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            TokenResponse tokenResponse = new TokenResponse(generatedToken, expiryDate.ToEpoch(), "Bearer");
            return tokenResponse;
        }
    }
}
