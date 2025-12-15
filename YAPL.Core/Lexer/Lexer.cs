using System.Text;

namespace YAPL.Core.Lexer;

public class Lexer
{
    private readonly string _source;
    private int _position;
    private int _line;
    private int _column;
    private char _currentChar;
    private readonly HashSet<char> _permittedOperators = new HashSet<char>() { '+', '-', '/', '*', '=' };

    public Lexer(string source)
    {
        _source = source;
        _position = 0;
        _line = 1;
        _column = 1;
        _currentChar = _source[0];
    }

    private char PeekOrNull(int index)
    {
        return index < _source.Length ? _source[index] : '\0';
    }

    private void Advance()
    {
        if (_currentChar == '\r' && PeekOrNull(_position + 1) == '\n')
        {
            _position++;
            _currentChar = '\n';
        }

        if (_currentChar == '\n')
        {
            _line++;
            _column = 1;
        }
        else
        {
            _column++;
        }


        _position++;
        _currentChar = _position < _source.Length ? _source[_position] : '\0';
    }


    private void SkipWhitespace()
    {
        while (char.IsWhiteSpace(_currentChar) && _position < _source.Length)
        {
            Advance();
        }
    }

    public Token NextToken()
    {
        SkipWhitespace();

        int startLine = _line;
        int startColumn = _column;

        if (char.IsLetter(_currentChar) || _currentChar == '_')
        {
            return ReadIdentifier();
        }

        if (char.IsDigit(_currentChar))
        {
            return ReadNumber();
        }

        if (_permittedOperators.Contains(_currentChar))
        {
            return ReadOperator();
        }

        if (_currentChar == '\0')
        {
            return new Token(TokenType.EOF, "", startLine, startColumn);
        }

        var ch = _currentChar.ToString();
        Advance();

        return new Token(TokenType.UNKNOWN, ch, startLine, startColumn);
    }

    private Token ReadIdentifier()
    {
        int startLine = _line;
        int startColumn = _column;

        var identifier = new StringBuilder();
        while (char.IsLetterOrDigit(_currentChar) || _currentChar == '_')
        {
            identifier.Append(_currentChar);
            Advance();
        }

        var identifierString = identifier.ToString();
        bool keywordExists = Enum.TryParse(typeof(TokenType), identifierString, true, out object? keyword);

        if (keywordExists)
        {
            return new Token((TokenType)keyword!, keyword.ToString()!, startLine, startColumn);
        }

        return new Token(TokenType.IDENTIFIER, identifierString, startLine, startColumn);
    }

    private Token ReadNumber()
    {
        int startLine = _line;
        int startColumn = _column;

        var number = new StringBuilder();

        while (char.IsDigit(_currentChar))
        {
            number.Append(_currentChar);
            Advance();
        }

        return new Token(TokenType.NUMBER, number.ToString(), startLine, startColumn);
    }

    private Token ReadOperator()
    {
        int startLine = _line;
        int startColumn = _column;

        var current = _currentChar;
        var next = PeekOrNull(_position + 1);

        if (current == '=' && next == '=')
        {
            Advance();
            Advance();

            return new Token(TokenType.DOUBLE_EQUAL, "==", startLine, startColumn);
        }

        Advance();

        return current switch
        {
            '+'=> new Token(TokenType.PLUS, "+", startLine, startColumn),
            '-' => new Token(TokenType.MINUS, "-", startLine, startColumn),
            '*' => new Token(TokenType.STAR, "*", startLine, startColumn),
            '/' => new Token(TokenType.SLASH, "/", startLine, startColumn),
            '=' => new Token(TokenType.EQUAL, "=", startLine, startColumn),
            _ => new Token(TokenType.UNKNOWN, current.ToString(), startLine, startColumn),
        };
    }
}
