namespace VocabularyTrainer.Web.Services.Models;

public record WordExamplesResult(
    List<(string Source, string Target)> ExamplePairs
);
