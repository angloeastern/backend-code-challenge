using AEBackend.Repositories.RepositoryUsingEF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AEBackend.Controllers.Utils;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
  private readonly IList<string> _roles;

  public AuthorizeAttribute(params string[] roles)
  {
    _roles = roles ?? new string[] { };
  }

  public void OnAuthorization(AuthorizationFilterContext context)
  {
    // Skip authorization if action is decorated with [AllowAnonymous] attribute
    var allowAnonymous = context.ActionDescriptor.EndpointMetadata
        .OfType<AllowAnonymousAttribute>().Any();

    if (allowAnonymous)
      return;

    // Authorization
    var user = context.HttpContext.User;

    if (user != null && _roles.Any())
    {
      var db = context.HttpContext.RequestServices
          .GetService<AppDBContext>();


      var userRoles = (from ur in db!.UserRoles
                       join r in db.Roles on ur.RoleId equals r.Id
                       join u in db.Users on ur.UserId equals u.Id
                       where u.UserName == user.Identity!.Name
                       select new
                       {
                         r.Name
                       }).ToList();

      var isAuthorized = false;
      foreach (var role in userRoles)
      {
        if (_roles.Contains(role.Name))
          isAuthorized = true;
      }

      if (isAuthorized)
        return;

    }

    context.Result = new UnauthorizedResult();
  }
}