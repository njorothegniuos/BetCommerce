using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Swagger.Models.Common;

namespace Presentation.Models.Common
{
    public class VersionErrorProvider : IErrorResponseProvider
    {
        public IActionResult CreateResponse(ErrorResponseContext context)
        {

            ResponseObject<object> responseObject = new ResponseObject<object>
            {
                Status = new ResponseStatus
                {
                    Code = $"{context.StatusCode}",
                    Message = $"{context.ErrorCode} - {context.Message}"
                }
            };

            return new BadRequestObjectResult(responseObject);
        }
    }
}
