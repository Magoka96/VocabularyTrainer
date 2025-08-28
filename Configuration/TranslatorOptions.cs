namespace VocabularyTrainer.Configuration;

public sealed class TranslatorOptions
{
    public string Endpoint { get; init; } = default!;
    public string Region { get; init; } = default!;
    public string Key { get; init; } = default!;
}