using System.Text;

class Program {
    private const string HTML_TEMPLATE_START = @"
<!DOCTYPE html>
<html lang=""en"">
    <head>
        <meta charset=""UTF-8"">
        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
        <title>Document</title>
    </head>
    <body>

";
    private const string HTML_TEMPLATE_END = @"
    </body>
</html>";
    private static bool hadError = false;
    static void Main(string[] args) {
        bool fullDoc = false;
        string? path = null;
        foreach (string arg in args) {
            // check for -d argument
            if (arg == "-d" && path == null)
                fullDoc = true;
            // first argument that isn't an option is assumed to be file path
            else if (path == null && !arg.StartsWith("-")) {
                path = arg;
            }
            else {
                throw new Exception("Invalid arguments");
            }

        }
        if (path == null)
            throw new Exception("Missing file");


        runFile(path, fullDoc);
        // runPrompt();
        // runFile("../test2.tst");
    }
    // Console.WriteLine(input);
    static void runPrompt() {
        while (true) {
            Console.Write("> ");
            // TODO: replace with StreamReader later
            String? line = Console.ReadLine();
            if (line == null) break;
            string processed = run(line);
            Console.WriteLine(processed);
            Console.WriteLine();
        }

    }

    static string run(String source) {
        Lexer lexer = new Lexer(source);
        List<Token> tokens = lexer.scanTokens();
        Console.WriteLine();
        foreach (Token token in tokens)
            Console.WriteLine(token.type);
        Console.WriteLine();
        Parser parser = new Parser(tokens);
        ParseTree result = parser.parse();

        // Had syntax error
        if (hadError)
            // exit on syntax error
            Environment.Exit(1);

        return result.process();
    }

    static void runFile(String path, bool fullDoc) {
        byte[] bytes = File.ReadAllBytes(path);
        String content = Encoding.Default.GetString(bytes);
        // Console.WriteLine(content);
        string processed = run(content);
        if (fullDoc) {
            string finalOutput = HTML_TEMPLATE_START + processed + HTML_TEMPLATE_END;
            File.WriteAllText("../output.html", finalOutput);
        }
        else
            File.WriteAllText("../output.html", processed);
        Console.WriteLine("saved to output.html");
    }

    public static ParseError error(int line, string message) {
        Console.WriteLine($"Error at line {line}: {message}");
        return new ParseError();
    }
    public static ParseError error(int line, string message, string type) {

        Console.WriteLine($"Error at line {line}: {message}");
        return new ParseError(type);
    }
}

// TODO: add more proper error types
class ParseError : Exception {
    public string type { get; set; }
    public ParseError(string type = "generic") {
        this.type = type;

    }
}