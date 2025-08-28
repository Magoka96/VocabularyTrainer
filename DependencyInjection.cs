using Microsoft.AspNetCore.Connections;
using Npgsql;
using VocabularyTrainer.Configuration;
using VocabularyTrainer.Data;
using VocabularyTrainer.Services;
using VocabularyTrainer.Services.Interfaces;

namespace VocabularyTrainer;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration config)
    {
        var connStr = config.GetConnectionString("Postgres")
                     ?? throw new InvalidOperationException("ConnectionStrings:Postgres missing");

        services.AddSingleton<NpgsqlDataSource>(_ =>
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

    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
    {
        

        return services;
    }
}
