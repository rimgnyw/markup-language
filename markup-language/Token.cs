class Token {
    public readonly TokenType type;
    public readonly string literal;
    public readonly int line;

    public Token(TokenType type, string literal, int line) {
        this.type = type;
        this.literal = literal;
        this.line = line;

    }
    public TokenType getType() { return type; }
    public string getText() { return literal.ToString(); }
}