using FluentValidation;

namespace Application.Product.Commands.CreateProduct
{
    public sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();

        }
    }
}
