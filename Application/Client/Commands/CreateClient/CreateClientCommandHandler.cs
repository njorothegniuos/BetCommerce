using Application.Abstractions.Messaging;
using Domain.Abstractions;

namespace Application.Client.Commands.CreateClient
{
    internal class CreateClientCommandHandler : ICommandHandler<CreateClientCommand, Guid>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateClientCommandHandler(IClientRepository clientRepository, IUnitOfWork unitOfWork)
        {
            _clientRepository = clientRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateClientCommand request, CancellationToken cancellationToken)
        {
            var client = new Domain.Entities.ClientModule.Client(Guid.NewGuid(), request.Name, request.Secret, request.Role, request.AccessTokenLifetimeInMins, request.AuthorizationCodeLifetimeInMins,
            request.IsActive, request.Description, request.ContactEmail, request.CreatedBy, request.ModifiedBy,
            request.RecordStatus);

            _clientRepository.Insert(client);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return client.Id;
        }
    }
}
