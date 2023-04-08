using IdentitySystem.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;
using IAuthorizationService = IdentitySystem.Core.Interfaces.Services.IAuthorizationService;

namespace IdentitySystem.Controllers.v1
{
    [AllowAnonymous]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly IAuthorizationService authorizationService;

        public TokenController(IAuthorizationService authorizationService)
        {
            this.authorizationService = authorizationService;
        }

        /// <summary>
        /// Generate token from refresh token and authorization code.
        /// </summary>
        /// <param name="tokenParameters"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GenerateToken([FromBody] TokenParameters tokenParameters)
        {
            try
            {
                var response = await authorizationService.GenerateToken(tokenParameters);
                if (!response.Success)
                {
                    return Unauthorized();
                }

                return Ok(response);
            }
            catch (InvalidCredentialException)
            {
                return Unauthorized();
            }
        }
    }
}
