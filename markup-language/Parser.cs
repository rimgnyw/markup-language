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
        else if (token.getType() == ITALIC || token.getType() == BOLD)
            result = formattedText();
        else {
            throw new NotImplementedException("invalid token error: " + token.getType());
        }

        return result;
    }

    public ParseTree formattedText() {
        ParseTree result;
        Token token = previousToken();
        if (token.getType() == ITALIC) {
            if (peekToken().getType() == ITALIC) throw new NotImplementedException($"too many italic error {token.line}");
            ParseTree content = textSegment();
            while (peekToken().getType() != ITALIC && peekToken().getType() != EOF) {
                if (peekToken().getType() == NL) {
                    nextToken(); // consume the new line
                    ParseTree next = textSegment();
                    content = new NewLine(content, next);
                }
                else {
                    ParseTree next = textSegment();
                    content = new Node(content, next);
                }
            }
            if (nextToken().getType() != ITALIC) throw new NotImplementedException($"missing italic error {token.line}");
            result = new Italic(content);
        }
        else if (token.getType() == BOLD) {
            if (peekToken().getType() == BOLD) throw new NotImplementedException($"too many bolded error {token.line}");
            ParseTree content = textSegment();
            while (peekToken().getType() != BOLD && peekToken().getType() != EOF) {
                if (peekToken().getType() == NL) {
                    nextToken(); // consume the new line
                    ParseTree next = textSegment();
                    content = new NewLine(content, next);
                }
                else {
                    ParseTree next = textSegment();
                    content = new Node(content, next);
                }
            }
            if (nextToken().getType() != BOLD) throw new NotImplementedException($"missing bolded error {peekToken().line},{peekToken().getType()}");
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
    public Token previousToken() {
        if (current - 1 < 0) throw new Exception("No previous token");
        return tokens[current - 1];
    }


}