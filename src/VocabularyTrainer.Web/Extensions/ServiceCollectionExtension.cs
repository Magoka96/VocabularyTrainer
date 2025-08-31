using VocabularyTrainer.Application.Interfaces;
using VocabularyTrainer.Web.Authorization;

namespace VocabularyTrainer.Web.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCurrentUserService(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}
