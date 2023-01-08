using Application.ShoppingCart.Commands.CreateShoppingCart;
using Application.ShoppingCart.Commands.DeleteShoppingCart;
using Application.ShoppingCart.Queries.GetShoppingCartById;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swagger.Models.Attribute;

namespace Presentation.Controllers
{
    /// <summary>
    /// Represents the ShoppingCart controller.
    /// </summary>
   // [Authorize(Policy = nameof(AuthPolicy.GlobalRights))]
    [Route("v{version:apiVersion}/shoppingCart"), SwaggerOrder("C")]
    public class ShoppingCartController : ApiController
    {
        /// <summary>
        /// Gets the shoppingCart  with the specified identifier, if it exists.
        /// </summary>
        /// <param name="shoppingCartId">The shoppingCart identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The shoppingCart with the specified identifier, if it exists.</returns>
        [HttpGet(), Route("shoppingCartById/{shoppingCartId}")]
        [ProducesResponseType(typeof(ShoppingCartResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetShoppingCart(Guid shoppingCartId, CancellationToken cancellationToken)
        {
            var query = new GetShoppingCartByIdQuery(shoppingCartId);

            var shoppingCart = await Sender.Send(query, cancellationToken);

            return Ok(shoppingCart);
        }

        /// <summary>
        /// Gets the shoppingCart  with the specified user identifier, if it exists.
        /// </summary>
        /// <param name="userId">The shoppingCart identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The shoppingCart with the specified user identifier, if it exists.</returns>
        [HttpGet(), Route("shoppingCartByUserId/{userId}")]
        [ProducesResponseType(typeof(List<ShoppingCartResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetShoppingCartByUserId(Guid userId, CancellationToken cancellationToken)
        {
            var query = new GetShoppingCartBy_UserIdQuery(userId);

            var shoppingCart = await Sender.Send(query, cancellationToken);

            return Ok(shoppingCart);
        }

        /// <summary>
        /// Creates a new ShoppingCart based on the specified request.
        /// </summary>
        /// <param name="request">The create ShoppingCart request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The identifier of the newly created ShoppingCart.</returns>
        [HttpPost(), Route("create")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateShoppingCart(
            [FromBody] CreateShoppingCartRequest request,
            CancellationToken cancellationToken)
        {
            var command = request.Adapt<CreateShoppingCartCommand>();

            var shoppingCartId = await Sender.Send(command, cancellationToken);

            return CreatedAtAction(nameof(CreateShoppingCart), new { shoppingCartId }, shoppingCartId);
        }

        /// <summary>
        /// Deletes a  ShoppingCart based on the specified request.
        /// </summary>
        /// <param name="request">The ShoppingCart item request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The identifier of the deleted  ShoppingCart item.</returns>
        [HttpPost(), Route("removeFromShoppingCartById")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveFromShoppingCartById([FromBody] DeleteFromShoppingCartRequest request,
            CancellationToken cancellationToken)
        {
            var command = request.Adapt<DeleteFromShoppingCartCommand>();

            var shoppingCartId = await Sender.Send(command, cancellationToken);

            return CreatedAtAction(nameof(CreateShoppingCart), new { shoppingCartId }, shoppingCartId);
        }
    }
}
