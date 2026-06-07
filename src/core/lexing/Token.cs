using QuickApi.Core.Common;

namespace QuickApi.Core.Lexing;

public class Token<T> : BaseToken
{
    public readonly T Value;

    public Token(Position startPosition, TokenType type, T value)
        : base(startPosition, type)
    {
        Value = value;
    }
}
