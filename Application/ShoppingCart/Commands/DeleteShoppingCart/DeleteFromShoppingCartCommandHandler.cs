using Application.Abstractions.Messaging;
using Domain.Abstractions;

namespace Application.ShoppingCart.Commands.DeleteShoppingCart
{
    internal class DeleteFromShoppingCartCommandHandler : ICommandHandler<DeleteFromShoppingCartCommand, Guid>
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteFromShoppingCartCommandHandler(IShoppingCartRepository shoppingCartRepository, IUnitOfWork unitOfWork)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(DeleteFromShoppingCartCommand request, CancellationToken cancellationToken)
        {
            var shoppingCart = new Domain.Entities.ShoppingCartModule.ShoppingCart(request.Id, request.UserId, request.ProductId, request.Quantity);

            _shoppingCartRepository.Delete(shoppingCart);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return shoppingCart.Id;
        }
    }
}
