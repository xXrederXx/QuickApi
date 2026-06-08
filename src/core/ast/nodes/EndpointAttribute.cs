using QuickApi.Core.Common;
using QuickApi.Core.Lexing;

namespace QuickApi.Core.Ast.Nodes;

public class EndpointAttribute
{
    public Position StartPos { get; init; }

    public Token<KeywordType> KeyToken { get; init; }
    public Token<string> ValueToken { get; init; }

}

