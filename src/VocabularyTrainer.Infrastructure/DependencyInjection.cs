using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using VocabularyTrainer.Application.Interfaces;
using VocabularyTrainer.Infrastructure.Configuration;
using VocabularyTrainer.Infrastructure.Data;
using VocabularyTrainer.Infrastructure.Services;

namespace VocabularyTrainer.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration config)
    {
        var connStr = config.GetConnectionString("Postgres")
                     ?? throw new InvalidOperationException("ConnectionStrings:Postgres missing");

        services.AddSingleton(_ =>
        {
            var builder = new NpgsqlDataSourceBuilder(connStr);
            return builder.Build();
        });

        services.AddScoped<IDbConnectionFactory, NpgsqlConnectionFactory>();

        return services;
    }

    public static IServiceCollection AddTranslationService(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<TranslatorOptions>(config.GetSection("Translator"));

        services.AddHttpClient<ITranslationService, TranslationService>();

        return services;
    }
}
