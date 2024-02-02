using Complaint_Report_Registering_API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Complaint_Report_Registering_API.Filters
{
    public class User_ValidationCreateUserFliterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var user = context.ActionArguments["userDTO"] as UserDTO;

            if (user == null)
            {
                context.ModelState.AddModelError("User", "User object is null!");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }
        }
    }
}