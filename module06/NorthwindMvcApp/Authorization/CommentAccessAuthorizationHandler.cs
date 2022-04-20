using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Northwind.Services.Blogging;
using NorthwindMvcApp.Extensions;
using System.Threading.Tasks;

namespace NorthwindMvcApp.Authorization
{
    public class CommentAccessAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, BlogComment>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, BlogComment resource)
        {
            if (context.User == null || resource == null)
            {
                return Task.CompletedTask;
            }

            if (context.User.IsAdmin() || context.User.IsAuthor(resource.AuthorId.ToString()))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
