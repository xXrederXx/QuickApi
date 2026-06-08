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

    [Theory]
    [MemberData(nameof(SingleType))]
    void souldLexSingleType(char input, TokenType type)
    {

        var res = new Lexer("<test>", $"{input}").GetTokens();
        Assert.True(res.TryGetValue(out var tokens), res.IsFailed ? res.GetError() : null);
        Assert.NotEmpty(tokens);
        Assert.Equal(type, tokens[0].type);
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

    public static TheoryData<char, TokenType> SingleType
    {
        get
        {
            TheoryData<char, TokenType> data = new();
            foreach (KeyValuePair<char, TokenType> kvp in LexerHelper.SingleMap)
            {
                data.Add(kvp.Key, kvp.Value);
            }
            return data;
        }
    }
}