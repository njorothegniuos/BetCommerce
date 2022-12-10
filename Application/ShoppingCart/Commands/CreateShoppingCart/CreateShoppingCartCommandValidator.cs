using FluentValidation;

namespace Application.ShoppingCart.Commands.CreateShoppingCart
{
    public sealed class CreateShoppingCartCommandValidator : AbstractValidator<CreateShoppingCartCommand>
    {
        public CreateShoppingCartCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();

            RuleFor(x => x.ProductId).NotEmpty();
        }
    }
}
