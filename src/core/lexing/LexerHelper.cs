using System.Collections.Immutable;

namespace QuickApi.Core.Lexing;

public static class LexerHelper
{
    public static readonly ImmutableDictionary<char, TokenType> SingleMap = new Dictionary<char, TokenType>() {
        {':', TokenType.COLON},
        {'/', TokenType.SLASH},
        {'{', TokenType.CURLY_LEFT},
        {'}', TokenType.CURLY_RIGHT},
        {'\n', TokenType.NEWLINE},
        {'\r', TokenType.NEWLINE},
        {'\t', TokenType.TAB},
    }.ToImmutableDictionary();

    public static bool TryParseSingle(char input, out TokenType type)
        => SingleMap.TryGetValue(input, out type);
}