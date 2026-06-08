namespace QuickApi.Core.Lexing;

public enum TokenType
{
    IDENTIFIER,
    KEYWORD,
    HTTPMETHOD,
    ENDPOINTATTRIBUTE,

    COLON,
    CURLY_LEFT,
    CURLY_RIGHT,
    SLASH,

    NEWLINE,
    TAB,

    EOF,
}
