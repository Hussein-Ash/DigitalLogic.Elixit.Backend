namespace Elixir.Extensions.StoreAuthoeization
{
    

using Microsoft.AspNetCore.Authorization;

public class StoreAuthorizeAttribute : AuthorizeAttribute
{
    public StoreAuthorizeAttribute() : base("StoreIdPolicy") { }
}

}