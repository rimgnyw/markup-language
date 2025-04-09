class Parser
{
    private Lexer lexer;

    public Parser(Lexer lexer)
    {
        this.lexer = lexer;
    }

    public ParseTree parse()
    {
        ParseTree result = line();//S();
        return result;
    }

    /*     public ParseTree S()
        {
            ParseTree result = text();
            Console.WriteLine("one " + result.process());
            // Console.WriteLine("The token is " + lexer.peekToken().getType());
            if (lexer.peekToken().getType() == TokenType.NL)
            {
                lexer.nextToken(); // consume NL
                ParseTree next = S();
                Console.WriteLine("two " + result.process());
                result = new NewLine(result, next);
            }
            else if (lexer.peekToken().getType() == TokenType.STR
                   || lexer.peekToken().getType() == TokenType.ITALIC
                    || lexer.peekToken().getType() == TokenType.BOLD)
            {
                ParseTree next = S();
                Console.WriteLine("three " + result.process());
                result = new Line(result, next);
            }
            return result;

        } */

    public ParseTree line()
    {
        ParseTree result = text();
        while (lexer.peekToken().getType() == TokenType.ITALIC ||
               lexer.peekToken().getType() == TokenType.BOLD)
        {
            ParseTree next = text();
            result = new Line(result, next);
        }
        return result;
    }

    public ParseTree text()
    {
        Token t = lexer.peekToken();
        if (t.getType() == TokenType.STR)
        {
            String text = (String)t.literal;
            // Console.WriteLine("literally " + t.literal);
            // consume the str token
            lexer.nextToken();
            return new Str(text);
        }
        if (t.getType() == TokenType.ITALIC)
        {
            lexer.nextToken(); // consume ITALIC
            ParseTree child = text();
            if (lexer.nextToken().getType() != TokenType.ITALIC) throw new Exception($"Temp syntax error: expected \"**\" got \"{lexer.peekToken().literal}\" exception");
            return new Italics(child);
        }
        if (t.getType() == TokenType.BOLD)
        {
            lexer.nextToken(); // consume BOLD
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