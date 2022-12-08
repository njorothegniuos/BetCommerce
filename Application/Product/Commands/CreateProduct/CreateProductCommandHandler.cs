using Application.Abstractions.Messaging;
using Domain.Abstractions;

namespace Application.Product.Commands.CreateProduct
{
    internal class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Guid>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Domain.Entities.ProductModule.Product(Guid.NewGuid(), request.Name, request.Image, request.Price, request.Quantity);

            _productRepository.Insert(product);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return product.Id;
        }
    }
}
