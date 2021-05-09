using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ShoppingCart.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecuringApplication.ActionFilters
{
    public class OwnerAuthorize: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                var id = new Guid(context.ActionArguments["id"].ToString());

                var currentLoggedInUser = context.HttpContext.User.Identity.Name;

                IFilesService fileService = (IFilesService)context.HttpContext.RequestServices.GetService(typeof(IFilesService));
                var file = fileService.GetFile(id);

                if (file.Owner != currentLoggedInUser)
                {
                    context.Result = new UnauthorizedObjectResult("Access Denied");
                }
            }catch(Exception e)
            {
                context.Result = new BadRequestObjectResult("Bad Request");
            }
           
            base.OnActionExecuting(context);
        }
    }
}
