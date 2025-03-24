using static TokenType;
using System.Text.RegularExpressions;

class Scanner
{
    private int start = 0;
    private int current = 0;
    private int line = 1;
    private readonly String source;
    private readonly List<Token> tokens = new List<Token>();

    public Scanner(String source)
    {
        this.source = source;
    }

    public List<Token> scanTokens()
    {
        while (!endOfLine())
        {
            start = current;
            scanToken();
        }
        tokens.Add(new Token(EOF, "", null));
        return tokens;
    }


    private void scanToken()
    {
        char c = advance();

        switch (c)
        {
            case '*':
                if (match('*')) addToken(ITALIC);
                else str(); break;
            case '\'':
                if (match('\'')) addToken(BOLD);
                else str(); break;
            default:
                str();
                break;

        }
    }


    private void str()
    {
        Regex rg = new Regex(@"[(\n)(\*)(\')]");
        while (!rg.IsMatch(peek().ToString()) && !endOfLine())
            advance();

        if (endOfLine())
        {
            String v = source.Substring(start, current - start - 1);
            addToken(STR, v);
            return;
        }

        // detected markdown symbol

        String value = source.Substring(start, current - start - 1);
        addToken(STR, value);

    }

    private bool match(char expected)
    {
        if (endOfLine()) return false;
        if (source[current] != expected) return false;
        current++;
        return true;
    }

    private char peek()
    {
        if (endOfLine()) return '\0';
        return source[current];
    }

    private bool endOfLine()
    {
        return current >= source.Length;
    }

    private char advance()
    {
        return source[current++];
    }
    private void addToken(TokenType type)
    {
        addToken(type, null);
    }
    private void addToken(TokenType type, object literal)
    {

        String text = source.Substring(start, current - start);
        tokens.Add(new Token(type, text, literal));
    }
}
