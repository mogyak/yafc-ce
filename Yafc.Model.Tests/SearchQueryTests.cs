using System;
using System.Collections.Generic;
using Xunit;

namespace Yafc.Model.Tests;

public class SearchQueryTests {
    [Fact]
    public void Tokens_AreReadOnly() {
        SearchQuery query = new("iron plate");

        Assert.Equal(new[] { "iron", "plate" }, query.tokens);
        Assert.False(query.tokens is string[]);

        IList<string> tokens = Assert.IsAssignableFrom<IList<string>>(query.tokens);
        Assert.True(tokens.IsReadOnly);
        Assert.Throws<NotSupportedException>(() => tokens[0] = "copper");
        Assert.True(query.Match("iron plate"));
        Assert.False(query.Match("copper plate"));
    }

    [Fact]
    public void DefaultQuery_HasEmptyTokens() {
        SearchQuery query = default;

        Assert.True(query.empty);
        Assert.Empty(query.tokens);
        Assert.True(query.Match("anything"));
    }

    [Fact]
    public void Query_TrimsAndNormalizesText() {
        SearchQuery query = new("  \u1112\u1161\u11AB\u1100\u1173\u11AF  ");

        Assert.Equal("한글", query.query);
        Assert.Equal(new[] { "한글" }, query.tokens);
        Assert.True(query.Match("테스트 한글 검색"));
        Assert.True(query.Match("테스트 \u1112\u1161\u11AB\u1100\u1173\u11AF 검색"));
    }

    [Fact]
    public void Query_NormalizesCompatibilityJamo() {
        SearchQuery query = new("  ㅇㅏㄴㄴㅕㅇ  ");

        Assert.Equal("안녕", query.query);
        Assert.Equal(new[] { "안녕" }, query.tokens);
        Assert.True(query.Match("테스트 안녕 검색"));
        Assert.True(query.Match("테스트 ㅇㅏㄴㄴㅕㅇ 검색"));
    }
}
