namespace YAPL.Core.Lexer;

public class Token
{
    public TokenType Type { get; init; }
    public string Value { get; init; }
    public int Line { get; init; }
    public int Column { get; init; }

    public Token(TokenType tokenType, string value, int line, int column)
    {
        Type = tokenType;
        Value = value;
        Line = line;
        Column = column;
    }
}
