using LinkO.Shared.CommonResult;
using LinkO.Shared.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiBaseController : ControllerBase
    {
        // Common Result

        // Handle Requset Without Values :-
        //          If Request Success => Retutn no Content [204]
        //          If Request Failure => Retutn Problem Details with status Code , Error Details
        protected IActionResult HandleResult(Result result)
        {
            if (result.IsSuccess)
                return NoContent();
            else
                return HandleProblem(result.Errors);
        }
        // Handle Request With Values :-
        //          If Request Success => Retutn Ok [200]
        //          If Request Failure => Retutn Problem Details with status Code , Error Details
        protected ActionResult<TValue> HandleResult<TValue>(Result<TValue> result)
        {
            if (result.IsSuccess)
                return Ok(ApiResponse<TValue>.SuccessResponse(result.Value));
                //return Ok(result.Value);
            else
                return HandleProblem(result.Errors);
        }

        private ActionResult HandleProblem(IReadOnlyList<Error> errors)
        {
            // if no Errors => Return Default error 500
            // If one Error => handle it 
            // if more than one => Hadnle As Validation

            if (errors.Count == 0)
                return Problem(statusCode: StatusCodes.Status500InternalServerError,
                                title: "Internal Server Error",
                                detail: "Unexpected Error Occured !");
            if (errors.All(E => E.Type == ErrorType.Validation))
                return HandleValidationProblem(errors);
            return HandleSingleErrorProblem(errors[0]);
        }

        private ActionResult HandleSingleErrorProblem(Error error)
        {
            return Problem(
                title: error.Code,
                detail: error.Description,
                type: error.Type.ToString(),
                statusCode: MapErrorTypeToSatusCode(error.Type)
                );
        }
        private static int MapErrorTypeToSatusCode(ErrorType errorType) => errorType switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.InvalidCredentials => StatusCodes.Status401Unauthorized,
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError

        };
        private ActionResult HandleValidationProblem(IReadOnlyList<Error> errors)
        {
            var ModelState = new ModelStateDictionary();
            foreach (var error in errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
            return ValidationProblem(ModelState);
        }

        protected string GetUserEmail() 
        {
           var UserEmail = User.FindFirstValue(ClaimTypes.Email);
            return UserEmail;
        }
    }
}
