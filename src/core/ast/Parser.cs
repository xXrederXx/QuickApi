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

    private Result<EndpointNode, string> ParseEndpoint()
    {
        if (
            currentToken.type is not TokenType.HTTPMETHOD
            || currentToken is not Token<HttpMethodType> methodTok
        )
        {
            return Result<EndpointNode, string>.Fail("Token must be of type HTTP METHOD");
        }
        Advance();
        return Result<EndpointNode, string>.Fail("Not Implemented");
    }

    private Result<PathNode, string> ParsePath()
    {
        List<PathElement> elements = [];
        while (currentToken.type is not TokenType.NEWLINE or TokenType.EOF)
        {
            if (currentToken.type is TokenType.SLASH)
            {
                Advance();
                continue;
            }

            if (currentToken.type is TokenType.CURLY_LEFT)
            {
                Advance();
                if (
                    currentToken.type is not TokenType.IDENTIFIER
                    || currentToken is not Token<string> identifierTok
                )
                {
                    return Result<PathNode, string>.Fail("Token must be of type Identifier");
                }
                Advance();
                // symbol }
                Advance();

                elements.Add(
                    new PathElement()
                    {
                        Type = PathElement.PathElementType.Variable,
                        Identifier = identifierTok,
                    }
                );
                continue;
            }
        }
        return Result<PathNode, string>.Fail("Not Implemented");
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
