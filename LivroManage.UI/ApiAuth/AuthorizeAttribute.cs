using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace LivroManage.UI.ApiAuth
{
    public class AuthorizeAttribute : TypeFilterAttribute
    {
        public AuthorizeAttribute() : base(typeof(AuthorizeActionFilter)) { }
    }

    public class AuthorizeActionFilter : IAsyncActionFilter
    {
        private readonly ValidateBearerToken _authToken;
        public AuthorizeActionFilter(ValidateBearerToken authToken)
        {
            _authToken = authToken;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            const string AUTHKEY = "authkey";
            const string AUTHID = "authid";
            var headers = context.HttpContext.Request.Headers;
            if (headers.ContainsKey(AUTHKEY))
            {
                bool isAuthorized = await _authToken.Validate(headers[AUTHKEY], headers[AUTHID]);
                if (!isAuthorized)
                    context.Result = new UnauthorizedResult();
                else
                    await next();
            }
            else
                context.Result = new UnauthorizedResult();
        }
    }
}
