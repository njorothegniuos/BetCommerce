using Application.Product.Commands.CreateProduct;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Services.Interface;
using RabbitMQ.Utility;
using Swagger.Models.Attribute;
using Swagger.Models.Common;

namespace Presentation.Controllers
{
    /// <summary>
    /// Represents the product controller.
    /// </summary>
    //[Authorize]
    [Route("v{version:apiVersion}/order"), SwaggerOrder("B")]
    public class OrderController : ApiController
    {
        private readonly RabbitMQConfiguration _rabbitMQConfiguration;
        private static IMessageQueueService<CreateOrderRequest> _messageQueueService;
        public OrderController(RabbitMQConfiguration rabbitMQConfiguration)
        {
            _rabbitMQConfiguration = rabbitMQConfiguration;
            _messageQueueService = new MessageQueueService<CreateOrderRequest>(_rabbitMQConfiguration.OrderRequestPath, _rabbitMQConfiguration);
        }

        /// <summary>
        /// Creates a new order based on the specified request.
        /// </summary>
        /// <param name="request">The create order request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The identifier of the newly order product.</returns>
        [HttpPost(), Route("create")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateOrder(
            [FromBody] CreateOrderRequest request,
            CancellationToken cancellationToken)
        {
            var command = request.Adapt<CreateOrderCommand>();

            var productId = await Sender.Send(command, cancellationToken);

            return CreatedAtAction(nameof(CreateOrder), new { productId }, productId);
        }

        /// <summary>
        /// publish a new order based on the specified request.
        /// </summary>
        /// <param name="request">The publish order request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The identifier of the newly published order.</returns>
        [HttpPost(), Route("publish")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PublishOder(
            [FromBody] CreateOrderRequest request,
            CancellationToken cancellationToken)
        {           
            _messageQueueService.Send(request);
            return Ok("Request received for processing");
        }
    }
}
