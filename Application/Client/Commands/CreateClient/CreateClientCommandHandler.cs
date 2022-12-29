using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Common;
using Microsoft.Extensions.Configuration;

namespace Application.Client.Commands.CreateClient
{
    internal class CreateClientCommandHandler : ICommandHandler<CreateClientCommand, CreateClientResponse>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public CreateClientCommandHandler(IClientRepository clientRepository, IUnitOfWork unitOfWork,
            IConfiguration configuration)
        {
            _clientRepository = clientRepository;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<CreateClientResponse> Handle(CreateClientCommand request, CancellationToken cancellationToken)
        {
            var client = new Domain.Entities.ClientModule.Client(Guid.NewGuid(), request.Name, 
                $"{Guid.NewGuid():N}".ToUpper(), Roles.Regular, Convert.ToInt32(_configuration["Security:TokenLifetimeInMins"]), Convert.ToInt32(_configuration["Security:CodeLifetimeInMins"]),
            false, request.Description, request.ContactEmail, ServiceOrigin.WebAPI.ToString(), null,
            RecordStatus.New);

            _clientRepository.Insert(client);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            var response = new CreateClientResponse(client.Id.ToString(), client.Secret);
            return response;
        }
    }
}
