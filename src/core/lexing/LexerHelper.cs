namespace QuickApi.Core.Lexing;

public static class LexerHelper
{
    private static readonly Dictionary<char, TokenType> SingleMap = new Dictionary<char, TokenType>() {
        {':', TokenType.COLON},
        {'/', TokenType.SLASH},
        {'{', TokenType.CURLY_LEFT},
        {'}', TokenType.CURLY_RIGHT}
    };

    public static bool TryParseSingle(char input, out TokenType type)
        => SingleMap.TryGetValue(input, out type);
}