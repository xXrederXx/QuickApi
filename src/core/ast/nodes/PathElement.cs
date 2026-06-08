using QuickApi.Core.Common;
using QuickApi.Core.Lexing;

namespace QuickApi.Core.Ast.Nodes;

public class PathElement
{
    public Position StartPos { get; init; }
    public enum PathElementType
    {
        Absolute, Variable
    }
    public PathElementType Type { get; init; }
    public Token<string> Identifier { get; init; }
}

