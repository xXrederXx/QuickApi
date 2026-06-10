using QuickApi.Core.Common;
using QuickApi.Core.Lexing;

namespace QuickApi.Core.Ast.Nodes;

public class EntityNode
{
    public required Position StartPos { get; init; }

    public required Token<string> Identifier { get; init; }

    public required EntityField[] EntityFields { get; init; }
}
