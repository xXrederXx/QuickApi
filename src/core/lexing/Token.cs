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

    public override bool Equals(object? obj)
    {
        return obj is Token<T> token &&
               base.Equals(obj) &&
               EqualityComparer<T>.Default.Equals(Value, token.Value);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Value);
    }

    public override string ToString()
    {
        return $"Token<{typeof(T)}>({StartPosition}, {type}, {Value})";
    }
}
