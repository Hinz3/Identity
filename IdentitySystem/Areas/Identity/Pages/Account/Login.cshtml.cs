// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IdentitySystem.Core.Models;
using IdentitySystem.Core.Interfaces;
using IdentitySystem.Core.Interfaces.Services;

namespace IdentitySystem.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IValidator<LoginParameters> validator;
        private readonly ILogger<LoginModel> _logger;
        private readonly IAuthorizationService authorizationService;

        public LoginModel(SignInManager<IdentityUser> signInManager, IValidator<LoginParameters> validator, ILogger<LoginModel> logger,
            IAuthorizationService authorizationService)
        {
            _signInManager = signInManager;
            this.validator = validator;
            _logger = logger;
            this.authorizationService = authorizationService;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public Login Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        public async Task OnGetAsync([FromQuery] LoginParameters loginParameters = null)
        {
            if (loginParameters == null)
            {
                ErrorMessage = "Parameters are required";
            }

            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (loginParameters == null)
            {
                return;
            }

            var errors = await validator.Validate(loginParameters);
            if (!errors.Any())
            {
                return;
            }

            ErrorMessage = errors.First().Message;
            ModelState.AddModelError(string.Empty, ErrorMessage);
        }

        public async Task<IActionResult> OnPostAsync([FromQuery] LoginParameters loginParameters = null)
        {
            if (loginParameters == null)
            {
                ModelState.AddModelError(string.Empty, "Missing required parameters.");
                return Page();
            }

            var errors = await validator.Validate(loginParameters);
            if (errors.Any())
            {
                ModelState.AddModelError(string.Empty, errors.First().Message);
                return Page();
            }

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (!ModelState.IsValid)
            {
                // If we got this far, something failed, redisplay form
                return Page();
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out.");
                return RedirectToPage("./Lockout");
            }

            if (result.RequiresTwoFactor)
            {
                return RedirectToPage("./LoginWith2fa", new { RememberMe = Input.RememberMe, ResponseCode = loginParameters.ResponseCode, ClientId = loginParameters.ClientId,
                                                              RedirectUrl = loginParameters.RedirectUrl, Scopes = loginParameters.Scopes, State = loginParameters.State });
            }

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }

            _logger.LogInformation("User logged in.");
            var response = await authorizationService.GenerateAuthorizationCode(Input.Email, loginParameters);
            if (!response.Success)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }

            return Redirect(response.Data);
            
        }
    }
}
