using Application.Abstractions.Messaging;
using Domain.Abstractions;

namespace Application.Role.Commands.CreateRole
{
    internal class CreateRoleCommandHandler : ICommandHandler<CreateRoleCommand, Guid>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateRoleCommandHandler(IRoleRepository roleRepository, IUnitOfWork unitOfWork)
        {
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var client = new Domain.Entities.ClientModule.Role(Guid.NewGuid(), request.CreatedAt, request.ModifiedAt);

            _roleRepository.Insert(client);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return client.Id;
        }
    }
}
