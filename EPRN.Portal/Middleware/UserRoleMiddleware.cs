using EPRN.Common.Enums;
using EPRN.Portal.Services.Interfaces;

namespace EPRN.Portal.Middleware
{
    public class UserRoleMiddleware
    {
        private readonly RequestDelegate _next;

        public UserRoleMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUserRoleService userRoleService)
        {
            ProcessQuerystring(context, userRoleService);

            await _next(context);
        }

        private void ProcessQuerystring(HttpContext context, IUserRoleService userRoleService)
        {
            // only get querystring data from a GET method
            if (context.Request.Method != HttpMethod.Get.Method)
                return;

            var vals = context.Request.Query.Where(qs => 
                qs.Key.Equals("role", StringComparison.CurrentCultureIgnoreCase) ||
                qs.Key.Equals("xrole", StringComparison.CurrentCultureIgnoreCase)
            );

            if (!vals.Any())
                return;

            foreach (var val in vals)
            {
                if (!val.Value.Any())
                    continue;

                foreach (var item in val.Value)
                {
                    if (Enum.TryParse<UserRole>(item, true, out var userRole))
                    {
                        if (val.Key.StartsWith("x"))
                            userRoleService.RemoveRole(userRole);
                        else
                            userRoleService.SetRole(userRole);
                    }
                }
            }
        }
    }
}
