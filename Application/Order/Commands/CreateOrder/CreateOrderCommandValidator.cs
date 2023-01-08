using FluentValidation;

namespace Application.Product.Commands.CreateProduct
{
    public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.OderNumber).NotEmpty();

        }
    }
}
