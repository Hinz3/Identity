using Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace Common.Implementations
{
    public class AuthenticatedUser : IAuthenticatedUser
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly HttpContext httpContext;

        public AuthenticatedUser(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public AuthenticatedUser(HttpContext context)
        {
            this.httpContext = context;
        }

        private HttpContext Context
        {
            get
            {
                if (httpContext == null)
                {
                    return httpContextAccessor?.HttpContext;
                }

                return httpContext;
            }
        }

        public string UserId
        {
            get
            {
                return GetClaim("UserId");
            }
        }

        public string UserName
        {
            get
            {
                return GetClaim("UserName");
            }
        }

        public List<int> Functions
        {
            get
            {
                return Context?.User?.Claims?.Where(x => x.Type == "RF").Select(x => int.Parse(x.Value)).ToList();
            }
        }

        public bool HasFunction(int functionId)
        {
            return Functions.Any(x => x == functionId);
        }

        private string GetClaim(string claimName)
        {
            var claim = Context?.User?.Claims?.Where(x => x.Type == claimName).FirstOrDefault();
            if (claim == null)
            {
                return null;
            }

            return claim.Value;
        }
    }
}
