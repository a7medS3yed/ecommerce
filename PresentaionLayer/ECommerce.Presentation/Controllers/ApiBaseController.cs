using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Shared.CommenResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ECommerce.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiBaseController : ControllerBase
    {
        protected IActionResult HandleProblem(Result result)
        {
            if (result.IsSuccess)
                return NoContent();
            else
                return HandleProblem(result.Errors);
        }

        protected ActionResult<TValue> HandleProblem<TValue>(Result<TValue> result)
        {
            if (result.IsSuccess)
                return Ok(result.Data);
            else
                return HandleProblem(result.Errors);
        }

        private ActionResult HandleProblem(IReadOnlyList<Error> errors)
        {
            // if no errors are provided, return 500
            if (errors.Count == 0)
            {
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "An Error Occurred"
            );

            }

            // if many error are validation errors handle them as validation problem 
            if (errors.All(E => E.ErrorType == ErrorTypes.Validation))
                return HandleValidationProblem(errors);

            // if only one error provided , handle them as single error 
            return HandleSingleError(errors[0]);
        }

        private ActionResult HandleSingleError(Error error)
        {
            return Problem(
                title: error.Code,
                detail: error.Description,
                type: error.ErrorType.ToString(),
                statusCode: MapErrorTypeIntoStatusCode(error.ErrorType)
                );
        }

        private ActionResult HandleValidationProblem(IReadOnlyList<Error> errors)
        {
            var modelState = new ModelStateDictionary();

            foreach (var error in errors)
            {
                modelState.AddModelError(error.Code, error.Description);
            }

            return ValidationProblem(modelState);
        }

        private static int MapErrorTypeIntoStatusCode(ErrorTypes errorTypes)
        {
          return errorTypes switch
            {
                ErrorTypes.NotFound => StatusCodes.Status404NotFound,
                ErrorTypes.Validation => StatusCodes.Status400BadRequest,
                ErrorTypes.InvalidCredintals => StatusCodes.Status401Unauthorized,
                ErrorTypes.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorTypes.Forbidden => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status500InternalServerError
            };
        }
    }
}
