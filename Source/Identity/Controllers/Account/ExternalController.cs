using IdentityModel;
using IdentityServer4.Events;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServerAspNetIdentity.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Infotecs.Identity
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class ExternalController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IEventService _events;
        readonly ILogger _logger;

        public ExternalController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IEventService events,
            ILogger logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interaction = interaction;
            _clientStore = clientStore;
            _events = events;
            _logger = logger.ForContext<ExternalController>();
        }

        [HttpGet]
        public async Task<IActionResult> Challenge(string provider, string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl)) returnUrl = "~/";

            var props = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(Callback)),
                Items = {
                    { "returnUrl", returnUrl },
                    { "scheme", provider },
                }
            };

            return Challenge(props, provider);
        }

        [HttpGet]
        public async Task<IActionResult> Callback()
        {
            var result = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);

            _logger.Information("Result: {result}", result);

            if (result?.Succeeded != true)
                throw new Exception("External authentication error");

            var externalUser = result.Principal;

            var externalUserIdClaim = externalUser.FindFirst(JwtClaimTypes.Subject) ?? externalUser.FindFirst(ClaimTypes.NameIdentifier) ??
                                      throw new Exception("Unknown userid");

            var claims = externalUser.Claims.ToList();
            claims.Remove(externalUserIdClaim);

            var provider = result.Properties.Items["scheme"];
            var providerUserId = externalUserIdClaim.Value;

            var user = await _userManager.FindByLoginAsync(provider, providerUserId);

            if (user == null)
            {
                var userName = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Name)?.Value ?? claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value ??
                    (claims.FirstOrDefault(x => x.Type == JwtClaimTypes.GivenName)?.Value ?? claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value ?? String.Empty +
                    claims.FirstOrDefault(x => x.Type == JwtClaimTypes.FamilyName)?.Value ?? claims.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value ?? String.Empty);

                var email = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Email)?.Value ?? claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

                user = new ApplicationUser(userName.Replace(" ", ""), email.Trim());
                var createUserResult = await _userManager.CreateAsync(user);

                if (!createUserResult.Succeeded)
                    throw new Exception(createUserResult.Errors.First().Description);

                var createLoginResult = await _userManager.AddLoginAsync(user, new UserLoginInfo(provider, providerUserId, provider));

                if (!createLoginResult.Succeeded)
                    throw new Exception(createLoginResult.Errors.First().Description);
            }

            var additionalLocalClaims = new List<Claim>();
            var localSignInProps = new AuthenticationProperties();
            ProcessLoginCallbackForOidc(result, additionalLocalClaims, localSignInProps);

            var principal = await _signInManager.CreateUserPrincipalAsync(user);
            additionalLocalClaims.AddRange(principal.Claims);

            var name = principal.FindFirst(JwtClaimTypes.Name)?.Value;
            await HttpContext.SignInAsync(user.Id, name, provider, localSignInProps, additionalLocalClaims.ToArray());
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            var returnUrl = result.Properties.Items["returnUrl"] ?? "~/";
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            await _events.RaiseAsync(new UserLoginSuccessEvent(provider, providerUserId, user.Id, name, true, context?.ClientId));

            _logger.Information("Return URL: {returnUrl}", returnUrl);

            return Redirect(returnUrl);
        }

        private void ProcessLoginCallbackForOidc(AuthenticateResult externalResult, List<Claim> localClaims, AuthenticationProperties localSignInProps)
        {
            var sid = externalResult.Principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.SessionId);

            if (sid != null)
                localClaims.Add(new Claim(JwtClaimTypes.SessionId, sid.Value));

            var id_token = externalResult.Properties.GetTokenValue("id_token");

            if (id_token != null)
                localSignInProps.StoreTokens(new[] { new AuthenticationToken { Name = "id_token", Value = id_token } });
        }
    }
}
