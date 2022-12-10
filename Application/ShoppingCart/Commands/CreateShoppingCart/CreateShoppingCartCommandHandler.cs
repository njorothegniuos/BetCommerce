using Application.Abstractions.Messaging;
using Domain.Abstractions;

namespace Application.ShoppingCart.Commands.CreateShoppingCart
{
    internal class CreateShoppingCartCommandHandler : ICommandHandler<CreateShoppingCartCommand, Guid>
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateShoppingCartCommandHandler(IShoppingCartRepository shoppingCartRepository, IUnitOfWork unitOfWork)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateShoppingCartCommand request, CancellationToken cancellationToken)
        {
            var shoppingCart = new Domain.Entities.ShoppingCartModule.ShoppingCart(Guid.NewGuid(), request.UserId, request.ProductId, request.Quantity);

            _shoppingCartRepository.Insert(shoppingCart);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return shoppingCart.Id;
        }
    }
}
