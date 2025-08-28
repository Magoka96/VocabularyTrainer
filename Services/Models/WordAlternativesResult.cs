using System.Text.Json;

namespace VocabularyTrainer.Services.Models;

public record WordAlternativesResult(
    List<string> Alternatives,
    string? Pos,
    JsonElement Best
);
