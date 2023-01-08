using Application.Abstractions.Messaging;
using Domain.Abstractions;

namespace Application.Product.Commands.CreateProduct
{
    internal class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, Guid>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var product = new Domain.Entities.OrderModule.Order(Guid.NewGuid(), request.UserId, request.OderNumber, request.OderTotals);

            _orderRepository.Insert(product);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return product.Id;
        }
    }
}
