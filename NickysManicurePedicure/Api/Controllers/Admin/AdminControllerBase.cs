using Microsoft.AspNetCore.Mvc;

namespace NickysManicurePedicure.Api.Controllers.Admin;

[ApiController]
[Produces("application/json")]
[ApiExplorerSettings(GroupName = "admin")]
// TODO: Add [Authorize(Policy = AdminAuthorizationPolicies.AccessDashboard)] when real auth is introduced.
public abstract class AdminControllerBase : ControllerBase
{
}
