namespace Elixir.Extensions.StoreAuthoeization
{




    using Microsoft.AspNetCore.Authorization;

    public class StoreIdRequirement : IAuthorizationRequirement
    {
        public StoreIdRequirement() { }
    }
}