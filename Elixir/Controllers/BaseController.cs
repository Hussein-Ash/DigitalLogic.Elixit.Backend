using System.Security.Claims;
using Elixir.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Elixir.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        protected virtual string GetClaim(string claimName)
        {
            var claims = (User.Identity as ClaimsIdentity)?.Claims;
            var claim = claims?.FirstOrDefault(c =>
                string.Equals(c.Type, claimName, StringComparison.CurrentCultureIgnoreCase) &&
                !string.Equals(c.Type, "null", StringComparison.CurrentCultureIgnoreCase));
            var rr = claim?.Value!.Replace("\"", "");

            return rr ?? "";
        }

        protected Guid Id => Guid.TryParse(GetClaim("Id"), out var id) ? id : Guid.Empty;

        protected string Role => GetClaim("Role");

        protected Guid? ParentId
        {
            get
            {
                var idString = GetClaim("ParentId");
                if (!string.Equals(idString, null, StringComparison.Ordinal) &&
                    !string.Equals(idString, "null", StringComparison.Ordinal))
                    return Guid.Parse(idString);
                return null;
            }
        }
        
        protected string MethodType => HttpContext.Request.Method;

        
        
        /// <summary>
        /// Handles a response that is already a GenericResponse.
        /// </summary>
        /// <typeparam name="T">The type of the data.</typeparam>
        /// <param name="response">The response of type GenericResponse.</param>
        /// <returns>Returns the standardized response.</returns>
        protected ObjectResult OkGeneric<T>(GenericResponse<T> response)
        {
            return response.Code >= 400 ? StatusCode(response.Code, response) : base.Ok(response);
        }
        
        
        /// <summary>
        /// Returns a generic response for success or failure.
        /// </summary>
        /// <typeparam name="T">The type of the data.</typeparam>
        /// <param name="result">The result tuple containing data and an error message.</param>
        /// <returns>Returns a standardized response.</returns>
        protected ObjectResult Ok<T>((T? data, string? error) result)
        {
            if (result.error != null)
            {
                return BadRequest(GenericResponse<T>.Failure(result.error));
            }

            return base.Ok(GenericResponse<T>.Success(result.data!));
        }

        /// <summary>
        /// Returns a paginated generic response.
        /// </summary>
        /// <typeparam name="T">The type of the data.</typeparam>
        /// <param name="result">The result tuple containing data, total count, and an error message.</param>
        /// <param name="pageNumber">The current page number.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <returns>Returns a paginated standardized response.</returns>
        protected ObjectResult OkPaginated<T>((List<T>? data, int? totalCount, string? error) result, int pageNumber, int pageSize)
        {
            if (result.error != null)
            {
                return BadRequest(GenericResponse<List<T>>.Failure(result.error));
            }

            var pagination = new PaginationMetadata(result.totalCount ?? 0, pageSize, pageNumber);

            return base.Ok(GenericResponse<List<T>>.Success(result.data!, pagination));
        }
    }
}
