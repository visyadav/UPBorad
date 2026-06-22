using Application.Common.Exceptions;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace API.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class HasPermissionAttribute : Attribute, IAsyncActionFilter
{
    private readonly string _module;
    private readonly string _operation;

    public HasPermissionAttribute(string module, string operation)
    {
        _module = module;
        _operation = operation;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var currentUserService = context.HttpContext.RequestServices.GetRequiredService<ICurrentUserService>();

        if (!currentUserService.HasPermission(_module, _operation))
        {
            throw new ForbiddenException($"You do not have permission to access {_module} for {_operation}.");
        }

        await next();
    }
}