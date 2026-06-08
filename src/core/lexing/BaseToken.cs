using QuickApi.Core.Common;

namespace QuickApi.Core.Lexing;

public class BaseToken
{
    public readonly Position StartPosition;
    public readonly TokenType type;

    public BaseToken(Position startPosition, TokenType type)
    {
        StartPosition = startPosition;
        this.type = type;
    }

    public override bool Equals(object? obj)
    {
        return obj is BaseToken token &&
               StartPosition.Equals(token.StartPosition) &&
               type == token.type;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(StartPosition, type);
    }

    public override string ToString()
    {
        return $"BaseToken({StartPosition}, {type})";
    }
}
