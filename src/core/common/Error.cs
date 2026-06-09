namespace QuickApi.Core.Common;

public abstract class Error(int errorCode, string message, Position startPos)
{
    public bool IsError => this is not ErrorNull;
    protected Position StartPos => startPos;

    public override string ToString() =>
        $"{FileNameRegistry.GetFileName(startPos.FileId)}({startPos.Line + 1}, {startPos.Column + 1}): ERROR YS{errorCode:D4} {message}";
}

// This is NoError, used instead of error = null
public class ErrorNull : Error
{
    public static readonly ErrorNull Instance = new();

    private ErrorNull()
        : base(0, string.Empty, Position.Null) { }

    public override string ToString() => string.Empty;
}

public class IllegalCharError(Position startPos, char illegalChar)
    : Error(0001, $"Illegal character '{illegalChar}'", startPos)
{ }