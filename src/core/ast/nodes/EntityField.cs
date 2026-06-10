using QuickApi.Core.Common;
using QuickApi.Core.Lexing;

namespace QuickApi.Core.Ast.Nodes;

public class EntityField
{
    public Position StartPos
    {
        get { return Identifier.StartPosition; }
    }

    public required Token<string> Identifier { get; init; }
    public required Token<EntityFieldDataType> DataType { get; init; }
}
