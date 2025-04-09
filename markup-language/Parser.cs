using static TokenType;
class Parser {
    private int current = 0;
    private readonly List<Token> tokens = new List<Token>();

    public Parser(List<Token> tokens) {
        this.tokens = tokens;
    }

    public ParseTree parse() {
        ParseTree result = S();
        return result;
    }

    public ParseTree S() {
        ParseTree result = textSegment();

        while (peekToken().getType() != EOF) {
            if (peekToken().getType() == NL) {
                nextToken(); // consume the new line
                ParseTree next = textSegment();
                result = new NewLine(result, next);
            }
            else {
                ParseTree next = textSegment();
                result = new Node(result, next);
            }
        }
        return result;
    }
    public ParseTree textSegment() {
        ParseTree result;
        Token token = nextToken();
        if (token.getType() == TEXT)
            result = new Text(token.getText());
        else if (token.getType() == ITALIC) {
            if (peekToken().getType() == ITALIC) throw new NotImplementedException("too many italic error");
            ParseTree content = textSegment();
            if (nextToken().getType() != ITALIC) throw new NotImplementedException("missing italic error");
            result = new Italic(content);
        }
        else if (token.getType() == BOLD) {
            if (peekToken().getType() == BOLD) throw new NotImplementedException("too many italic error");
            ParseTree content = textSegment();
            if (nextToken().getType() != BOLD) throw new NotImplementedException("missing italic error");
            result = new Bold(content);
        }
        else {
            throw new NotImplementedException("invalid token error: " + token.getType());
        }

        return result;
    }

    public Token peekToken() {
        if (current > tokens.Count - 1) throw new Exception("I am going to kill myself");
        return tokens[current];
    }
    public Token nextToken() {
        Token result = peekToken();
        current++;
        return result;
    }

}