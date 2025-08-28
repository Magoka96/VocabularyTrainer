using Npgsql;

namespace VocabularyTrainer.Data;

public interface IDbConnectionFactory
{
    Task<NpgsqlConnection> OpenConnectionAsync(CancellationToken ct = default);
}