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
    static void Main(string[] args) {
        // runPrompt();
        runFile("../testfile.tst");
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
        Parser parser = new Parser(tokens);
        ParseTree result = parser.parse();
        return result.process();
    }

    static void runFile(String path) {
        byte[] bytes = File.ReadAllBytes(path);
        String content = Encoding.Default.GetString(bytes);
        // Console.WriteLine(content);
        string processed = run(content);
        string finalOutput = HTML_TEMPLATE_START + processed + HTML_TEMPLATE_END;
        File.WriteAllText("../output.html", processed);
        Console.WriteLine("saved to output.html");
    }
}