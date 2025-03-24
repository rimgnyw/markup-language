class Token
{
    public readonly TokenType type;
    public readonly String lexeme;
    public readonly object literal;

    public Token(TokenType type, String lexeme, object literal)
    {
        this.type = type;
        this.lexeme = lexeme;
        this.literal = literal;

    }
}
