using Microsoft.Extensions.Options;
using System.Text.Json;
using VocabularyTrainer.Configuration;
using VocabularyTrainer.Services.Interfaces;
using VocabularyTrainer.Services.Models;

namespace VocabularyTrainer.Services;

public sealed class TranslationService(HttpClient http, IOptions<TranslatorOptions> options) : ITranslationService
{
    private readonly HttpClient _http = http;
    private readonly TranslatorOptions _options = options.Value;

    public async Task<TranslationResult> TranslateAsync(string text, string from, string to, CancellationToken ct = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, $"{_options.Endpoint.TrimEnd('/')}/translate?api-version=3.0&from={from}&to={to}");

        AddHeaders(request);

        request.Content = JsonContent.Create(new[] { new { Text = text } });

        using var result = await _http.SendAsync(request, ct);
        result.EnsureSuccessStatusCode();

        using var document = await JsonDocument.ParseAsync(await result.Content.ReadAsStreamAsync(ct), cancellationToken: ct);

        var translated = document.RootElement[0].GetProperty("translations")[0].GetProperty("text").GetString()!;

        return new TranslationResult(text, translated, from, to);
    }

    public async Task<TranslationResult> LookupAsync(string word, string from, string to, CancellationToken ct = default)
    {        
        var alternativesResult = await GetAlternativesForSearchedWord(word, from, to, ct);
        var examplesResult = await GetExamplesForSearchedWord(alternativesResult.BestNormalizedTarget, word, from, to, ct);

        return new TranslationResult(word,
            Target: alternativesResult.Alternatives.FirstOrDefault() ?? string.Empty,
            From: from, To: to,
            Pos: alternativesResult.Pos,
            Alternatives: alternativesResult.Alternatives,
            ExamplePairs: examplesResult.ExamplePairs);
    }

    private async Task<WordAlternativesResult> GetAlternativesForSearchedWord(string word, string from, string to, CancellationToken ct = default)
    {
        using var lookupReq = new HttpRequestMessage(HttpMethod.Post, $"{_options.Endpoint.TrimEnd('/')}/dictionary/lookup?api-version=3.0&from={from}&to={to}");

        AddHeaders(lookupReq);

        lookupReq.Content = JsonContent.Create(new[] { new { Text = word } });

        using var lookupResult = await _http.SendAsync(lookupReq, ct);
        lookupResult.EnsureSuccessStatusCode();

        using var lookupDocument = await JsonDocument.ParseAsync(await lookupResult.Content.ReadAsStreamAsync(ct), cancellationToken: ct);
        var translations = lookupDocument.RootElement[0].GetProperty("translations").EnumerateArray().ToList();

        // Part of Speech, such as noun, verb etc.
        string? pos = null;
        string? bestNormalizedTarget = null;

        if (translations.Count > 0)
        {
            var best = translations[0];
            if (best.TryGetProperty("posTag", out var posEl))
                pos = posEl.GetString();
            if (best.TryGetProperty("normalizedTarget", out var normEl))
                bestNormalizedTarget = normEl.GetString();
        }

        var alternatives = translations
            .Select(t => t.GetProperty("displayTarget").GetString()!)
            .Distinct()
            .Where(s => !string.Equals(s, bestNormalizedTarget, StringComparison.OrdinalIgnoreCase))
            .ToList();


        return new WordAlternativesResult(alternatives, pos, bestNormalizedTarget);
    }

    private async Task<WordExamplesResult> GetExamplesForSearchedWord(string? bestNormalizedTarget, string word, string from, string to, CancellationToken ct)
    {
        var examplePairs = new List<(string Source, string Target)>();

        if (string.IsNullOrWhiteSpace(bestNormalizedTarget))
            return new WordExamplesResult(examplePairs);

        using var exReq = new HttpRequestMessage(HttpMethod.Post,
            $"{_options.Endpoint.TrimEnd('/')}/dictionary/examples?api-version=3.0&from={from}&to={to}");

        AddHeaders(exReq);
        exReq.Content = JsonContent.Create(new[] { new { Text = word, Translation = bestNormalizedTarget } });

        using var exRes = await _http.SendAsync(exReq, ct);
        if (!exRes.IsSuccessStatusCode)
            return new WordExamplesResult(examplePairs);

        using var exDoc = await JsonDocument.ParseAsync(await exRes.Content.ReadAsStreamAsync(ct), cancellationToken: ct);
        foreach (var ex in exDoc.RootElement[0].GetProperty("examples").EnumerateArray())
        {
            var src = (ex.GetProperty("sourcePrefix").GetString() ?? "")
                    + (ex.GetProperty("sourceTerm").GetString() ?? "")
                    + (ex.GetProperty("sourceSuffix").GetString() ?? "");
            var tgt = (ex.GetProperty("targetPrefix").GetString() ?? "")
                    + (ex.GetProperty("targetTerm").GetString() ?? "")
                    + (ex.GetProperty("targetSuffix").GetString() ?? "");
            examplePairs.Add((src, tgt));
        }

        return new WordExamplesResult(examplePairs);
    }

    private void AddHeaders(HttpRequestMessage req)
    {
        req.Headers.TryAddWithoutValidation("Ocp-Apim-Subscription-Key", _options.Key);
        req.Headers.TryAddWithoutValidation("Ocp-Apim-Subscription-Region", _options.Region);
    }
}

