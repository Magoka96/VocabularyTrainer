using Auth0.AspNetCore.Authentication;
using VocabularyTrainer.Infrastructure.Configuration;

namespace VocabularyTrainer.Web.Extensions;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddAuth0Authentication(
        this IServiceCollection services, IConfiguration config)
    {
        services.Configure<AuthOptions>(config.GetSection("Auth0"));

        var auth0 = config.GetSection("Auth0").Get<AuthOptions>()!;
        services.AddAuth0WebAppAuthentication(options =>
        {
            options.Domain = auth0.Domain;
            options.ClientId = auth0.ClientId;
        });

        return services;
    }
}
