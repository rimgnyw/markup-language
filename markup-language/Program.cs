// See https://aka.ms/new-console-template for more information
// Console.Write("> ");
// String? input = Console.ReadLine();

runPrompt();

// Console.WriteLine(input);
static void runPrompt()
{
    while (true)
    {
        Console.Write("> ");
        // TODO: replace with StreamReader later
        String? line = Console.ReadLine();
        if (line == null) break;
        run(line);
    }

}

static void run(String source)
{
    Lexer lexer = new Lexer(source);
    // List<Token> tokens = lexer.scanTokens();
    Parser parser = new Parser(lexer);
    ParseTree result = parser.parse();
    /* foreach (Token token in lexer.tokens)
    {
        Console.Write(token.type);
        Console.Write(" ");
    } */
    Console.WriteLine(result.process());
    Console.WriteLine();
}
