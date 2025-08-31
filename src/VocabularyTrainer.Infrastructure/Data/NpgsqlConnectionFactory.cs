using Npgsql;

namespace VocabularyTrainer.Infrastructure.Data;

public sealed class NpgsqlConnectionFactory : IDbConnectionFactory
{
    private readonly NpgsqlDataSource _dataSource;

    public NpgsqlConnectionFactory(NpgsqlDataSource dataSource) => _dataSource = dataSource;

    public async Task<NpgsqlConnection> OpenConnectionAsync(CancellationToken ct = default)
    {
        var conn = await _dataSource.OpenConnectionAsync(ct);
        return conn;
    }
}
