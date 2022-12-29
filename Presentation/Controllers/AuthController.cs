using Application.Client.Commands.CreateClient;
using Application.Client.Queries.AuthenticateClient;
using Application.Product.GetProductById;
using Application.Token.Commands.CreateToken;
using Domain.Common;
using Domain.Entities.ProductModule;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swagger.Models.Attribute;
using Swagger.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mime;

namespace Presentation.Controllers
{
    /// <summary>
    /// Represents the auth controller.
    /// </summary>
    [Route("v{version:apiVersion}/auth"), SwaggerOrder("A")]
    public  class AuthController : ApiController
    {
        /// <summary>
        /// Offers ability to register api clients
        /// </summary>     
        /// <param name="request"></param>
        /// <returns></returns>
        //[ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost, Route("client")]
        [Produces(MediaTypeNames.Application.Json), Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(CreateClientResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> RegisterClient([FromBody, Required] CreateClientRequest request, CancellationToken cancellationToken)
        {
            var command = request.Adapt<CreateClientCommand>();

            var createClientResponse = await Sender.Send(command, cancellationToken);

            return Ok(createClientResponse);
        }

        /// <summary>
        /// Before invoking this endpoint ensure your key and secret are whitelisted. 
        /// Generates a JWT Bearer access token that can be used to authorize subsequent requests.      
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, AllowAnonymous, Route("token")]
        [Produces(MediaTypeNames.Application.Json), Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(TokenResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateToken([FromBody, Required] TokenRequest _request, CancellationToken cancellationToken)
        {

            var query = new AuthenticateClientQuery(_request.ApiKey, _request.AppSecret);

            var client = await Sender.Send(query, cancellationToken);

            if (client is null) return Forbid();

            Roles Role = client.role;
            CreateTokenRequest request = new CreateTokenRequest(client.Id, Role);

            var command = request.Adapt<CreateTokenCommand>();

            var tokenResponse = await Sender.Send(command, cancellationToken);

            return Ok(tokenResponse);
        }
    }
}
