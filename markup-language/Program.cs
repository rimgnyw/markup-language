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
    Scanner scanner = new Scanner(source);
    List<Token> tokens = scanner.scanTokens();

    foreach (Token token in tokens)
    {
        Console.Write(token.type);
        Console.Write(" ");
    }
    Console.WriteLine();
}
