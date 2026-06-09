using QuickApi.Core.Lexing;

namespace QuickApi.Core.Common;

public abstract class Error(int errorCode, string message, Position startPos)
{
    public bool IsError => this is not ErrorNull;
    protected Position StartPos => startPos;

    public override string ToString() =>
        $"{FileNameRegistry.GetFileName(startPos.FileId)}({startPos.Line + 1}, {startPos.Column + 1}): ERROR QA{errorCode:D4} {message}";

    public static implicit operator string(Error? self)
    {
        return self is null ? "NULL ERROR" : self.ToString();
    }
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
    : Error(0001, $"Illegal character '{illegalChar}'", startPos) { }

public class ExpectedHTTPMethodError(Position startPos)
    : Error(0001, $"Expected HTTP Method", startPos) { }

public class ExpectedSymbolError(Position startPos, string symbol)
    : Error(0002, $"Expected Symbol '{symbol}'", startPos) { }

public class ExpectedIdentifierError(Position startPos)
    : Error(0003, $"Expected an Identifer", startPos) { }

public class InvalidPathError(Position startPos, string reason) : Error(0004, reason, startPos) { }

public class ExpectedNewLineError(Position startPos)
    : Error(0005, "Expected a Newline", startPos) { }

public class ExpectedEndpointAttributeError(Position startPos)
    : Error(0006, $"Expected Endpoint Attribute", startPos) { }
