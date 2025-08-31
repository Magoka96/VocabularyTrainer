using VocabularyTrainer.Application.Models;

namespace VocabularyTrainer.Application.Interfaces;

public interface ITranslationService
{
    Task<TranslationResult> TranslateAsync(string text, string from, string to, CancellationToken ct = default);
    Task<TranslationResult> LookupAsync(string word, string from, string to, CancellationToken ct = default);
}
