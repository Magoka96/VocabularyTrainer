namespace VocabularyTrainer.Infrastructure.Configuration;
public sealed class AuthOptions
{
    public string Domain { get; init; } = default!;
    public string ClientId { get; init; } = default!;
}
