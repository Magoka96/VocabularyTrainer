namespace VocabularyTrainer.Services.Models;

public record TranslationResult(
    string Source,
    string Target,
    string From,
    string To,
    string? Pos = null,
    IReadOnlyList<string>? Alternatives = null,
    IReadOnlyList<(string Source, string Target)>? ExamplePairs = null);