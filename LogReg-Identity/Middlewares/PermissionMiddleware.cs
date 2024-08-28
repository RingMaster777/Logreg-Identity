using LogReg_Identity.Data;
using LogReg_Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace LogReg_Identity.Middlewares
{
    public class PermissionMiddleware
    {
        private readonly RequestDelegate _next;

        public PermissionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            var user = await userManager.GetUserAsync(context.User);

            if (user != null)
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var requestMethod = context.Request.Method; // GET, POST, etc.
                var requestPath = context.Request.Path.Value;

                

                var userPermissions = dbContext.RolePermissions
                    .Where(rp => userRoles.Contains(rp.Role.Name))
                    .Select(rp => rp.Permission.PermissionName)
                    .ToList();

                if (userPermissions.Contains(requestMethod))
                {
                    await _next(context); 
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden; 
                    await context.Response.WriteAsync("You do not have permission to access this resource.");
                }
            }
            else
            {
                await _next(context); 
            }
        }

    }

}
