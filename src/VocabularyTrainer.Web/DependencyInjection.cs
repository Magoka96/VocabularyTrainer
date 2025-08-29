using Microsoft.AspNetCore.Connections;
using Npgsql;
using VocabularyTrainer.Web.Configuration;
using VocabularyTrainer.Web.Data;
using VocabularyTrainer.Web.Services;
using VocabularyTrainer.Web.Services.Interfaces;

namespace VocabularyTrainer.Web;

public static class ServiceCollectionExtensions
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

    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
    {
        

        return services;
    }
}
