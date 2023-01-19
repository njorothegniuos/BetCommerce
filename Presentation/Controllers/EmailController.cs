using Application.Email.Commands.CreateEmail;
using Application.Email.Queries.GetEmailById;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Services.Interface;
using RabbitMQ.Utility;
using Swagger.Models.Attribute;

namespace Presentation.Controllers
{
    /// <summary>
    /// Represents the Email controller.
    /// </summary>
    [Route("v{version:apiVersion}/email"), SwaggerOrder("D")]
    //[Authorize]
    public class EmailController : ApiController
{
        private readonly RabbitMQConfiguration _rabbitMQConfiguration;
        private static IMessageQueueService<CreateEmailRequest> _messageQueueService;
        public EmailController(RabbitMQConfiguration rabbitMQConfiguration)
        {
            _rabbitMQConfiguration = rabbitMQConfiguration;
            _messageQueueService = new MessageQueueService<CreateEmailRequest>(_rabbitMQConfiguration.EmailAlertRequestPath, _rabbitMQConfiguration);
        }
        /// <summary>
        /// Gets the emailId with the specified identifier, if it exists.
        /// </summary>
        /// <param name="emailId">The email identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The email with the specified identifier, if it exists.</returns>
        [HttpGet("{emailId:guid}")]
        [ProducesResponseType(typeof(EmailResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEmail(Guid emailId, CancellationToken cancellationToken)
        {
            var query = new GetEmailByIdQuery(emailId);

            var email = await Sender.Send(query, cancellationToken);

            return Ok(email);
        }

        /// <summary>
        /// Creates a new email based on the specified request.
        /// </summary>
        /// <param name="request">The create email request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The identifier of the newly created email.</returns>
        [HttpPost(), Route("createEmail")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateEmail(
            [FromBody] CreateEmailRequest request,
            CancellationToken cancellationToken)
        {
            var command = request.Adapt<CreateEmailCommand>();

            var emailId = await Sender.Send(command, cancellationToken);

            return CreatedAtAction(nameof(CreateEmail), new { emailId }, emailId);
        }

        /// <summary>
        /// publish a new email based on the specified request.
        /// </summary>
        /// <param name="request">The create email request.</param>
        /// <returns>The identifier of the newly created order.</returns>
        [HttpPost(), Route("publish")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PublishEmailAlert(
            [FromBody] CreateEmailRequest request)
        {           
            _messageQueueService.Send(request);
            return Ok("Request received for processing");
        }
    }
}
