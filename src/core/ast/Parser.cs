using QuickApi.Core.Lexing;

namespace QuickApi.Core.Ast;

public class Parser
{
    private BaseToken[] Tokens { get; init; }

    public Parser(BaseToken[] tokens)
    {
        Tokens = tokens;
    }

    private int currentIndex = -1;

    
    public static void ParseTokens()
    {
        
    }
}