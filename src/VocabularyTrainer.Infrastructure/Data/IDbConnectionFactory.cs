using Npgsql;

namespace VocabularyTrainer.Infrastructure.Data;

public interface IDbConnectionFactory
{
    Task<NpgsqlConnection> OpenConnectionAsync(CancellationToken ct = default);
}