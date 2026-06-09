using QuickApi.Core.Common;

namespace QuickApi.Core.Ast.Nodes;

public class PathNode
{
    public Position StartPos
    {
        get
        {
            var element = Elements.FirstOrDefault() ?? throw new InvalidOperationException("Cant access the start pos of empty path node");
            return element.StartPos;
        }
    }

    public PathElement[] Elements { get; init; }

}

