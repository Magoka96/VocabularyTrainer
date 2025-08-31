using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace VocabularyTrainer.Web.Extensions;

public static class Auth0EndpointExtensions
{
    public static IEndpointRouteBuilder MapAuth0Endpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/Account/Login", async (HttpContext ctx, string? returnUrl) =>
        {
            var authProps = new LoginAuthenticationPropertiesBuilder()
                .WithRedirectUri(returnUrl ?? "/")
                .Build();

            await ctx.ChallengeAsync(Auth0Constants.AuthenticationScheme, authProps);
        });

        endpoints.MapGet("/Account/Logout", async (HttpContext ctx) =>
        {
            var authProps = new LogoutAuthenticationPropertiesBuilder()
                .WithRedirectUri("/")
                .Build();

            await ctx.SignOutAsync(Auth0Constants.AuthenticationScheme, authProps);
            await ctx.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        });

        return endpoints;
    }
}
