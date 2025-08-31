using System.Text.Json;

namespace VocabularyTrainer.Application.Models;

public record WordAlternativesResult(
    List<string> Alternatives,
    string? Pos,
    string? BestNormalizedTarget
);
