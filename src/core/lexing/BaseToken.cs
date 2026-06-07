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
}
