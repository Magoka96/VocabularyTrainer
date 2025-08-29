using Npgsql;

namespace VocabularyTrainer.Web.Data;

public interface IDbConnectionFactory
{
    Task<NpgsqlConnection> OpenConnectionAsync(CancellationToken ct = default);
}