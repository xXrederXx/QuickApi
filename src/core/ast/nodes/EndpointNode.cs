using QuickApi.Core.Common;
using QuickApi.Core.Lexing;

namespace QuickApi.Core.Ast.Nodes;

public class EndpointNode
{
    public Position StartPos { get; init; }

    public Token<HttpMethodType> MethodToken { get; init; }
    public PathNode PathNode { get; init; }
    public EndpointAttribute[] Attributes { get; init; }
}