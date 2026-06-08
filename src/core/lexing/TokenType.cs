namespace QuickApi.Core.Lexing;

public enum TokenType
{
    IDENTIFIER,
    KEYWORD,
    HTTPMETHOD,
    ENTITYATTRIBUTE,

    COLON,
    CURLY_LEFT,
    CURLY_RIGHT,
    SLASH,

    NEWLINE,
    TAB,

    EOF,
}
