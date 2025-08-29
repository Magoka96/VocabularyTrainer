using VocabularyTrainer.Web.Services.Models;

namespace VocabularyTrainer.Web.Services.Interfaces;

public interface ITranslationService
{
    Task<TranslationResult> TranslateAsync(string text, string from, string to, CancellationToken ct = default);
    Task<TranslationResult> LookupAsync(string word, string from, string to, CancellationToken ct = default);
}
