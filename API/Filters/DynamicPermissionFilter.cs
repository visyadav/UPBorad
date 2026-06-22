using API.Attributes;
using Application.Common.Exceptions;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Filters;

public class DynamicPermissionFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // 1. Check if the endpoint allows anonymous access or explicitly skips permission checks
        var hasAllowAnonymous = context.ActionDescriptor.EndpointMetadata
            .Any(em => em.GetType() == typeof(AllowAnonymousAttribute));

        var hasSkipCheck = context.ActionDescriptor.EndpointMetadata
            .Any(em => em.GetType() == typeof(SkipPermissionCheckAttribute));

        if (hasAllowAnonymous || hasSkipCheck)
        {
            await next();
            return;
        }

        // 2. Identify the Module
        if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
        {
            var controllerName = controllerActionDescriptor.ControllerName;
            
            // Check for the [PermissionModule] attribute on the controller
            var moduleAttribute = controllerActionDescriptor.ControllerTypeInfo
                .GetCustomAttributes(typeof(PermissionModuleAttribute), true)
                .FirstOrDefault() as PermissionModuleAttribute;

            var moduleName = moduleAttribute != null ? moduleAttribute.ModuleName : controllerName;

            // 3. Identify the Operation (HTTP Method)
            var httpMethod = context.HttpContext.Request.Method;
            var operation = httpMethod switch
            {
                "GET" => "Read",
                "POST" => "Write",
                "PUT" => "Update",
                "PATCH" => "Update",
                "DELETE" => "Delete",
                _ => "Read" // default for OPTIONS, HEAD, etc.
            };

            // 4. Verify Permissions
            var currentUserService = context.HttpContext.RequestServices.GetRequiredService<ICurrentUserService>();
            
            // If the user is not authenticated at all, we should let standard [Authorize] handle it, 
            // but if they are, we check module permissions.
            if (context.HttpContext.User.Identity?.IsAuthenticated == true)
            {
                if (!currentUserService.HasPermission(moduleName, operation))
                {
                    throw new ForbiddenException($"You do not have permission to access {moduleName} for {operation}.");
                }
            }
        }

        await next();
    }
}