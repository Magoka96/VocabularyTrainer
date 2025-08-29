using System.Text.Json;

namespace VocabularyTrainer.Web.Services.Models;

public record WordAlternativesResult(
    List<string> Alternatives,
    string? Pos,
    string? BestNormalizedTarget
);
