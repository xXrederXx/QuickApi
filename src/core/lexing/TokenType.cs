namespace QuickApi.Core.Lexing;

public enum TokenType
{
    IDENTIFIER,
    KEYWORD,

    COLON,
    CURLY_LEFT,
    CURLY_RIGHT,
    SLASH,

    NEWLINE,

    EOF,
}
