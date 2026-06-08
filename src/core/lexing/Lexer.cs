using FastEnumUtility;
using QuickApi.Core.Common;

namespace QuickApi.Core.Lexing;

public class Lexer
{
    private const char StopChar = char.MaxValue;
    private readonly string Text;
    private Position Pos;
    private char currentChar;

    public Lexer(string path, string text)
    {
        Text = text;

        byte fileId = FileNameRegistry.GetFileId(path);
        Pos = new Position(0, 0, 0, fileId);
        currentChar = Pos.Index < Text.Length ? Text[Pos.Index] : StopChar;
    }

    public Result<BaseToken[], string> GetTokens()
    {
        List<BaseToken> tokens = [];
        while (currentChar is not StopChar)
        {
            if (LexerHelper.TryParseSingle(currentChar, out TokenType singleType))
            {
                tokens.Add(new BaseToken(Pos, singleType));
                Advance();
            }
            else if (currentChar is ' ')
            {
                Advance();
            }
            else if (char.IsLetter(currentChar))
            {
                tokens.Add(MakeIdentifier());
            }
            else
            {
                return Result<BaseToken[], string>.Fail($"Invalid Char: {currentChar} ({(short)currentChar})");

            }
        }
        tokens.Add(new BaseToken(Pos, TokenType.EOF));
        return Result<BaseToken[], string>.Success(tokens.ToArray());
    }

    private BaseToken MakeIdentifier()
    {
        string identifier = string.Empty;
        Position startPos = Pos;
        while (IsValidIdentifierChar(currentChar))
        {
            identifier += currentChar;
            Advance();
        }
        bool IsKeyword = FastEnum.TryParse(identifier, out KeywordType keywordType);

        return IsKeyword
            ? new Token<KeywordType>(startPos, TokenType.KEYWORD, keywordType)
            : new Token<string>(startPos, TokenType.IDENTIFIER, identifier);
    }

    private static bool IsValidIdentifierChar(char c) => char.IsLetterOrDigit(c) || c == '_';

    private void Advance()
    {
        Pos = Pos.Advance(currentChar);
        currentChar = Pos.Index < Text.Length ? Text[Pos.Index] : StopChar;
    }
}
