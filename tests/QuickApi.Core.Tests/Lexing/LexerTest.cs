using FastEnumUtility;
using QuickApi.Core.Lexing;

namespace QuickApi.Core.Tests;

public class LexerTest
{
    [Fact]
    void shouldAddEOF()
    {
        var res = new Lexer("<test>", "GET").GetTokens();

        Assert.True(res.TryGetValue(out var tokens), res.IsFailed ? res.GetError() : null);
        Assert.NotEmpty(tokens);
        Assert.Equal(TokenType.EOF, tokens[^1].type);
    }

    [Theory]
    [MemberData(nameof(KeywordData))]
    void souldLexKeywords(string input, KeywordType keyword)
    {

        var res = new Lexer("<test>", input).GetTokens();
        Assert.True(res.TryGetValue(out var tokens), res.IsFailed ? res.GetError() : null);
        Assert.NotEmpty(tokens);
        var tok = Assert.IsType<Token<KeywordType>>(tokens[0]);
        Assert.Equal(TokenType.KEYWORD, tok.type);
        Assert.Equal(keyword, tok.Value);
    }

    public static TheoryData<string, KeywordType> KeywordData
    {
        get
        {
            TheoryData<string, KeywordType> data = new();
            foreach (KeywordType keyword in FastEnum.GetValues<KeywordType>())
            {
                data.Add(keyword.FastToString(), keyword);
            }
            return data;
        }
    }
}