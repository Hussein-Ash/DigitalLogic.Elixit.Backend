using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace Elixir.Extensions.StoreAuthoeization
{

public class StoreIdAuthorizationHandler : AuthorizationHandler<StoreIdRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, StoreIdRequirement requirement)
    {
        var storeIdClaim = context.User?.Claims.FirstOrDefault(c => c.Type == "StoreId")?.Value;

        if (string.IsNullOrEmpty(storeIdClaim)  )
        {
            context.Fail(); // If the claim is missing, mark the requirement as failed.
        }
        else
        {
            context.Succeed(requirement); // If the claim is present, succeed the requirement.
        }

        return Task.CompletedTask;
    }
}

}