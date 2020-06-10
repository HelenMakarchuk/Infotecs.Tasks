using IdentityServer4.Events;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServerAspNetIdentity.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Infotecs.Identity
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IEventService _events;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            IEventService events)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interaction = interaction;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
            _events = events;
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            return View("Login", await BuildLoginViewModelAsync(returnUrl));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginInputModel model)
        {
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberLogin, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.Username);
                    await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName, clientId: context?.ClientId));

                    if (!Url.IsLocalUrl(model.ReturnUrl))
                        throw new Exception("Return URL is not local");

                    return Redirect(model.ReturnUrl);
                }

                await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "invalid credentials", clientId: context?.ClientId));
                ModelState.AddModelError(string.Empty, AccountOptions.IncorrectCredentialsErrorMessage);
            }

            return View("Login", await BuildLoginViewModelAsync(model));
        }

        [HttpGet]
        public async Task<IActionResult> Register(string returnUrl)
        {
            return View("Login", await BuildLoginViewModelAsync(returnUrl, true));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(LoginInputModel model)
        {
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Username);

                if (user == null)
                {
                    user = new ApplicationUser(model.Username);

                    var createUserResult = await _userManager.CreateAsync(user, model.Password);

                    if (!createUserResult.Succeeded)
                        throw new Exception(createUserResult.Errors.First().Description);

                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
                    };

                    var addClaimsResult = await _userManager.AddClaimsAsync(user, claims);

                    if (!addClaimsResult.Succeeded)
                        throw new Exception(addClaimsResult.Errors.First().Description);

                    await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName, clientId: context?.ClientId));

                    return Redirect(model.ReturnUrl);
                }

                await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, AccountOptions.LoginExistsErrorMessage, clientId: context?.ClientId));
                ModelState.AddModelError(string.Empty, AccountOptions.LoginExistsErrorMessage);
            }

            return View("Login", await BuildLoginViewModelAsync(model));
        }

        /// <summary>
        /// Show logout page
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            // build a model so the logout page knows what to display
            //var vm = await BuildLogoutViewModelAsync(logoutId);

            //if (vm.ShowLogoutPrompt == false)
            //{
            //    // if the request for logout was properly authenticated from IdentityServer, then
            //    // we don't need to show the prompt and can just log the user out directly.
            //    return await Logout(vm);
            //}

            //return View(vm);

            return Ok();
        }

        /// <summary>
        /// Handle logout page postback
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutInputModel model)
        {
            // build a model so the logged out page knows what to display
            // var vm = await BuildLoggedOutViewModelAsync(model.LogoutId);

            //if (User?.Identity.IsAuthenticated == true)
            //{
            //    // delete local authentication cookie
            //    await _signInManager.SignOutAsync();

            //    // raise the logout event
            //    await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
            //}

            //// check if we need to trigger sign-out at an upstream identity provider
            //if (vm.TriggerExternalSignout)
            //{
            //    // build a return URL so the upstream provider will redirect back
            //    // to us after the user has logged out. this allows us to then
            //    // complete our single sign-out processing.
            //    string url = Url.Action("Logout", new { logoutId = vm.LogoutId });

            //    // this triggers a redirect to the external provider for sign-out
            //    return SignOut(new AuthenticationProperties { RedirectUri = url }, vm.ExternalAuthenticationScheme);
            //}

            //return View("LoggedOut", vm);

            return Ok();
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl, bool isRegister = false)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            var schemes = await _schemeProvider.GetAllSchemesAsync();
            var externalProviders = schemes.Where(x => !String.IsNullOrEmpty(x.DisplayName))
                                           .Select(x => new ExternalProvider { DisplayName = x.DisplayName, AuthenticationScheme = x.Name });

            return new LoginViewModel
            {
                isRegister = isRegister,
                ReturnUrl = returnUrl,
                Username = context?.LoginHint,
                ExternalProviders = externalProviders.ToArray()
            };
        }

        async Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model)
        {
            var vm = await BuildLoginViewModelAsync(model.ReturnUrl, model.isRegister);
            vm.Username = model.Username;
            vm.RememberLogin = model.RememberLogin;
            return vm;
        }
    }
}
