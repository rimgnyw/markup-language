class Token {
    public readonly TokenType type;
    public readonly object literal;
    public readonly int line;

    public Token(TokenType type, object literal, int line) {
        this.type = type;
        this.literal = literal;
        this.line = line;

    }
    public TokenType getType() { return type; }
    public string getText() { return literal.ToString(); }
}