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
        Advance();
    }

    public void ParseTokens() { }

    public Result<EntityNode, Error> ParseEntity()
    {
        if (
            currentToken.type is not TokenType.KEYWORD
            || currentToken is not Token<KeywordType> keywordTok
            || keywordTok.Value != KeywordType.ENTITY
        )
        {
            return Result<EntityNode, Error>.Fail(
                new ExpectedKeywordError(currentToken.StartPosition, KeywordType.ENTITY)
            );
        }

        Position startPos = currentToken.StartPosition;

        Advance();

        if (
            currentToken.type is not TokenType.IDENTIFIER
            || currentToken is not Token<string> identifierTok
        )
        {
            return Result<EntityNode, Error>.Fail(
                new ExpectedIdentifierError(currentToken.StartPosition)
            );
        }

        if (currentToken.type is not TokenType.NEWLINE)
        {
            return Result<EntityNode, Error>.Fail(
                new ExpectedNewLineError(currentToken.StartPosition)
            );
        }
        SkipNewLines();

        List<EntityField> entityAttributes = [];
        while (currentToken.type is TokenType.TAB)
        {
            Advance();

            if (
                currentToken.type is not TokenType.IDENTIFIER
                || currentToken is not Token<string> attributeIdentifierTok
            )
            {
                return Result<EntityNode, Error>.Fail(
                    new ExpectedIdentifierError(currentToken.StartPosition)
                );
            }

            Advance();

            if (
                currentToken.type is not TokenType.ENTITYFIELDDATATYPE
                || currentToken is not Token<EntityFieldDataType> dataTypeTok
            )
            {
                return Result<EntityNode, Error>.Fail(
                    new ExpectedIdentifierError(currentToken.StartPosition)
                );
            }

            if (currentToken.type is not TokenType.NEWLINE)
            {
                return Result<EntityNode, Error>.Fail(
                    new ExpectedNewLineError(currentToken.StartPosition)
                );
            }

            SkipNewLines();

            entityAttributes.Add(
                new EntityField() { Identifier = attributeIdentifierTok, DataType = dataTypeTok }
            );
        }

        return Result<EntityNode, Error>.Success(
            new EntityNode()
            {
                Identifier = identifierTok,
                StartPos = startPos,
                EntityFields = entityAttributes.ToArray(),
            }
        );
    }

    private Result<EndpointNode, Error> ParseEndpoint()
    {
        if (
            currentToken.type is not TokenType.HTTPMETHOD
            || currentToken is not Token<HttpMethodType> methodTok
        )
        {
            return Result<EndpointNode, Error>.Fail(
                new ExpectedHTTPMethodError(currentToken.StartPosition)
            );
        }
        Advance();
        var pathResult = ParsePath();
        if (pathResult.TryGetError(out Error pathError))
        {
            return Result<EndpointNode, Error>.Fail(pathError);
        }

        if (currentToken.type is not TokenType.NEWLINE)
        {
            return Result<EndpointNode, Error>.Fail(
                new ExpectedNewLineError(currentToken.StartPosition)
            );
        }
        SkipNewLines();

        List<EndpointAttribute> endpointAttributes = [];
        while (currentToken.type is TokenType.TAB)
        {
            Advance();

            if (
                currentToken.type is not TokenType.ENDPOINTATTRIBUTE
                || currentToken is not Token<EndpointAttributeType> endpointAttributeType
            )
            {
                return Result<EndpointNode, Error>.Fail(
                    new ExpectedEndpointAttributeError(currentToken.StartPosition)
                );
            }

            Advance();

            if (
                currentToken.type is not TokenType.IDENTIFIER
                || currentToken is not Token<string> identifierTok
            )
            {
                return Result<EndpointNode, Error>.Fail(
                    new ExpectedIdentifierError(currentToken.StartPosition)
                );
            }

            Advance();

            if (currentToken.type is not TokenType.NEWLINE)
            {
                return Result<EndpointNode, Error>.Fail(
                    new ExpectedNewLineError(currentToken.StartPosition)
                );
            }

            SkipNewLines();

            endpointAttributes.Add(
                new EndpointAttribute()
                {
                    KeyToken = endpointAttributeType,
                    ValueToken = identifierTok,
                }
            );
        }

        return Result<EndpointNode, Error>.Success(
            new EndpointNode()
            {
                MethodToken = methodTok,
                PathNode = pathResult.GetValue(),
                Attributes = endpointAttributes.ToArray(),
            }
        );
    }

    private void SkipNewLines()
    {
        while (currentToken.type is TokenType.NEWLINE)
        {
            Advance();
        }
    }

    private Result<PathNode, Error> ParsePath()
    {
        List<PathElement> elements = [];
        bool lastWasSlash = false;
        while (currentToken.type != TokenType.NEWLINE && currentToken.type != TokenType.EOF)
        {
            if (currentToken.type is TokenType.SLASH)
            {
                if (lastWasSlash)
                {
                    return Result<PathNode, Error>.Fail(
                        new InvalidPathError(
                            currentToken.StartPosition,
                            "A path can not contain 2 Slashes like this //"
                        )
                    );
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
                continue;
            }

            if (
                currentToken.type is not TokenType.IDENTIFIER
                || currentToken is not Token<string> identifierTok
            )
            {
                return Result<PathNode, Error>.Fail(
                    new ExpectedIdentifierError(currentToken.StartPosition)
                );
            }
            Advance();
            elements.Add(
                new PathElement()
                {
                    Identifier = identifierTok,
                    Type = PathElement.PathElementType.Absolute,
                }
            );
        }
        return Result<PathNode, Error>.Success(new PathNode() { Elements = elements.ToArray() });
    }

    private Result<PathElement, Error> ParseVariablePathElement()
    {
        if (currentToken.type is not TokenType.CURLY_LEFT)
        {
            return Result<PathElement, Error>.Fail(
                new ExpectedSymbolError(currentToken.StartPosition, "{")
            );
        }
        Advance();
        if (
            currentToken.type is not TokenType.IDENTIFIER
            || currentToken is not Token<string> identifierTok
        )
        {
            return Result<PathElement, Error>.Fail(
                new ExpectedIdentifierError(currentToken.StartPosition)
            );
        }
        Advance();
        if (currentToken.type is not TokenType.CURLY_RIGHT)
        {
            return Result<PathElement, Error>.Fail(
                new ExpectedSymbolError(currentToken.StartPosition, "}")
            );
        }
        Advance();
        return Result<PathElement, Error>.Success(
            new PathElement()
            {
                Identifier = identifierTok,
                Type = PathElement.PathElementType.Variable,
            }
        );
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
