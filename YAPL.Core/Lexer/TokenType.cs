namespace YAPL.Core.Lexer;

public enum TokenType
{
    LET,
    PRINT,

    IDENTIFIER,

    NUMBER,
    STRING,

    PLUS,
    MINUS,
    STAR,
    SLASH,
    EQUAL,
    DOUBLE_EQUAL,

    EOF,
    UNKNOWN
}