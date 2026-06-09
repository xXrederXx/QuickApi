using QuickApi.Core.Common;
using QuickApi.Core.Lexing;

namespace QuickApi.Core.Ast.Nodes;

public class EndpointNode
{
    public Position StartPos
    {
        get => MethodToken.StartPosition;
    }

    public required Token<HttpMethodType> MethodToken { get; init; }
    public required PathNode PathNode { get; init; }
    public required EndpointAttribute[] Attributes { get; init; }
}
