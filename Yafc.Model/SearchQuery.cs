using System;
using System.Collections.Generic;
using Yafc.Core;

namespace Yafc.Model;

public readonly struct SearchQuery {
    private static readonly IReadOnlyList<string> emptyTokens = Array.AsReadOnly(Array.Empty<string>());
    private readonly string? queryText;
    private readonly IReadOnlyList<string>? queryTokens;

    public SearchQuery(string? query) {
        queryText = NormalizeQuery(query);
        queryTokens = queryText.Length == 0 ? emptyTokens : Array.AsReadOnly(queryText.Split(' ', StringSplitOptions.RemoveEmptyEntries));
    }

    public readonly string query => queryText ?? "";
    public readonly IReadOnlyList<string> tokens => queryTokens ?? emptyTokens;
    public readonly bool empty => tokens.Count == 0;

    public bool Match(string? text) {
        if (text == null) {
            return false;
        }

        if (empty) {
            return true;
        }

        text = TextUtils.NormalizeHangulCompatibilityJamo(text);

        foreach (string token in tokens) {
            if (text.IndexOf(token, StringComparison.OrdinalIgnoreCase) < 0) {
                return false;
            }
        }

        return true;
    }

    private static string NormalizeQuery(string? query) => TextUtils.NormalizeHangulCompatibilityJamo((query ?? "").Trim());
}
