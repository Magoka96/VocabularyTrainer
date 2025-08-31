namespace VocabularyTrainer.Application.Models;

public record WordExamplesResult(
    List<(string Source, string Target)> ExamplePairs
);
