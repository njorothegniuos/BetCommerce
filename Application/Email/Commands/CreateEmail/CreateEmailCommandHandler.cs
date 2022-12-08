using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Entities.MessagingModule;

namespace Application.Email.Commands.CreateEmail
{
    internal class CreateEmailCommandHandler : ICommandHandler<CreateEmailCommand, Guid>
    {
        private readonly IEmailRepository _emailRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateEmailCommandHandler(IEmailRepository emailRepository, IUnitOfWork unitOfWork)
        {
            _emailRepository = emailRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateEmailCommand request, CancellationToken cancellationToken)
        {
            var emailAlert = new EmailAlert(Guid.NewGuid(), request.From, request.To, request.Cc, request.Subject, request.Body, request.IsHtml, request.DlrStatus,
            request.Origin, request.Priority, request.SendRetry, request.AttachmentFilePath);

            _emailRepository.Insert(emailAlert);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return emailAlert.Id;
        }
    }
}
