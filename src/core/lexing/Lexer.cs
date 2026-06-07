using QuickApi.Core.Common;

namespace QuickApi.Core.Lexing;

public class Lexer
{
    private const char StopChar = char.MaxValue;
    private readonly string Text;
    private Position Pos;
    private char currentChar => Pos.Index < Text.Length ? Text[Pos.Index] : StopChar;


    public Lexer(string path)
    {
        Text = File.ReadAllText(path);

        byte fileId = FileNameRegistry.GetFileId(path);
        Pos = new Position(0, 0, 0, fileId);
    }

    public Result<BaseToken[], string> GetTokens()
    {
        return Result<BaseToken[], string>.Fail("Not Implemented");
    }
}
