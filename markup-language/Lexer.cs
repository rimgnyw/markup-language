using static TokenType;
using System.Text.RegularExpressions;

class Lexer {
    private int start = 0;
    private int current = 0;
    private int line = 1;
    private String source;
    private readonly List<Token> tokens = new List<Token>();

    public Lexer(String source) {
        this.source = source.Replace("\r\n", "\n"); // sanitise input to avoid windows garbash
        scanTokens();
    }

    public List<Token> scanTokens() {
        while (!endOfLine()) {
            start = current;
            scanToken();
        }
        tokens.Add(new Token(EOF, "", line));
        return tokens;
    }


    private void scanToken() {
        char c = advance();

        switch (c) {
            case '*':
                if (match('*')) addToken(ITALIC);
                else text(); break;
            case '\'':
                if (match('\'')) addToken(BOLD);
                else text(); break;
            case '\n':
                line++;
                addToken(NL); break;
            default:
                text();
                break;

        }
    }

    // FIXED?  \n is a one letter symbol but we're looking for a two letter symbol (I assume this worked because of the \r\n from before)
    private void text() {
        Regex rg = new Regex(@"(\*\*)|(\'\')");
        while (!rg.IsMatch(peek().ToString() + peekNext().ToString()) && peek().ToString() != "\n" && !endOfLine()) {
            advance();
        }

        if (endOfLine()) {
            String v = source.Substring(start, current - start);
            // Console.WriteLine(v);
            addToken(TEXT, v);
            return;
        }
        if (peek().ToString() == "\n") {
            String v = source.Substring(start, current - start);
            // Console.WriteLine(v);
            addToken(TEXT, v);
            return;

        }

        // detected markdown symbol
        String value = source.Substring(start, current - start);// remove trailing marker

        // Console.WriteLine(value);
        addToken(TEXT, value);

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
        addToken(type, "");
    }
    private void addToken(TokenType type, string literal) {

        tokens.Add(new Token(type, literal, line));
    }
}