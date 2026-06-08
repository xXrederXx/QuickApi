using QuickApi.Core.Common;

namespace QuickApi.Core.Ast.Nodes;

public class PathNode
{
    public Position StartPos { get; init; }

    public PathElement[] elements { get; init; }

}

