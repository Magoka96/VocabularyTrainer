using VocabularyTrainer.Services.Models;

namespace VocabularyTrainer.Services.Interfaces;

public interface ITranslationService
{
    Task<TranslationResult> TranslateAsync(string text, string from, string to, CancellationToken ct = default);
    Task<TranslationResult> LookupAsync(string word, string from, string to, CancellationToken ct = default);
}
