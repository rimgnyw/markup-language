class Parser
{
    private Lexer lexer;

    public Parser(Lexer lexer)
    {
        this.lexer = lexer;
    }

    public ParseTree parse()
    {
        ParseTree result = text();
        return result;
    }

    public ParseTree text()
    {
        Token t = lexer.peekToken();
        if (t.getType() == TokenType.STR)
        {
            String text = (String)t.literal;
            Console.WriteLine(t.literal);
            // consume the str token
            lexer.nextToken();
            return new Str(text);
        }
        if (t.getType() == TokenType.ITALIC)
        {
            lexer.nextToken();
            ParseTree child = text();
            if (lexer.nextToken().getType() != TokenType.ITALIC) throw new Exception($"Temp syntax error: expected \"**\" got \"{lexer.peekToken().literal}\" exception");
            return new Italics(child);
        }
        if (t.getType() == TokenType.BOLD)
        {
            lexer.nextToken();
            ParseTree child = text();
            if (lexer.nextToken().getType() != TokenType.BOLD) throw new Exception($"Temp syntax error: expected \"''\" got \"{lexer.peekToken().literal}\" exception");
            return new Bolded(child);
        }
        else
        {
            throw new Exception($"Temp syntax error: what the fuck is a \"{lexer.peekToken().literal}?\"");
        }

    }

}