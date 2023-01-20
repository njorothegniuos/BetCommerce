using Application.Product.Commands.CreateProduct;
using Application.Product.GetProductById;
using Application.Product.Queries.GetProducts;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swagger.Models.Attribute;

namespace Presentation.Controllers
{
    /// <summary>
    /// Represents the product controller.
    /// </summary>
    [Authorize]
    [Route("v{version:apiVersion}/product"), SwaggerOrder("B")]
    public class ProductController : ApiController
    {
        /// <summary>
        /// Gets the product  with the specified identifier, if it exists.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The product with the specified identifier, if it exists.</returns>
        [HttpGet(), Route("{productId:guid}")]
        [ProducesResponseType(typeof(ProductListingResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProduct(Guid productId, CancellationToken cancellationToken)
        {
            var query = new GetProductByIdQuery(productId);

            var product = await Sender.Send(query, cancellationToken);

            return Ok(product);
        }
        /// <summary>
        /// Gets the products
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The products list.</returns>
        [HttpGet(), Route("list")]
        [ProducesResponseType(typeof(ProductListingResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProducts(CancellationToken cancellationToken)
        {
            var query = new GetProductsQuery();

            var product = await Sender.Send(query, cancellationToken);

            return Ok(product);
        }

        /// <summary>
        /// Creates a new product based on the specified request.
        /// </summary>
        /// <param name="request">The create product request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The identifier of the newly created product.</returns>
        [HttpPost(), Route("createProduct")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProduct(
            [FromBody] CreateProductRequest request,
            CancellationToken cancellationToken)
        {
            var command = request.Adapt<CreateProductCommand>();

            var productId = await Sender.Send(command, cancellationToken);

            return CreatedAtAction(nameof(CreateProduct), new { productId }, productId);
        }
    }
}
