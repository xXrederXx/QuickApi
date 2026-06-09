using QuickApi.Core.Ast.Nodes;
using QuickApi.Core.Common;
using QuickApi.Core.Lexing;

namespace QuickApi.Core.Ast;

public class Parser
{
    private BaseToken[] Tokens { get; init; }
    private BaseToken currentToken;
    private int currentIndex = -1;

    public Parser(BaseToken[] tokens)
    {
        Tokens = tokens;
    }

    public void ParseTokens() { }

    private Result<EndpointNode, Error> ParseEndpoint()
    {
        if (
            currentToken.type is not TokenType.HTTPMETHOD
            || currentToken is not Token<HttpMethodType> methodTok
        )
        {
            return Result<EndpointNode, Error>.Fail(new ExpectedHTTPMethodError(currentToken.StartPosition));
        }
        Advance();
        return Result<EndpointNode, string>.Fail("Not Implemented");
    }

    private Result<PathNode, Error> ParsePath()
    {
        List<PathElement> elements = [];
        bool lastWasSlash = false;
        while (currentToken.type is not (TokenType.NEWLINE or TokenType.EOF))
        {
            if (currentToken.type is TokenType.SLASH)
            {
                if (lastWasSlash)
                {
                    return Result<PathNode, Error>.Fail(new InvalidPathError(currentToken.StartPosition, "A path can not contain 2 Slashes like this //"));

                }
                lastWasSlash = true;
                Advance();
                continue;
            }
            lastWasSlash = false;

            if (currentToken.type is TokenType.CURLY_LEFT)
            {
                var result = ParseVariablePathElement();
                if (result.TryGetError(out Error error))
                {
                    return Result<PathNode, Error>.Fail(error);
                }
                elements.Add(result.GetValue());
            }

            if (currentToken.type is not TokenType.IDENTIFIER
                    || currentToken is not Token<string> identifierTok)
            {
                return Result<PathNode, Error>.Fail(new ExpectedIdentifierError(currentToken.StartPosition));

            }
            elements.Add(new PathElement()
            {
                Identifier = identifierTok,
                Type = PathElement.PathElementType.Absolute
            });
        }
        return Result<PathNode, Error>.Success(new PathNode()
        {
            Elements = elements.ToArray()
        });
    }

    private Result<PathElement, Error> ParseVariablePathElement()
    {
        if (currentToken.type is not TokenType.CURLY_LEFT)
        {
            return Result<PathElement, Error>.Fail(new ExpectedSymbolError(currentToken.StartPosition, "{"));
        }
        Advance();
        if (currentToken.type is not TokenType.IDENTIFIER
                    || currentToken is not Token<string> identifierTok)
        {
            return Result<PathElement, Error>.Fail(new ExpectedIdentifierError(currentToken.StartPosition));

        }
        Advance();
        if (currentToken.type is not TokenType.CURLY_RIGHT)
        {
            return Result<PathElement, Error>.Fail(new ExpectedSymbolError(currentToken.StartPosition, "}"));
        }
        return Result<PathElement, Error>.Success(new PathElement()
        {
            Identifier = identifierTok,
            Type = PathElement.PathElementType.Variable
        });
    }

    private void Advance()
    {
        currentIndex++;
        if (currentIndex < Tokens.Length)
        {
            currentToken = Tokens[currentIndex];
        }
    }
}
