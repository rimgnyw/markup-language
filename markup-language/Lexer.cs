using static TokenType;
using System.Text.RegularExpressions;

class Lexer {
    private int start = 0;
    private int current = 0;
    private int line = 1;
    private int currentToken = 0;
    private readonly String source;
    private readonly List<Token> tokens = new List<Token>();

    public Lexer(String source) {
        this.source = source;
        scanTokens();
    }

    public List<Token> scanTokens() {
        while (!endOfLine()) {
            start = current;
            scanToken();
        }
        tokens.Add(new Token(EOF, "", null));
        return tokens;
    }


    private void scanToken() {
        char c = advance();

        switch (c) {
            case '*':
                if (match('*')) addToken(ITALIC);
                else str(); break;
            case '\'':
                if (match('\'')) addToken(BOLD);
                else str(); break;
            case '\n':
                line++;
                addToken(NL); break;
            default:
                str();
                break;

        }
    }


    private void str() {
        Regex rg = new Regex(@"(\n)|(\*\*)|(\'\')");
        while (!rg.IsMatch(peek().ToString() + peekNext().ToString()) && !endOfLine()) {
            advance();
        }

        if (endOfLine()) {
            String v = source.Substring(start, current - start);
            addToken(STR, v);
            return;
        }

        // detected markdown symbol
        String value = source.Substring(start, current - start);// remove trailing marker

        addToken(STR, value);

    }

    private bool match(char expected) {
        if (endOfLine()) return false;
        if (source[current] != expected) return false;
        current++;
        return true;
    }

    private char peek() {
        if (endOfLine()) return '\0';
        return source[current];
    }
    private char peekNext() {
        if (current + 1 > source.Length - 1)
            return '\0';
        return source[current + 1];
    }

    private bool endOfLine() {
        return current >= source.Length;
    }

    private char advance() {
        return source[current++];
    }
    private void addToken(TokenType type) {
        addToken(type, null);
    }
    private void addToken(TokenType type, object literal) {

        String text = source.Substring(start, current - start);
        tokens.Add(new Token(type, text, literal));
    }
    public Token peekToken() {
        if (currentToken > tokens.Count - 1) throw new Exception("I am going to kill myself");
        return tokens[currentToken];
    }
    public Token nextToken() {
        Token result = peekToken();
        currentToken++;
        return result;
    }
}